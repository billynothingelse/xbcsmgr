using XboxCsMgr.XboxLive.Model.Authentication;

namespace XboxCsMgr.XboxLive
{
    public interface IXboxLiveConfig
    {
        string Token { get; }
        XboxLiveUserOptions UserOptions { get; }
    }
}
