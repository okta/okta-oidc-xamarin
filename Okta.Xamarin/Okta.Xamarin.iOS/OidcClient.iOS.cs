using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using SafariServices;
using UIKit;

namespace Okta.Xamarin
{
	public partial class OidcClient
	{
		public UIKit.UIViewController iOSViewController { get; set; }
		SFSafariViewController safariViewController;

		public void LaunchBrowser(string url)
		{
			safariViewController = new SFSafariViewController(Foundation.NSUrl.FromString(url));
			iOSViewController.PresentViewControllerAsync(safariViewController, true);
		}

		public OidcClient(UIKit.UIViewController iOSViewController, IOktaConfig config)
		{
			while (iOSViewController.PresentedViewController != null)
			{
				iOSViewController = iOSViewController.PresentedViewController;
			}
			this.iOSViewController = iOSViewController;
			this.Config = config;
			validator.Validate(Config);
		}

		public static bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
			if (OidcClient.CaptureRedirectUrl(new Uri(url.AbsoluteString)))
			{
				return true;
			}

			return false;
		}

		private void CloseBrowser()
		{
			if (safariViewController != null)
			{
				safariViewController.DismissViewControllerAsync(false);
			}

		}
	}
}
