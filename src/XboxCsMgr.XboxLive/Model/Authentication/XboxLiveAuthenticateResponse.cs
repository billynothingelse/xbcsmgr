using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XboxCsMgr.XboxLive.Model.Authentication
{
    public class XboxLiveAuthenticateResponse<T>
    {
        public string Token { get; set; }

        public DateTime NotAfter { get; set; }

        public DateTime IssueInstant { get; set; }

        public bool ClientAttested { get; set; }

        public T DisplayClaims { get; set; }
    }
}
