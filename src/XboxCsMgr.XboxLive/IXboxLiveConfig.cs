using XboxCsMgr.XboxLive.Model.Auth;

namespace XboxCsMgr.XboxLive
{
    public interface IXboxLiveConfig
    {
        string Token { get; }
        XboxLiveUserOptions UserOptions { get; }
    }
}
