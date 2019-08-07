using SafariServices;

namespace Okta.Xamarin
{
	public partial class OidcClient
	{
#pragma warning disable IDE1006 // Naming Styles
		/// <summary>
		/// Stores a reference to the current iOS <see cref="UIKit.UIViewController"/>, for use in launching the browser for login
		/// </summary>
		private UIKit.UIViewController iOSViewController { get; set; }
#pragma warning restore IDE1006 // Naming Styles

		/// <summary>
		/// Stores the current Safari view controller, so that it can be programmatically closed
		/// </summary>
		private SFSafariViewController SafariViewController;

		/// <summary>
		/// Launches a Safari view controller to the specified url
		/// </summary>
		/// <param name="url">The url to launch in a Safari view controller</param>
		private void LaunchBrowser(string url)
		{
			SafariViewController = new SFSafariViewController(Foundation.NSUrl.FromString(url));
			iOSViewController.PresentViewControllerAsync(SafariViewController, true);
		}

		/// <summary>
		/// Creates a new iOS Okta OidcClient, attached to the provided <see cref="UIKit.UIViewController"/> and based on the specified <see cref="OktaConfig"/>
		/// </summary>
		/// <param name="iOSViewController">A reference to the current iOS <see cref="UIKit.UIViewController"/>, for use in launching the browser for login</param>
		/// <param name="config">The <see cref="OktaConfig"/> to use for this client.  The config must be valid at the time this is called.</param>
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

		/// <summary>
		/// Called to close the Safari view controller used for login after the redirect
		/// </summary>
		private void CloseBrowser()
		{
			if (SafariViewController != null)
			{
				SafariViewController.DismissViewControllerAsync(false);
			}

		}
	}
}
