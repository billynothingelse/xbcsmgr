using Newtonsoft.Json;

namespace XboxCsMgr.XboxLive.Model.Auth
{
    public class XboxLiveDeviceOptions
    {
        [JsonProperty("did")]
        public string DeviceId { get; set; }
    }

    public class XboxLiveUserOptions
    {
        [JsonProperty("agg")]
        public string AgeGate { get; set; }

        [JsonProperty("gtg")]
        public string Gamertag { get; set; }

        [JsonProperty("uhs")]
        public string UserHash { get; set; }

        [JsonProperty("prv")]
        public string Privileges { get; set; }

        [JsonProperty("xid")]
        public long XboxUserId { get; set; }
    }

    public class XboxLiveDisplayClaims
    {
        [JsonProperty("xui")]
        public XboxLiveUserOptions[] XboxUserIdentity { get; set; }

        [JsonProperty("xdi")]
        public XboxLiveDeviceOptions XboxDeviceIdentity { get; set; }
    }
}
