using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using XboxCsMgr.Helpers.Http;
using XboxCsMgr.XboxLive.Model.TitleHub;

namespace XboxCsMgr.XboxLive.Services
{
    public class TitleHubService : XboxLiveService
    {
        public TitleHubService(XboxLiveConfig config) : base(config, "https://titlehub.xboxlive.com")
        {
            HttpHeaders = new Dictionary<string, string>()
            {
                { "Accept-Language", System.Globalization.CultureInfo.CurrentCulture.ToString() },
                { "x-xbl-contract-version", "2" }
            };
        }

        /// <summary>
        /// Retreives the title history of the current authenticated user
        /// </summary>
        /// <returns>TitleDecoration collection</returns>
        public Task<TitleDecorationResult> GetTitleHistory()
        {
            return SignAndRequest<TitleDecorationResult>($"users/xuid({Config.UserOptions.XboxUserId})/titles/titlehistory/decoration/scid,image,detail", "");
        }
    }
}
