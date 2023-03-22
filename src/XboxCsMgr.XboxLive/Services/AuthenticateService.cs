using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XboxCsMgr.Helpers.Http;
using XboxCsMgr.Helpers.Serialization;
using XboxCsMgr.XboxLive.Model.Auth;

namespace XboxCsMgr.XboxLive.Services
{
    /// <summary>
    /// Provide the multiple stages of Xbox Live authentication
    /// and service authorization.
    /// </summary>
    public static class AuthenticateService
    {
        /// <summary>
        /// Authorizes a user and the optional device and title token(s) to retrieve an
        /// XToken.
        /// </summary>
        /// <param name="userToken"></param>
        /// <param name="deviceToken"></param>
        /// <param name="titleToken"></param>
        /// <returns>An authenticated XToken</returns>
        public async static Task<XboxLiveAuthenticateResponse<XboxLiveDisplayClaims>> AuthenticateXstsAsync(
            string userToken, string deviceToken = null, string titleToken = null)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://xsts.auth.xboxlive.com");

            var request = new HttpRequestMessage(HttpMethod.Post, "xsts/authorize");
            var requestBody = new XboxLiveAuthenticateRequest
            {
                RelyingParty = "http://xboxlive.com",
                TokenType = "JWT",
                Properties = new Dictionary<string, object>
                {
                    { "UserTokens", new string[] { userToken } },
                    { "SandboxId", "RETAIL" }
                }
            };

            if (deviceToken != null)
                requestBody.Properties.Add("DeviceToken", deviceToken);
            if (titleToken != null)
                requestBody.Properties.Add("TitleToken", titleToken);

            request.Headers.Add("x-xbl-contract-version", "1");
            request.Content = new JsonContent(requestBody);

            // todo; handle any bonkening and fix this
            // bad blocking but hopefully temporary
            //var response = await client.SendAsync(request);
            var task = client.SendAsync(request);
            var response = task.GetAwaiter().GetResult();
            var data = await response.Content.ReadAsJsonAsync<XboxLiveAuthenticateResponse<XboxLiveDisplayClaims>>();
            return data;
        }
    }
}
