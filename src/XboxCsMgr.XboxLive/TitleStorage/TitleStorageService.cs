using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XboxCsMgr.Helpers;
using XboxCsMgr.Helpers.Http;

namespace XboxCsMgr.XboxLive.TitleStorage
{
    public class TitleStorageService : XboxLiveService
    {
        private string ServiceConfigurationId;
        private string PackageFamilyName;

        private const int MinBlockUploadSize = 1024;
        private const int MaxBlockUploadSize = 4 * 1024 * 1024;
        private const int DefaultBlockUploadSize = 256 * 1024;

        public TitleStorageService(XboxLiveConfig config) : base(config, "https://titlestorage.xboxlive.com")
        {
            HttpHeaders = new Dictionary<string, string>()
            {
                { "x-xbl-contract-version", "107" }
            };
        }

        public TitleStorageService(XboxLiveConfig config, string scid, string pfn)
            : base(config, "https://titlestorage.xboxlive.com")
        {
            ServiceConfigurationId = scid;
            PackageFamilyName = pfn;

            HttpHeaders = new Dictionary<string, string>()
            {
                { "x-xbl-contract-version", "107" },
                { "x-xbl-pfn", PackageFamilyName }
            };
        }
    }
}
