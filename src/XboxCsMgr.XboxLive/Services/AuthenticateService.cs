using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using XboxCsMgr.Helpers.Http;
using XboxCsMgr.Helpers.Serialization;
using XboxCsMgr.XboxLive.Model.Authentication;

namespace XboxCsMgr.XboxLive.Services
{
    /// <summary>
    /// Provide the multiple stages of Xbox Live authentication
    /// and service authorization.
    /// </summary>
    public class AuthenticateService : XboxLiveService
    {
        const string AuthorizeUrl = "https://xsts.auth.xboxlive.com/xsts/authorize";
        const string DeviceAuthenticateUrl = "https://device.auth.xboxlive.com/device/authenticate";
        const string TitleAuthenticateUrl = "https://title.auth.xboxlive.com/title/authenticate";
        const string UserAuthenticateUrl = "https://user.auth.xboxlive.com/user/authenticate";

        public AuthenticateService(XboxLiveConfig config) : base(config, "https://auth.xboxlive.com")
        {
        }

        public Task<XboxLiveAuthenticateResponse<XboxLiveDisplayClaims>> AuthenticateDeviceToken(string accessToken, string deviceVersion, string tokenPrefix = "t=")
        {
            return SignAndRequest<XboxLiveAuthenticateResponse<XboxLiveDisplayClaims>>(DeviceAuthenticateUrl, new XboxLiveAuthenticateRequest
            {
                RelyingParty = "http://auth.xboxlive.com",
                TokenType = "JWT",
                Properties = new Dictionary<string, object>
                {
                    { "AuthMethod", "RPS" },
                    { "ProofKey", Security.ProofKey },
                    { "RpsTicket", tokenPrefix + accessToken },
                    { "SiteName", "user.auth.xboxlive.com" },
                    { "Version", deviceVersion }
                }
            }, "");
        }

        public Task<XboxLiveAuthenticateResponse<XboxLiveDisplayClaims>> AuthenticateUser(string accessToken, string tokenPrefix = "t=")
        {
            return SignAndRequest<XboxLiveAuthenticateResponse<XboxLiveDisplayClaims>>(UserAuthenticateUrl, new XboxLiveAuthenticateRequest
            {
                RelyingParty = "http://auth.xboxlive.com",
                TokenType = "JWT",
                Properties = new Dictionary<string, object>
                {
                    { "AuthMethod", "RPS" },
                    { "SiteName", "user.auth.xboxlive.com" },
                    { "RpsTicket", tokenPrefix + accessToken }
                }
            }, "");
        }

        public Task<XboxLiveAuthenticateResponse<XboxLiveDisplayClaims>> AuthenticateTitle(string accessToken, string deviceToken, string tokenPrefix = "t=")
        {
            return SignAndRequest<XboxLiveAuthenticateResponse<XboxLiveDisplayClaims>>(TitleAuthenticateUrl, new XboxLiveAuthenticateRequest
            {
                RelyingParty = "http://auth.xboxlive.com",
                TokenType = "JWT",
                Properties = new Dictionary<string, object>
                {
                    { "AuthMethod", "RPS" },
                    { "SiteName", "user.auth.xboxlive.com" },
                    { "DeviceToken", deviceToken },
                    { "RpsTicket", tokenPrefix + accessToken }
                }
            }, "");
        }

        public Task<XboxLiveAuthenticateResponse<XboxLiveDisplayClaims>> AuthorizeXsts(string userToken, string deviceToken = null, string titleToken = null)
        {
            Dictionary<string, object> authProperties = new Dictionary<string, object>()
            {
                    { "UserTokens", new string[] { userToken } },
                    { "SandboxId", "RETAIL" }
            };

            if (deviceToken != null) authProperties.Add("DeviceToken", deviceToken);
            if (titleToken != null) authProperties.Add("TitleToken", titleToken);

            return SignAndRequest<XboxLiveAuthenticateResponse<XboxLiveDisplayClaims>>(AuthorizeUrl, new XboxLiveAuthenticateRequest
            {
                RelyingParty = "http://xboxlive.com",
                TokenType = "JWT",
                Properties = authProperties
            }, "");
        }

        private string NextUUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
