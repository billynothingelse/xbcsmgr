using XboxCsMgr.XboxLive.Model.Authentication;

namespace XboxCsMgr.XboxLive
{
    public class XboxLiveConfig : IXboxLiveConfig
    {
        /// <summary>
        /// A 'full' XSTS authorized access token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// User-associated information for given token
        /// </summary>
        public XboxLiveUserOptions UserOptions { get; set; }

        public XboxLiveConfig(string token, XboxLiveUserOptions userOptions)
        {
            Token = token;
            UserOptions = userOptions;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(UserOptions.UserHash) || !string.IsNullOrEmpty(Token);
        }
    }
}
