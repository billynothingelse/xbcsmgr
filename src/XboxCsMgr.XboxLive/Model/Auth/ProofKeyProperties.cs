using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace XboxCsMgr.XboxLive.Model.Auth
{
    public class ProofKeyProperties
    {
        [JsonPropertyName("kid")]
        public string KeyId { get; set; }

        [JsonPropertyName("kty")]
        public string KeyType { get; set; }

        [JsonPropertyName("alg")]
        public string Algorithm { get; set; }

        [JsonPropertyName("use")]
        public string Use { get; set; }

        [JsonPropertyName("crv")]
        public string Curve { get; set; }

        [JsonPropertyName("x")]
        public string X { get; set; }

        [JsonPropertyName("y")]
        public string Y { get; set; }
    }
}
