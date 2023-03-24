using Newtonsoft.Json;
using System.Collections.Generic;

namespace XboxCsMgr.XboxLive.Model.Authentication
{
    public class XboxLiveAuthenticateRequest
    {
        public string RelyingParty { get; set; }

        public string TokenType { get; set; }

        public Dictionary<string, object> Properties { get; set; }
    }
}
