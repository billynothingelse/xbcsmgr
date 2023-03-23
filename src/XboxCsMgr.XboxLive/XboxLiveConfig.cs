using XboxCsMgr.XboxLive.Model.Auth;

namespace XboxCsMgr.XboxLive
{
    public class XboxLiveConfig : IXboxLiveConfig
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
            Token = token;
            UserOptions = userOptions;
        }
    }
}
