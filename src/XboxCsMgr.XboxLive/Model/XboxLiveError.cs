using System.Text.Json.Serialization;

namespace XboxCsMgr.XboxLive.Model
{
    public class XboxLiveErrorResponse
    {
        [JsonPropertyName("XErr")]
        public string? XErr { get; set; }

        [JsonPropertyName("Message")]
        public string? Message { get; set; }

        [JsonPropertyName("Redirect")]
        public string? Redirect { get; set; }
    }
}
