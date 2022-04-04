using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XboxCsMgr.XboxLive.Authentication;

namespace XboxCsMgr.XboxLive
{
    public class XboxLiveConfig
    {
        /// <summary>
        /// A 'full' XSTS authorized access token
        /// </summary>
        public string Token { get; internal set; }

        /// <summary>
        /// User-associated information for given token
        /// </summary>
        public XboxLiveUserOptions UserOptions { get; internal set; }

        public XboxLiveConfig(string token, XboxLiveUserOptions userOptions)
        {
            if (token == string.Empty || userOptions == null)
                return;

            Token = token;
            UserOptions = userOptions;
        }
    }
}
