using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin.Models
{
	public class BearerTokenClaims: Serializable
	{
		[JsonProperty("iss")]
		public string Issuer { get; set; }

		[JsonProperty("sub")]
		public string Subject { get; set; }

		[JsonProperty("aud")]
		public string Audience { get; set; }

		[JsonProperty("exp")]
		public string ExpirationTime { get; set; }

		[JsonProperty("scp")]
		public string[] Scope { get; set; }

		public static BearerTokenClaims FromBearerToken(BearerToken bearerToken)
		{
			if(bearerToken?.Payload == null)
			{
				return new BearerTokenClaims();
			}
			return FromPayload(bearerToken.Payload);
		}

		public static BearerTokenClaims FromPayload(string payload)
		{
			if (string.IsNullOrEmpty(payload))
			{
				return new BearerTokenClaims();
			}
			return JsonConvert.DeserializeObject<BearerTokenClaims>(payload);
		}
	}
}
