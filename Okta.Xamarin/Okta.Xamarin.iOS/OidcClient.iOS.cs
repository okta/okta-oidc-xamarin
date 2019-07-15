using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace Okta.Xamarin
{
	public partial class OidcClient
	{
		private void Auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
		{
			// UI presented, so it's up to us to dimiss it on iOS
			// dismiss ViewController with UIWebView or SFSafariViewController
			this.DismissViewController(true, null);

			if (eventArgs.IsAuthenticated)
			{
				// Use eventArgs.Account to do wonderful things
			}
			else
			{
				// The user cancelled
			}
		}
	}
}
