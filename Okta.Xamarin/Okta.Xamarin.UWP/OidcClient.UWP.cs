using System;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace Okta.Xamarin
{
	public partial class OidcClient : IOidcClient
    {
		/// <summary>
		/// Creates a new UWO Okta OidcClient based on the specified <see cref="OktaConfig"/>
		/// </summary>
		/// <param name="config">The <see cref="OktaConfig"/> to use for this client.  The config must be valid at the time this is called.</param>
		public OidcClient(IOktaConfig config)
		{
			this.Config = config;
			validator.Validate(Config);
		}

		private async Task LaunchBrowserAsync(string url)
		{
			var appUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().AbsoluteUri;

			var result = await WebAuthenticationBroker.AuthenticateAsync(
										WebAuthenticationOptions.None,
										new Uri(url), 
										new Uri(appUri));
			if (result.ResponseStatus == WebAuthenticationStatus.Success)
				OidcClient.CaptureRedirectUrl(new Uri(result.ResponseData));
			else if (result.ResponseStatus == WebAuthenticationStatus.UserCancel)
				throw new TaskCanceledException(result.ResponseData);
			else
				throw new Exception(result.ResponseData);
		}

		/// <summary>
		/// Called by the cross-platform code to close the browser used for login after the redirect.  On UWP this is handled automatically, so an implementation is not needed.
		/// </summary>
		private void CloseBrowser()
		{
			// not needed on UWP
		}
	}
}
