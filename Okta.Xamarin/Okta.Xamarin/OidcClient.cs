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


		/// <summary>
		/// Start the authorization flow.  This is an async method and should be awaited.
		/// </summary>
		/// <returns>In case of successful authorization, this Task will return a valid <see cref="StateManager"/>.  Clients are responsible for further storage and maintenance of the manager.</returns>
		public async Task<StateManager> SignInWithBrowserAsync()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This method will end the user's Okta session in the browser.  This is an async method and should be awaited.
		/// </summary>
		/// <param name="stateManager">The state manager associated with the login that you wish to log out</param>
		/// <returns>Task which tracks the progress of the logout</returns>
		public async Task SignOutOfOktaAsync(StateManager stateManager)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Complete the authorization of a valid session obtained via the <see cref="https://github.com/okta/okta-auth-dotnet">AuthN SDK</see>.
		/// </summary>
		/// <param name="sessionToken">A valid session  token obtained via the <see cref="https://github.com/okta/okta-auth-dotnet">AuthN SDK</see></param>
		/// <returns>In case of successful authorization, this Task will return a valid <see cref="StateManager"/>.  Clients are responsible for further storage and maintenance of the manager.</returns>
		public async Task<StateManager> AuthenticateAsync(string sessionToken)
		{
			throw new NotImplementedException();
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
