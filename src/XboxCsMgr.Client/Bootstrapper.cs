using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stylet;
using StyletIoC;
using System.Collections.Generic;
using System.Diagnostics;
using XboxCsMgr.Client.ViewModels;
using XboxCsMgr.Helpers.Win32;
using XboxCsMgr.XboxLive.Authentication;

namespace XboxCsMgr.Client
{
    public class AppBootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override async void OnLaunch()
        {
            base.OnLaunch();

            // Lookup current Xbox Live authentication data stored via wincred
            Dictionary<string, string> currentCredentials = CredentialUtil.EnumerateCredentials();

            string userToken = string.Empty;
            string deviceToken = string.Empty;

            if (currentCredentials != null)
            {
                // Temporary scratch
                foreach (var cred in currentCredentials)
                {
                    if (cred.Key.Contains("Dtoken"))
                    {
                        var data = (JObject)JsonConvert.DeserializeObject(cred.Value);
                        deviceToken = data["TokenData"]["Token"].Value<string>();
                    }

                    if (cred.Key.Contains("Utoken"))
                    {
                        var data = (JObject)JsonConvert.DeserializeObject(cred.Value);
                        userToken = data["TokenData"]["Token"].Value<string>();
                    }
                }

                var response = await AuthenticateService.AuthenticateXstsAsync(userToken, deviceToken);
                if (response != null)
                {
                    string token = string.Format("XBL3.0 x={0};{1}", response.DisplayClaims.XboxUserIdentity[0].UserHash, response.Token);
                    Debug.WriteLine("Auth token: {0}", token);
                }
            }
        }
    }
}
