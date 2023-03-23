using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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

            Config = config;
            HttpBaseUrl = baseUrl;

            HttpClient.BaseAddress = new Uri(HttpBaseUrl);
            //HttpClient.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("XBL3.0", $"x={Config.UserOptions.UserHash};{Config.Token}");

            Security = new XboxLiveSecurity();
        }

        public async Task<T> SignAndRequest<T>(string uri, object body, string token)
        {
            var bodyStr = JsonSerializer.Serialize(body);

            var reqMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(uri),
                Method = HttpMethod.Post,
                Content = new StringContent(bodyStr, Encoding.UTF8, "application/json")
            };

            if (token == null)
                token = "";
            var signature = Security.GenerateSignature(uri, token, bodyStr);
            reqMessage.Headers.Add("Signature", signature);
            reqMessage.Headers.Add("x-xbl-contract-version", "2");

            var res = await HttpClient.SendAsync(reqMessage);
            return await HandleResponse<T>(res);
        }

        public static async Task<T> HandleResponse<T>(HttpResponseMessage res)
        {
            var resBody = await res.Content.ReadAsStringAsync()
                .ConfigureAwait(false);

            try
            {
                res.EnsureSuccessStatusCode();
                return JsonSerializer.Deserialize<T>(resBody)
                    ?? throw new JsonException();
            }
            catch (Exception ex) when (
                ex is JsonException ||
                ex is HttpRequestException)
            {
                try
                {
                    throw XboxAuthException.FromResponseBody(resBody, (int)res.StatusCode);
                }
                catch (FormatException)
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
}
