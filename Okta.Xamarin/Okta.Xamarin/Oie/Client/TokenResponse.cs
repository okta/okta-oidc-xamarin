// <copyright file="TokenResponse.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Net.Http;
using Newtonsoft.Json;

namespace Okta.Xamarin.Oie.Client
{
    public class TokenResponse : IdentityResponse
    {
        public TokenResponse() { }

        public TokenResponse(HttpResponseMessage httpResponseMessage) : base(httpResponseMessage)
        {
        }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int? ExpiresIn { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("scope")]
        public string Scope{ get; set; }
    }
}
