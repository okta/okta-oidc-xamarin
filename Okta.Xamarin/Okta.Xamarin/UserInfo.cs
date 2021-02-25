// <copyright file="UserInfo.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using System.Security.Claims;
using Newtonsoft.Json;
using Okta.Xamarin.Models;

namespace Okta.Xamarin
{
    public class UserInfo : Serializable
    {
        [JsonProperty("sub")]
        public string Sub { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("preferred_username")]
        public string PreferredUserName { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("zoneinfo")]
        public string ZoneInfo { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt{ get; set; }

        public List<Claim> ToClaims()
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Name, GivenName, ClaimValueTypes.String),
                new Claim(ClaimTypes.Surname, FamilyName, ClaimValueTypes.String),
                new Claim("zoneinfo", ZoneInfo),
                new Claim("preferred_username", PreferredUserName, ClaimValueTypes.String),
                new Claim("given_name", GivenName, ClaimValueTypes.String),
                new Claim("family_name", FamilyName, ClaimValueTypes.String),
                new Claim("updated_at", UpdatedAt, ClaimValueTypes.String),
                new Claim(ClaimTypes.Sid, Sub, ClaimValueTypes.String),
            };
        }
    }
}
