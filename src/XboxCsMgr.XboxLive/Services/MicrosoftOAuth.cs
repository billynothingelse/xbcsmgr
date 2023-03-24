using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using XboxCsMgr.XboxLive.Exceptions;
using XboxCsMgr.XboxLive.Model.Authentication;

// https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-auth-code-flow
// https://docs.microsoft.com/en-us/advertising/guides/authentication-oauth
// https://docs.microsoft.com/en-us/advertising/shopping-content/code-example-authentication-oauth

namespace XboxCsMgr.XboxLive.Services
{
    public class MicrosoftOAuth
    {
        const string OAuthAuthorize = "https://login.live.com/oauth20_authorize.srf";
        const string OAuthDesktop = "https://login.live.com/oauth20_desktop.srf";
        const string OAuthDesktopPath = "/oauth20_desktop.srf";
        const string OAuthErrorPath = "/err.srf";
        const string OAuthToken = "https://login.live.com/oauth20_token.srf";

        const string UserAgent = "Mozilla/5.0 (XboxReplay; XboxLiveAuth/3.0) " +
                                        "AppleWebKit/537.36 (KHTML, like Gecko) " +
                                        "Chrome/71.0.3578.98 Safari/537.36";

        protected readonly HttpClient httpClient;

        public MicrosoftOAuth(string clientId, string scope, HttpClient client)
        {
            this.ClientId = clientId;
            this.Scope = scope;
            this.httpClient = client;
        }

        public string ClientId { get; }
        public string Scope { get; }

        protected Dictionary<string, string?> createQueriesForAuth(string redirectUrl = OAuthDesktop)
        {
            return new Dictionary<string, string?>()
            {
                { "client_id", ClientId },
                { "grant_type", "authorization_code" },
                { "redirect_uri", redirectUrl },
                { "scope", Scope }
            };
        }

        private async Task<MicrosoftOAuthResponse> microsoftOAuthRequest(HttpRequestMessage req)
        {
            req.Headers.Add("User-Agent", UserAgent);
            req.Headers.Add("Accept-Encoding", "gzip");
            req.Headers.Add("Accept-Language", "en-US");

            var res = await httpClient.SendAsync(req)
                .ConfigureAwait(false);

            return await handleMicrosoftOAuthResponse(res);
        }

        internal async Task<MicrosoftOAuthResponse> handleMicrosoftOAuthResponse(HttpResponseMessage res)
        {
            var resBody = await res.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            try
            {
                res.EnsureSuccessStatusCode();
                var resObj = JsonSerializer.Deserialize<MicrosoftOAuthResponse>(resBody);
                if (resObj == null)
                    throw new MicrosoftOAuthException("Response was null", (int)res.StatusCode);

                if (resObj.ExpiresOn == default)
                    resObj.ExpiresOn = DateTimeOffset.Now.AddSeconds(resObj.ExpireIn);
                return resObj;
            }
            catch (Exception ex) when (
                ex is JsonException ||
                ex is HttpRequestException)
            {
                try
                {
                    throw MicrosoftOAuthException.FromResponseBody(resBody, (int)res.StatusCode);
                }
                catch (FormatException)
                {
                    throw new MicrosoftOAuthException($"{(int)res.StatusCode}: {res.ReasonPhrase}", (int)res.StatusCode);
                }
            }
        }

        public string CreateUrlForOAuth()
        {
            return CreateUrlForOAuth(new MicrosoftOAuthParameters());
        }

        public string CreateUrlForOAuth(MicrosoftOAuthParameters param)
        {
            if (string.IsNullOrEmpty(param.ResponseType))
                param.ResponseType = "code";

            var query = createQueriesForAuth();

            if (!string.IsNullOrEmpty(param.RedirectUri))
                query["redirect_uri"] = param.RedirectUri;
            if (!string.IsNullOrEmpty(param.ResponseMode))
                query["response_mode"] = param.ResponseMode;
            if (!string.IsNullOrEmpty(param.ResponseType))
                query["response_type"] = param.ResponseType;
            if (!string.IsNullOrEmpty(param.State))
                query["state"] = param.State;
            if (!string.IsNullOrEmpty(param.Prompt))
                query["prompt"] = param.Prompt;
            if (!string.IsNullOrEmpty(param.LoginHint))
                query["login_hint"] = param.LoginHint;
            if (!string.IsNullOrEmpty(param.DomainHint))
                query["domain_hint"] = param.DomainHint;
            if (!string.IsNullOrEmpty(param.CodeChallenge))
                query["code_challenge"] = param.CodeChallenge;
            if (!string.IsNullOrEmpty(param.CodeChallengeMethod))
                query["code_challenge_method"] = param.CodeChallengeMethod;
            
            return OAuthAuthorize + "?" + GetQueryString(query);
        }

        public bool CheckLoginSuccess(string url, out MicrosoftOAuthCode authCode)
        {
            var uri = new Uri(url);
            return CheckLoginSuccess(uri, out authCode);
        }

        public bool CheckLoginSuccess(Uri uri, out MicrosoftOAuthCode authCode)
        {
            var result = CheckOAuthCodeResult(uri, out authCode);
            return result && authCode.IsSuccess;
        }

        public bool CheckOAuthCodeResult(Uri uri, out MicrosoftOAuthCode authCode)
        {
            var query = HttpUtility.ParseQueryString(uri.Query);
            authCode = new MicrosoftOAuthCode
            {
                Code = query["code"],
                IdToken = query["id_token"],
                State = query["state"],
                Error = query["error"],
                ErrorDescription = HttpUtility.UrlDecode(query["error_description"])
            };

            return !authCode.IsEmpty;
        }

        public async Task<MicrosoftOAuthResponse> GetTokens(MicrosoftOAuthCode authCode)
        {
            if (authCode == null || !authCode.IsSuccess)
                throw new InvalidOperationException("AuthCode.IsSuccess was not true. Create AuthCode first.");

            var query = createQueriesForAuth();
            query["code"] = authCode.Code ?? "";

            var res = await microsoftOAuthRequest(new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(OAuthToken),
                Content = new FormUrlEncodedContent(query)
            }).ConfigureAwait(false);
            
            if (string.IsNullOrEmpty(res.IdToken))
                res.IdToken = authCode.IdToken;
            return res;
        }

        public async Task<MicrosoftOAuthResponse> RefreshToken(string refreshToken)
        {
            var query = createQueriesForAuth();
            query["refresh_token"] = refreshToken;
            query["grant_type"] = "refresh_token";

            return await microsoftOAuthRequest(new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(OAuthToken),
                Content = new FormUrlEncodedContent(query)
            }).ConfigureAwait(false);
        }

        public static string GetSignOutUrl()
        {
            return "https://login.microsoftonline.com/consumer/oauth2/v2.0/logout";
        }

        public static string GetQueryString(Dictionary<string, string?> queries)
        {
            return string.Join("&",
                queries.Select(x => $"{x.Key}={HttpUtility.UrlEncode(x.Value)}"));
        }
    }
}
