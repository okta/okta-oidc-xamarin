using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin
{
	static class UrlHelper
	{

		public static string CreateIssuerUrl(string oktaDomain, string authorizationServerId)
		{
			if (string.IsNullOrEmpty(oktaDomain))
			{
				throw new ArgumentNullException(nameof(oktaDomain));
			}

			if (string.IsNullOrEmpty(authorizationServerId))
			{
				return oktaDomain;
			}

			return $"{EnsureTrailingSlash(oktaDomain)}oauth2/{authorizationServerId}";
		}

		/// <summary>
		/// Ensures that this URI ends with a trailing slash <c>/</c>.
		/// </summary>
		/// <param name="uri">The URI string.</param>
		/// <returns>The URI string, appended with <c>/</c> if necessary.</returns>
		public static string EnsureTrailingSlash(string uri)
		{
			if (string.IsNullOrEmpty(uri))
			{
				throw new ArgumentNullException(nameof(uri));
			}

			return uri.EndsWith("/")
				? uri
				: $"{uri}/";
		}


	}
}
