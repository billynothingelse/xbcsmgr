using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace XboxCsMgr.XboxLive.Model.Authentication
{
    public class MicrosoftOAuthResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpireIn { get; set; }
        
        [JsonPropertyName("expires_on")]
        public DateTimeOffset ExpiresOn { get; set; }

        [JsonPropertyName("scope")]
        public string? Scope { get; set; }

        [JsonPropertyName("refresh_token")]
        public string? RawRefreshToken { get; set; }

        [JsonIgnore]
        public string? RefreshToken
        {
            get => RawRefreshToken?.Split('.')?.Last();
            set => RawRefreshToken = "M.R3_BAY." + value;
        }

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }
        
        [JsonPropertyName("id_token")]
        public string? IdToken { get; set; }
    }
}
