using Newtonsoft.Json;

namespace XboxCsMgr.XboxLive.Model.Authentication
{
    public class ProofKeyProperties
    {
        [JsonProperty("kid")]
        public string KeyId { get; set; }

        [JsonProperty("kty")]
        public string KeyType { get; set; }

        [JsonProperty("alg")]
        public string Algorithm { get; set; }

        [JsonProperty("use")]
        public string Use { get; set; }

        [JsonProperty("crv")]
        public string Curve { get; set; }

        [JsonProperty("x")]
        public string X { get; set; }

        [JsonProperty("y")]
        public string Y { get; set; }
    }
}
