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

		public void LaunchBrowser(string url)
		{
			var sfViewController = new SFSafariViewController(Foundation.NSUrl.FromString(url));
			iOSViewController.PresentViewControllerAsync(sfViewController, true);
		}

		public OidcClient(UIKit.UIViewController iOSViewController, IOktaConfig config = null)
		{
			this.iOSViewController = iOSViewController;
			this.Config = config;
		}

		public static bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{			if (OidcClient.CaptureRedirectUrl(new Uri(url.AbsoluteString)))
			{
				return true;
			}

			return false;
		}
	}
}
