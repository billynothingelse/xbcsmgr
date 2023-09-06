using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XboxCsMgr.Helpers.Http;
using XboxCsMgr.Helpers.Serialization;
using XboxCsMgr.XboxLive.Exceptions;

namespace XboxCsMgr.XboxLive
{
    public class XboxLiveService
    {
        protected HttpClient HttpClient { get; }
        public string HttpBaseUrl { get; internal set; }
        public Dictionary<string, string> HttpHeaders { get; internal set; }

        public XboxLiveConfig Config { get; internal set; }

        public XboxLiveSecurity Security { get; internal set; }

        public XboxLiveService(XboxLiveConfig config, string baseUrl)
        {
            HttpClient = new HttpClient();
            HttpBaseUrl = baseUrl;
            HttpClient.BaseAddress = new Uri(HttpBaseUrl);
            Config = config;

            Security = new XboxLiveSecurity();

            if (Config != null && Config.IsValid())
            {
                HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("XBL3.0", $"x={Config.UserOptions.UserHash};{Config.Token}");
            }
        }

        public async Task<T> SignAndRequest<T>(string uri, object body, string token)
        {
            var bodyStr = NewtonsoftJsonSerializer.Create(JsonNamingStrategy.Default).Serialize(body);

            var reqMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(uri, UriKind.RelativeOrAbsolute),
                Method = HttpMethod.Post,
                Content = new JsonContent(body)
            };

            if (token == null)
                token = "";
            var signature = Security.GenerateSignature(uri, token, bodyStr);
            reqMessage.Headers.Add("Signature", signature);
            reqMessage.Headers.Add(HttpHeaders);

            var res = await HttpClient.SendAsync(reqMessage);
            return await HandleResponse<T>(res);
        }

        public async Task<T> SignAndRequest<T>(string uri, StringContent body, string token)
        {
            var bodyStr = NewtonsoftJsonSerializer.Create(JsonNamingStrategy.Default).Serialize(body);

            var reqMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(uri, UriKind.RelativeOrAbsolute),
                Method = HttpMethod.Post,
                Content = body
            };

            if (token == null)
                token = "";
            var signature = Security.GenerateSignature(uri, token, bodyStr);
            reqMessage.Headers.Add("Signature", signature);
            reqMessage.Headers.Add(HttpHeaders);

            var res = await HttpClient.SendAsync(reqMessage);
            return await HandleResponse<T>(res);
        }

        public async Task<T> SignAndRequest<T>(string uri, string token, string method = "GET")
        {
            var reqMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(uri, UriKind.RelativeOrAbsolute),
                Method = new HttpMethod(method)
            };

            if (token == null)
                token = "";
            reqMessage.Headers.Add("Accept-Language", System.Globalization.CultureInfo.CurrentCulture.ToString());
            reqMessage.Headers.Add(HttpHeaders);

            var res = await HttpClient.SendAsync(reqMessage);
            return await HandleResponse<T>(res);
        }

        public static async Task<T> HandleResponse<T>(HttpResponseMessage res)
        {
            try
            {
                res.EnsureSuccessStatusCode();
                return await res.Content.ReadAsJsonAsync<T>()
                    .ConfigureAwait(false)
                    ?? throw new JsonException();
            }
            catch (Exception ex) when (
                ex is JsonException ||
                ex is HttpRequestException)
            {
                try
                {
                    throw XboxAuthException.FromResponseHeaders(res.Headers, (int)res.StatusCode);
                }
                catch (FormatException)
                {
                    throw new XboxAuthException($"{(int)res.StatusCode}: {res.ReasonPhrase}", (int)res.StatusCode);
                }
            }
        }
    }
}
