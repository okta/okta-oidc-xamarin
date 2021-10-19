using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Identity
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
