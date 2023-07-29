using Newtonsoft.Json;

namespace XboxCsMgr.XboxLive.Model
{
    public class XboxLiveErrorResponse
    {
        [JsonProperty("XErr")]
        public string XErr { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("Redirect")]
        public string Redirect { get; set; }
    }
}
