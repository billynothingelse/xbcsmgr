using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using XboxCsMgr.Client.Events;
using XboxCsMgr.XboxLive;
using XboxCsMgr.XboxLive.TitleHub;

namespace XboxCsMgr.Client.ViewModels
{
    public class GameViewModel : Screen
    {
        private IEventAggregator _events;
        private XboxLiveConfig _xblConfig;
        private TitleHubService _titleHubService;

        private ObservableCollection<TitleDecoration> _gameCollection;
        public ObservableCollection<TitleDecoration> GameCollection
        {
            get => _gameCollection;
            set => SetAndNotify(ref _gameCollection, value);
        }

        public IEnumerable<TitleDecoration> FilteredGameCollection
        {
            get
            {
                return GameCollection;
            }
        }

        public TitleDecoration SelectedTitle { get; set; }

        public GameViewModel(IEventAggregator events, XboxLiveConfig xblConfig)
        {
            _events = events;
            _xblConfig = xblConfig;
            _titleHubService = new TitleHubService(_xblConfig);
            _gameCollection = new ObservableCollection<TitleDecoration>();
        }

        protected override async void OnViewLoaded()
        {
            base.OnViewLoaded();

            TitleDecorationResult result = await _titleHubService.GetTitleHistoryAsync();
            if (result != null)
            {
                foreach (TitleDecoration titleDecoration in result.Titles)
                    if (!titleDecoration.Devices.Contains("Win32") && !titleDecoration.Devices.Contains("Xbox360"))
                        _gameCollection.Add(titleDecoration);
            }
        }

        /// <summary>
        /// Updates the current selected game
        /// </summary>
        public void LoadSelected()
        {
            if (this.SelectedTitle == null)
                return;

            _events.Publish(new LoadSaveDetailsEvent()
            {
                ServiceConfigurationId = this.SelectedTitle.ServiceConfigId,
                PackageFamilyName = this.SelectedTitle.Pfn
            });
        }
    }
}
