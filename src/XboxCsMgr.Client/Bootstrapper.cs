using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stylet;
using StyletIoC;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using XboxCsMgr.Client.ViewModels;
using XboxCsMgr.Helpers.Win32;
using XboxCsMgr.XboxLive;
using XboxCsMgr.XboxLive.Account;
using XboxCsMgr.XboxLive.Authentication;
using XboxCsMgr.XboxLive.TitleHub;

namespace XboxCsMgr.Client
{
    public class AppBootstrapper : Bootstrapper<ShellViewModel>
    {
        public static XboxLiveConfig? XblConfig { get; internal set; }

        protected override void ConfigureIoC(StyletIoC.IStyletIoCBuilder builder)
        {
            base.ConfigureIoC(builder);
        }

        protected async void InitializeXboxLive()
        {
            // Lookup current Xbox Live authentication data stored via wincred
            Dictionary<string, string> currentCredentials = CredentialUtil.EnumerateCredentials();

            string userToken = string.Empty;
            string deviceToken = string.Empty;

            if (currentCredentials != null)
            {
                // Temporary scratch
                foreach (var cred in currentCredentials)
                {
                    if (!cred.Key.Contains("XblGrts") && cred.Key.Contains("Dtoken"))
                    {
                        // Fixes an odd issue where unexpected character will be at the end of the value
                        var FixedJson = cred.Value.ToString().TrimEnd('X').ToString();
                        XboxLiveToken? cachedToken = JsonConvert.DeserializeObject<XboxLiveToken>(FixedJson);
                        if (cachedToken != null && deviceToken == string.Empty)
                        {
                            deviceToken = cachedToken.TokenData.Token;
                        }
                    }

                    if (!cred.Key.Contains("XblGrts") && cred.Key.Contains("Utoken"))
                    {
                        var FixedJson = cred.Value.ToString().TrimEnd('X').ToString();
                        XboxLiveToken? cachedToken = JsonConvert.DeserializeObject<XboxLiveToken>(cred.Value);
                        if (cachedToken != null && userToken == string.Empty)
                        {
                            userToken = cachedToken.TokenData.Token;
                        }
                    }
                }
            }

            var response = await AuthenticateService.AuthenticateXstsAsync(userToken, deviceToken);
            if (response != null)
            {
                XblConfig = new XboxLiveConfig(response.Token, response.DisplayClaims.XboxUserIdentity[0]);
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            InitializeXboxLive();
        }
    }
}
