using Android.Content;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace Okta.Xamarin
{
	public partial class OidcClient
	{
		public static OidcClient currentAuthenticator;

		public Task<StateManager> SignInWithBrowserAsync(Context context)
		{
			OAuth2Authenticator auth = new OAuth2Authenticator
				(
					clientId: Config.ClientId,
					scope: Config.Scope,
					authorizeUrl: new Uri(CreateIssuerUrl(Config.OktaDomain, Config.AuthorizationServerId)),
					redirectUrl: new Uri(Config.RedirectUri),
					isUsingNativeUI: true
				);

			TaskCompletionSource<StateManager> result = new TaskCompletionSource<StateManager>();

			currentAuthenticator = this;

			var authActivity = auth.GetUI(context);

			auth.Completed += (object sender, AuthenticatorCompletedEventArgs e) =>
			{
				// UI presented, so it's up to us to dimiss it on Android
				// dismiss Activity with WebView or CustomTabs
				//authActivity .Finish();

				if (e.IsAuthenticated)
				{
					result.SetResult(new StateManager(
						e.Account.Properties["access_token"],
						e.Account.Properties["id_token"],
						e.Account.Properties["refresh_token"]));
				}
				else
				{
					result.SetCanceled();
				}
			};


			context.StartActivity(authActivity);


			return result.Task;
		}

	}
}
