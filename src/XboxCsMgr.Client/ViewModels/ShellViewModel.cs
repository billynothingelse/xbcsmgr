using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stylet;
using System;
using System.Collections.Generic;
using XboxCsMgr.Helpers.Win32;
using XboxCsMgr.XboxLive;
using XboxCsMgr.XboxLive.Authentication;
using XboxCsMgr.XboxLive.Account;

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
        private XboxLiveConfig _xblConfig => AppBootstrapper.XblConfig;

        private GameViewModel _gameView;
        public GameViewModel GameView
        {
            get => _gameView;
            set => SetAndNotify(ref this._gameView, value);
        }

        public ShellViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        protected override void OnViewLoaded()
        {
            base.OnViewLoaded();

            GameView = new GameViewModel(_xblConfig);
        }
    }
}
