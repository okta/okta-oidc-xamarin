using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Identity
{
	public class IdentityClientConfiguration
	{
		public const string DefaultOktaDomain = "dotnet-idx-sdk.okta.com";
		public const string DefaultClientId = "0oatzfskmLm4faAaQ5d6"; 
		public const string DefaultScopes = "openid profile offline_access";
		public const string DefaultIssuerUri = "https://dotnet-idx-sdk.okta.com/oauth2/default";
		public const string DefaultRedirectUri = "https://localhost:44314/interactioncode/callback";

		public string OktaDomain { get; set; }
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }

		public List<string> Scopes { get; set; } = new List<string> { "openid", "profile", "offline_access" };

		public string IssuerUri { get; set; }

		public string RedirectUri { get; set; }

		public static IdentityClientConfiguration Default => new IdentityClientConfiguration
		{
			OktaDomain = DefaultOktaDomain,
			ClientId = DefaultClientId,
			Scopes = DefaultScopes.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
			IssuerUri = DefaultIssuerUri,
			RedirectUri = DefaultRedirectUri
		};

		public override bool Equals(object obj)
		{
			if(obj == null)
			{
				return false;
			}

			if (obj is IdentityClientConfiguration configuration)
			{
				return AreEqual(OktaDomain, configuration.OktaDomain) &&
					AreEqual(ClientId, configuration.ClientId) &&
					AreEqual(ClientSecret, configuration.ClientSecret) &&
					ScopesAreEqual(configuration) &&
					AreEqual(IssuerUri, configuration.IssuerUri) &&
					AreEqual(RedirectUri, configuration.RedirectUri);
			}

			return base.Equals(obj);
		}

		public override string ToString()
		{
			return ToJson();
		}

		public string ToJson(bool indent = false)
		{
			return JsonConvert.SerializeObject(this, indent ? Formatting.Indented: Formatting.None);
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		private bool ScopesAreEqual(IdentityClientConfiguration configuration)
		{
			HashSet<string> currentScopes = new HashSet<string>(Scopes);
			HashSet<string> compareToScopes = new HashSet<string>(configuration.Scopes);
			foreach (string scope in currentScopes)
			{
				if (!compareToScopes.Contains(scope))
				{
					return false;
				}
			}

			foreach (string scope in compareToScopes)
			{
				if (!currentScopes.Contains(scope))
				{
					return false;
				}
			}
			return true;
		}

		private bool AreEqual(string one, string two)
		{
			if (one == null && two != null)
			{
				return false;
			}

			if (two == null && one != null)
			{
				return false;
			}

			if (one == null && two == null)
			{
				return true;
			}

			return one.Equals(two);
		}
	}
}
