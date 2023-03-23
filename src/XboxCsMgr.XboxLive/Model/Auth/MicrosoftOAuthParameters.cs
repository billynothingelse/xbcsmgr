namespace XboxCsMgr.XboxLive.Model.Auth
{
    public class MicrosoftOAuthParameters
    {
        /// <summary>
        /// redirect_uri
        /// </summary>
        public string? RedirectUri { get; set; }

        /// <summary>
        /// response_mode: query, fragment, form_post
        /// </summary>
        public string? ResponseMode { get; set; }

        /// <summary>
        /// response_type: id_token, token, code
        /// </summary>
        public string? ResponseType { get; set; }
        
        /// <summary>
        /// state
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// prompt: login, none, consent, select_account
        /// </summary>
        public string? Prompt { get; set; }

        /// <summary>
        /// login_hint
        /// </summary>
        public string? LoginHint { get; set; }

        /// <summary>
        /// domain_hint
        /// </summary>
        public string? DomainHint { get; set; }

        /// <summary>
        /// code_challenge
        /// </summary>
        public string? CodeChallenge { get; set; }

        /// <summary>
        /// code_challenge_method
        /// </summary>
        public string? CodeChallengeMethod { get; set; }

        public string? Scope { get; set; }
        public string? Nonce { get; set; }
    }
}
