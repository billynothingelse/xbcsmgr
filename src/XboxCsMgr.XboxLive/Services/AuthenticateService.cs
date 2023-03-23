using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using XboxCsMgr.Helpers.Http;
using XboxCsMgr.Helpers.Serialization;
using XboxCsMgr.XboxLive.Model.Auth;

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

        public Task<XboxLiveAuthenticateResponse<XboxLiveDisplayClaims>> AuthenticateDeviceToken(string id, string deviceType, string deviceVersion)
        {
            return SignAndRequest<XboxLiveAuthenticateResponse<XboxLiveDisplayClaims>>(DeviceAuthenticateUrl, new XboxLiveAuthenticateRequest
            {
                RelyingParty = "http://auth.xboxlive.com",
                TokenType = "JWT",
                Properties = new Dictionary<string, object>
                {
                    { "AuthMethod", "ProofOfPossession" },
                    { "Id", "{" + id + "}" },
                    { "SerialNumber", "{" + NextUUID() + "}"  },
                    { "DeviceType", deviceType },
                    { "Version", deviceVersion },
                    { "ProofKey", Security.ProofKey }
                }
            }, "");
        }

        public Task<XboxLiveAuthenticateResponse<XboxLiveDisplayClaims>> AuthenticateUser(string accessToken, string? tokenPrefix = "t=")
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

        public Task<XboxLiveAuthenticateResponse<XboxLiveDisplayClaims>> AuthenticateTitle(string accessToken, string deviceToken, string? tokenPrefix = "t=")
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
                    { "RpsTicket", tokenPrefix + accessToken },
                    { "ProofKey",  Security.ProofKey }
                }
            }, "");
        }

        public Task<XboxLiveAuthenticateResponse<XboxLiveDisplayClaims>> AuthorizeXsts(string userToken, string deviceToken = null, string titleToken = null)
        {
            return SignAndRequest<XboxLiveAuthenticateResponse<XboxLiveDisplayClaims>>(AuthorizeUrl, new XboxLiveAuthenticateRequest
            {
                RelyingParty = "http://xboxlive.com",
                TokenType = "JWT",
                Properties = new Dictionary<string, object>
                {
                    { "UserTokens", new string[] { userToken } },
                    { "SandboxId", "RETAIL" }
                }
            }, "");
        }

        private string NextUUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
