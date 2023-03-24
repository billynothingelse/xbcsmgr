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
        }

        /// <summary>
        /// Retreives the title history of the current authenticated user
        /// </summary>
        /// <returns>TitleDecoration collection</returns>
        public Task<TitleDecorationResult> GetTitleHistory()
        {
            return SignAndRequest<TitleDecorationResult>($"users/xuid({Config.UserOptions.XboxUserId})/titles/titlerecord/decoration/titleRecord?maxItems=32&filterTo=isGame", "");
        }
    }
}
