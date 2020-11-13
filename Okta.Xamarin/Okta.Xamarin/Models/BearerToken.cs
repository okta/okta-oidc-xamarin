using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin.Models
{
	public class BearerToken
	{
		public BearerToken(string jwtToken)
		{
			if (string.IsNullOrEmpty(jwtToken))
			{
				return;
			}
			string[] segments = jwtToken.Split('.');
			if(segments.Length != 3)
			{
				return;
			}
			Base64UrlEncodedHeader = segments[0];
			Base64UrlEncodedPayload = segments[1];
			Signature = segments[2];
		}

		public string Header 
		{
			get
			{
				return string.IsNullOrEmpty(Base64UrlEncodedHeader) ? "" : Base64UrlEncoder.Decode(Base64UrlEncodedHeader);
			}
		}

		public string Payload 
		{
			get
			{
				return string.IsNullOrEmpty(Base64UrlEncodedPayload) ? "" : Base64UrlEncoder.Decode(Base64UrlEncodedPayload);
			}
		}

		public override string ToString()
		{
			return $"{Base64UrlEncodedHeader}.{Base64UrlEncodedPayload}.{Signature}";
		}

		protected string Base64UrlEncodedHeader { get; set; }
		protected string Base64UrlEncodedPayload { get; set; }
		public string Signature { get; set; }

		public Dictionary<string, object> GetClaims()
		{
			return JsonConvert.DeserializeObject<Dictionary<string, object>>(BearerTokenClaims.FromBearerToken(this).ToJson());
		}
	}
}
