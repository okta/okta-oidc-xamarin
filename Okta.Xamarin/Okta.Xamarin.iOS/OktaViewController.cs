using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using System.Threading.Tasks;

namespace Okta.Xamarin.iOS
{
	[Register("UniversalView")]
	public class UniversalView : UIView
	{
		public UniversalView()
		{
			Initialize();
		}

		public UniversalView(RectangleF bounds) : base(bounds)
		{
			Initialize();
		}

		void Initialize()
		{
			BackgroundColor = UIColor.Red;
		}
	}

	[Register("OktaViewController")]
	public class OktaViewController : UIViewController
	{
		OidcClient client;
		public OktaViewController()
		{
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}

		public OktaConfig OktaConfig { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			OktaConfig config = iOsOktaConfig.LoadFromPList("OktaConfig.plist");
			// TODO: log telemetry data
			/*  $"Domain: {config.OktaDomain}\n" +
				$"ClientId: {config.ClientId}\n" +
				$"RedirectUri: {config.RedirectUri}\n" +
				$"PostLogoutRedirecturi: {config.PostLogoutRedirectUri}\n" +
				$"Scope: {string.Join(", ", config.Scopes)}\n" +
				$"ClockSkew: {config.ClockSkew.ToString()}\n";*/

			client = new iOsOidcClient(this, config);
		}

		public async Task<IOktaStateManager> SignIn(IOktaConfig oktaConfig = default)
		{
			oktaConfig = oktaConfig ?? iOsOktaConfig.LoadFromPList("OktaConfig.plist");
			OidcClient oidcClient = new iOsOidcClient(this, oktaConfig);
			return await oidcClient.SignInWithBrowserAsync();
		}
	}
}