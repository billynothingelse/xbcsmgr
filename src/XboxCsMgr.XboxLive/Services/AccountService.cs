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
        const string AccountDetailsUrl = "https://accounts.xboxlive.com/users/current/profile";

        public AccountService(XboxLiveConfig config) : base(config, "https://accounts.xboxlive.com")
        {
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
