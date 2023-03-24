using Microsoft.Web.WebView2.Core;
using Stylet;
using System;
using System.Net.Http;
using System.Windows.Controls;
using XboxCsMgr.XboxLive.Model.Authentication;
using XboxCsMgr.XboxLive.Services;

namespace XboxCsMgr.Client.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly MicrosoftOAuth _microsoftAuth;
        private readonly HttpClient _httpClient;

        private Uri? _loginUrl;
        public Uri? LoginUrl
        {
            get => _loginUrl;
            set => SetAndNotify(ref _loginUrl, value);
        }

        public string AccessToken { get; set; }

        public LoginViewModel()
        {
            _httpClient = new HttpClient();
            _microsoftAuth = new MicrosoftOAuth("000000004c12ae6f", "service::user.auth.xboxlive.com::MBI_SSL", _httpClient);
            LoginUrl = new Uri(_microsoftAuth.CreateUrlForOAuth());
        }

        public async void OnNavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            if (e.IsRedirected && _microsoftAuth.CheckOAuthCodeResult(new Uri(e.Uri), out var authCode))
            {
                if (!authCode.IsSuccess)
                {
                    return;
                }

                var result = await _microsoftAuth.GetTokens(authCode);
                if (result != null && !string.IsNullOrEmpty(result.AccessToken))
                {
                    AccessToken = result.AccessToken;
                    this.RequestClose(true);
                }
            }
        }
    }
}
