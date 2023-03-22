using XboxCsMgr.XboxLive.Model.TitleStorage;
using XboxCsMgr.XboxLive.Services;

namespace XboxCsMgr.Client.ViewModels.Controls
{
    public class SavedBlobsViewModel : TreeViewItemViewModel
    {
        private TitleStorageService _storageService;
        private TitleStorageBlobMetadata _blobMetadata;

        public TitleStorageBlobMetadata BlobMetadata
        {
            get => _blobMetadata;
        }

        public string BlobName
        {
            get => _blobMetadata.FileName;
        }

        public SavedBlobsViewModel(TitleStorageService storageService, TitleStorageBlobMetadata blobMetadata) : base(null, true)
        {
            _storageService = storageService;
            _blobMetadata = blobMetadata;
        }

        protected override async void LoadChildren()
        {
            TitleStorageAtomMetadataResult atoms = await _storageService.GetBlobAtomsAsync(_blobMetadata.FileName);
            foreach (string atom in atoms.Atoms.Keys)
                base.Children.Add(new SavedAtomsViewModel(null, atom, atoms.Atoms[atom], this));
        }
    }
}
