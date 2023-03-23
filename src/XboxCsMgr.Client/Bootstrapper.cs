using Stylet;
using XboxCsMgr.Client.ViewModels;
using XboxCsMgr.XboxLive;

namespace XboxCsMgr.Client
{
    public class AppBootstrapper : Bootstrapper<ShellViewModel>
    {
        public static XboxLiveConfig? XblConfig { get; internal set; }

        protected override void ConfigureIoC(StyletIoC.IStyletIoCBuilder builder)
        {
            base.ConfigureIoC(builder);
        }

        protected override void OnStart()
        {
            base.OnStart();
        }
    }
}
