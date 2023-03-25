using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using XboxCsMgr.Helpers.Http;
using XboxCsMgr.XboxLive.Model.Account;

namespace XboxCsMgr.XboxLive.Services
{
    public class AccountService : XboxLiveService
    {
        public AccountService(XboxLiveConfig config) : base(config, "https://accounts.xboxlive.com")
        {
            HttpHeaders = new Dictionary<string, string>()
            {
                { "Accept-Language", CultureInfo.CurrentCulture.ToString() },
                { "x-xbl-contract-version", "2" }
            };
        }

        /// <summary>
        /// Gets the current authenticated users account information
        /// </summary>
        /// <returns>AccountDetails - Contains a summary of profile information</returns>
        public Task<AccountDetails> GetAccountDetails()
        {
            return SignAndRequest<AccountDetails>($"users/current/profile", "");
        }
    }
}
