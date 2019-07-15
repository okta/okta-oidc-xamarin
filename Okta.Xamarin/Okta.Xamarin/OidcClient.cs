using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace Okta.Xamarin
{
	public partial class OidcClient
	{
		public OktaConfig Config { get; private set; }

		public OidcClient(OktaConfig config)
		{
			this.Config = config;
		}



		private static string CreateIssuerUrl(string oktaDomain, string authorizationServerId)
		{
			if (string.IsNullOrEmpty(oktaDomain))
			{
				throw new ArgumentNullException(nameof(oktaDomain));
			}

			if (string.IsNullOrEmpty(authorizationServerId))
			{
				return oktaDomain;
			}

			return oktaDomain.EndsWith("/") ? oktaDomain : $"{oktaDomain}/"
				+ "oauth2/" + authorizationServerId;
		}


	}
}
