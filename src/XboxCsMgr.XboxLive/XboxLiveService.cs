using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace XboxCsMgr.XboxLive
{
    public class XboxLiveService
    {
        protected HttpClient HttpClient { get; }
        public string HttpBaseUrl { get; internal set; }
        public Dictionary<string, string> HttpHeaders { get; internal set; }

        public XboxLiveConfig Config { get; internal set; }

        public XboxLiveService(XboxLiveConfig config, string baseUrl)
        {
            HttpClient = new HttpClient();

            Config = config;
            HttpBaseUrl = baseUrl;

            HttpClient.BaseAddress = new Uri(HttpBaseUrl);
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("XBL3.0", $"x={Config.UserOptions.UserHash};{Config.Token}");
        }
    }
}
