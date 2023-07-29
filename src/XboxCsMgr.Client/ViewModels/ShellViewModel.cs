using Stylet;
using System;
using XboxCsMgr.Client.Events;
using XboxCsMgr.XboxLive;
using XboxCsMgr.XboxLive.Services;

namespace XboxCsMgr.Client.ViewModels
{
    public interface IDialogFactory
    {
        LoginViewModel CreateLoginDialog();
    }

    /// <summary>
    /// The ShellViewModel represents the ShellView which is the base window
    /// of the application. It's responsible for containing and managing other
    /// sub-views, etc.
    /// </summary>
    public class ShellViewModel : Screen, IHandle<LoadSaveDetailsEvent>
    {
        private readonly IWindowManager windowManager;
        private readonly IDialogFactory dialogFactory;

        private IEventAggregator _events;
        private XboxLiveConfig? _xblConfig => AppBootstrapper.XblConfig;

        private GameViewModel? _gameView;
        public GameViewModel? GameView
        {
            get => _gameView;
            set => SetAndNotify(ref this._gameView, value);
        }

        private SaveViewModel? _saveView;
        public SaveViewModel? SaveView
        {
            get => _saveView;
            set => SetAndNotify(ref this._saveView, value);
        }

        private LoginViewModel? _loginView;
        public LoginViewModel? LoginView
        {
            get => _loginView;
            set => SetAndNotify(ref this._loginView, value);
        }

        public ShellViewModel(IWindowManager windowManager, IDialogFactory dialogFactory, IEventAggregator events)
        {
            this.windowManager = windowManager;
            this.dialogFactory = dialogFactory;
            _events = events;

            _events.Subscribe(this);
        }

        public void Handle(LoadSaveDetailsEvent message)
        {
            Screen vm;
            vm = new SaveViewModel(_events, _xblConfig, message.PackageFamilyName, message.ServiceConfigurationId);
            SaveView = (SaveViewModel)vm;
        }
        
        /// <summary>
        /// Called from the boostrapper once authentication is ready
        /// </summary>
        public void OnAuthComplete()
        {
            GameView = new GameViewModel(_events, _xblConfig);
        }
    }
}
