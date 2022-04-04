using Stylet;
using XboxCsMgr.XboxLive;

namespace XboxCsMgr.Client.ViewModels
{
    /// <summary>
    /// The ShellViewModel represents the ShellView which is the base window
    /// of the application. It's responsible for containing and managing other
    /// sub-views, etc.
    /// </summary>
    public class ShellViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private IXboxLiveConfig _xblConfig => AppBootstrapper.XblConfig;

        public ShellViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }
    }
}
