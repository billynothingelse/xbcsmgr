using System;
using Newtonsoft.Json;

namespace XboxCsMgr.XboxLive.TitleStorage
{
    public class TitleStorageBlobMetadata
    {
        public DateTimeOffset ClientFileTime { get; set; }

        public string DisplayName { get; set; }

        public string ETag { get; set; }

        public string FileName { get; set; }

        public ulong Size { get; set; }
    }
}
