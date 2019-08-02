using System;
using System.Collections.Generic;

namespace Okta.Xamarin
{
	public interface IOktaConfig
	{
		string AuthorizationServerId { get; set; }
		string AuthorizeUri { get; set; }
		string ClientId { get; set; }
		TimeSpan ClockSkew { get; set; }
		string OktaDomain { get; set; }
		string PostLogoutRedirectUri { get; set; }
		string RedirectUri { get; set; }
		string Scope { get; set; }
		IReadOnlyList<string> Scopes { get; }
	}

	public static class OktaConfigExtensions
	{
		public static string GetAuthorizeUri(this IOktaConfig config)
		{
			if (string.IsNullOrEmpty(config.AuthorizeUri))
			{
				return $"{config.OktaDomain}/oauth2/{config.AuthorizationServerId}/v1/authorize";
			}
			else
			{
				return config.AuthorizeUri;
			}
		}

		public static string GetAccessTokenUrl(this IOktaConfig config)
		{

			return $"{config.OktaDomain}/oauth2/{config.AuthorizationServerId}/v1/token";

		}
	}
}