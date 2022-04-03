using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XboxCsMgr.XboxLive.Authentication
{
    public class XboxLiveAuthenticateResponse<T>
    {
        public DateTime IssueInstant { get; set; }

        public DateTime NotAfter { get; set; }

        public string Token { get; set; }

        public T DisplayClaims { get; set; }
    }
}
