using System;
using System.Collections.Generic;

namespace Okta.Xamarin
{
	public interface IOktaConfig
	{
		string AuthorizationServerId { get; set; }
		string ClientId { get; set; }
		TimeSpan ClockSkew { get; set; }
		string OktaDomain { get; set; }
		string PostLogoutRedirectUri { get; set; }
		string RedirectUri { get; set; }
		string Scope { get; set; }
		IReadOnlyList<string> Scopes { get; }
	}
}