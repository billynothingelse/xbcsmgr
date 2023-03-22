using Newtonsoft.Json;

namespace XboxCsMgr.XboxLive.Model.Auth
{
    public class XboxLiveToken
    {
        public bool HasSignInDisplayClaims { get; set; }

        public string IdentityType { get; set; }

        public string Sandbox { get; set; }

        public string TokenType { get; set; }

        public string RelyingParty { get; set; }

        public string SubRelyingParty { get; set; }

        public XboxLiveAuthenticateResponse<XboxLiveDisplayClaims> TokenData { get; set; }
    }
}
