using System;
using Stylet;
using XboxCsMgr.Client.ViewModels;
using XboxCsMgr.XboxLive;
using XboxCsMgr.Helpers.Win32;
using Newtonsoft.Json;
using System.Collections.Generic;
using XboxCsMgr.XboxLive.Model.Authentication;
using XboxCsMgr.XboxLive.Services;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;

namespace XboxCsMgr.Client
{
    public class AppBootstrapper : Bootstrapper<ShellViewModel>
    {
        public static XboxLiveConfig? XblConfig { get; internal set; }
        private AuthenticateService authenticateService;
        private string DeviceToken { get; set; }
        private string UserToken { get; set; } 

        protected override void ConfigureIoC(StyletIoC.IStyletIoCBuilder builder)
        {
            base.ConfigureIoC(builder);

            builder.Bind<IDialogFactory>().ToAbstractFactory();
        }

        protected override async void OnStart()
        {
            authenticateService = new AuthenticateService(XblConfig);

            LoadXblTokenCredentials();

            var result = await authenticateService.AuthorizeXsts(UserToken, DeviceToken);
            if (result != null)
            {
                Debug.WriteLine("Authorized! Token: " + result.Token);
                XblConfig = new XboxLiveConfig(result.Token, result.DisplayClaims.XboxUserIdentity[0]);
                this.RootViewModel.OnAuthComplete();
            }

            base.OnStart();
        }

        private void LoadXblTokenCredentials()
        {
            // Lookup current Xbox Live authentication data stored via wincred
            Dictionary<string, string> currentCredentials = CredentialUtil.EnumerateCredentials();

            var xblCredentials = currentCredentials.Where(k => k.Key.Contains("Xbl|")
                    && k.Key.Contains("Dtoken") 
                    || k.Key.Contains("Utoken"))
                    .ToDictionary(p => p.Key, p => p.Value);

            foreach (var credential in xblCredentials)
            {
                // Remove trailing 'X' that is found on some credentials
                var fixedJson = credential.Value.TrimEnd('X').ToString();
                XboxLiveToken? token = JsonConvert.DeserializeObject<XboxLiveToken>(fixedJson);
                if (token.TokenData.NotAfter > DateTime.UtcNow)
                {
                    if (credential.Key.Contains("Dtoken"))
                    {
                        DeviceToken = token.TokenData.Token;
                    }
                    else if (credential.Key.Contains("Utoken"))
                    {
                        if (token.TokenData.Token != "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA") UserToken = token.TokenData.Token;
                    }
                }
            }
        }
    }
}
