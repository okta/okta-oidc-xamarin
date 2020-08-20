using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Xamarin.UITest
{
	// TODO: move OktaConfig to Okta.Xamarin.Shared to eliminate the need for this class
	public class TestOktaConfig : IOktaConfig
	{
		public string ClientId { get; set; }
		public string AuthorizeUri { get; set; }
		public string RedirectUri { get; set; }
		public string PostLogoutRedirectUri { get; set; }
		public string Scope { get; set; }

		public IReadOnlyList<string> Scopes
		{
			get; set;
		}

		public string OktaDomain { get; set; }
		public string AuthorizationServerId { get; set; }
		public TimeSpan ClockSkew { get; set; }

		// TODO: refactor IOktaConfig and related to consolidate implementation
		/// <summary>
		/// Gets the Authorize Url used for logging in, which is either the <see cref="AuthorizeUri"/> if specified, or constructed from the <see cref="OktaDomain"/> and <see cref="AuthorizationServerId"/>.
		/// </summary>
		/// <returns>The computed Authorize Url used for logging in</returns>
		public string GetAuthorizeUrl()
		{
			if (string.IsNullOrEmpty(AuthorizeUri))
			{
				return $"{OktaDomain}/oauth2/{AuthorizationServerId}/v1/authorize";
			}
			else
			{
				return AuthorizeUri;
			}
		}

		/// <summary>
		/// Gets the Access Token Url used for retrieving a token, which is constructed from the <see cref="OktaDomain"/> and <see cref="AuthorizationServerId"/>.
		/// </summary>
		/// <returns>The computed Access Token Url used for retrieving a token</returns>
		public string GetAccessTokenUrl()
		{
			return $"{OktaDomain}/oauth2/{AuthorizationServerId}/v1/token";
		}
	}
}
