namespace XboxCsMgr.XboxLive.Model.Authentication
{
    // https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-auth-code-flow
    public class MicrosoftOAuthCode
    {
        public string Code { get; set; }
        public string IdToken { get; set; }
        public string State { get; set; }
        public string Error { get; set; }
        public string ErrorDescription { get; set; }

        public bool IsSuccess 
           => !string.IsNullOrEmpty(Code)
            && string.IsNullOrEmpty(Error);

        public bool IsEmpty 
            => string.IsNullOrEmpty(Code) 
            && string.IsNullOrEmpty(Error)
            && string.IsNullOrEmpty(ErrorDescription);
    }
}
