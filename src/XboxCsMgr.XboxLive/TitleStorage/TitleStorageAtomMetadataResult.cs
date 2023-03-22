using System.Collections.Generic;

namespace XboxCsMgr.XboxLive.TitleStorage
{
    public class TitleStorageAtomData
    {
        public string Name { get; set; }

        public string Atom { get; set; }

        public string Data { get; set; }

        public string GameData { get; set; }

        public string MetaData { get; set; }
    }

    public class TitleStorageAtomMetadataResult
    {
        public Dictionary<string, string> Atoms { get; set; }
    }
}
