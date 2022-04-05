using XboxCsMgr.XboxLive.TitleStorage;

namespace XboxCsMgr.Client.Events
{
    public class LoadSaveDetailsEvent
    {
        public string ServiceConfigurationId { get; set; }

        public string PackageFamilyName { get; set; }

        public TitleStorageBlobMetadata Blob { get; set; }
    }
}
