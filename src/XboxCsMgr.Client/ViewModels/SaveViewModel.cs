using Stylet;
using System.Collections.ObjectModel;
using System.IO;
using XboxCsMgr.Client.ViewModels.Controls;
using XboxCsMgr.XboxLive;
using XboxCsMgr.XboxLive.Model.TitleStorage;
using XboxCsMgr.XboxLive.Services;

namespace XboxCsMgr.Client.ViewModels
{
    public class SaveViewModel : Screen
    {
        private IEventAggregator _events;
        private XboxLiveConfig _xblConfig;
        private TitleStorageService _storageService;

        private string _packageFamilyName;
        public string PackageFamilyName
        {
            get => _packageFamilyName;
            set
            {
                _packageFamilyName = value;
            }
        }

        private string _serviceConfigurationId;
        public string ServiceConfigurationId
        {
            get => _serviceConfigurationId;
            set
            {
                _serviceConfigurationId = value;
            }
        }

        private ObservableCollection<SavedBlobsViewModel> _saveData;
        public ObservableCollection<SavedBlobsViewModel> SaveData
        {
            get => _saveData;
        }

        public SavedAtomsViewModel? SelectedAtom
        {
            get;
            set;
        }

        public SaveViewModel(IEventAggregator events, XboxLiveConfig config, string pfn, string scid)
        {
            _events = events;
            _xblConfig = config;
            _packageFamilyName = pfn;
            _serviceConfigurationId = scid;

            _storageService = new TitleStorageService(_xblConfig, _packageFamilyName, _serviceConfigurationId);
            _saveData = new ObservableCollection<SavedBlobsViewModel>();

            FetchSaveMetadata();
        }

        private async void FetchSaveMetadata()
        {
            TitleStorageBlobMetadataResult blobMetadataResult = await _storageService.GetBlobMetadata();
            if (blobMetadataResult != null && blobMetadataResult.Blobs != null)
            {
                foreach (TitleStorageBlobMetadata entry in blobMetadataResult.Blobs)
                {
                    _saveData.Add(new SavedBlobsViewModel(_storageService, entry));
                }
            }
        }

        public void SelectedItemChanged(object args)
        {
            SelectedAtom = args as SavedAtomsViewModel;
        }

        public async void Download()
        {
            if (SelectedAtom == null)
                return;

            string atom = SelectedAtom.AtomValue;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = SelectedAtom.AtomName;

            bool? res = dlg.ShowDialog();
            if (res == true)
            {
                byte[] atomData = await _storageService.DownloadAtomAsync(atom);
                await File.WriteAllBytesAsync(dlg.FileName, atomData);
            }
        }

        public async void Upload()
        {
            if (SelectedAtom == null)
                return;

            // Remove the file type, just need the UUID
            string atom = SelectedAtom.AtomValue.Substring(0, SelectedAtom.AtomValue.IndexOf(','));
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = SelectedAtom.AtomName;

            bool? res = dlg.ShowDialog();
            if (res == true)
            {
                byte[] atomData = await File.ReadAllBytesAsync(dlg.FileName);
                try
                {
                    var t = await _storageService.UploadBlobAsync(null, atomData, atom);
                    var e = 1 + 1;
                }
                catch
                {

                }
            }
        }
    }
}
