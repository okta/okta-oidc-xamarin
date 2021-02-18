using System.Net.Http;
using Newtonsoft.Json;
using Okta.Xamarin.Models;

namespace Okta.Xamarin
{
    public class LogoutOptions : Serializable
    {
        public LogoutOptions()
        {
        }

        public LogoutOptions(OktaStateManager stateManager, IOktaConfig oktaConfig, string state)
        {
            this.IdTokenHint = stateManager.IdToken;
            this.PostLogoutRedirectUri = oktaConfig.PostLogoutRedirectUri;
            this.State = state;
        }

        [JsonProperty("id_token_hint")]
        public string IdTokenHint { get; set; }

        [JsonProperty("post_logout_redirect_uri")]
        public string PostLogoutRedirectUri { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }
}