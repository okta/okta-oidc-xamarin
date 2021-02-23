using Newtonsoft.Json;
using Okta.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin
{
    public class RenewResponse : Serializable
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("id_token")]
        public string IdToken { get; set; }
    }
}
