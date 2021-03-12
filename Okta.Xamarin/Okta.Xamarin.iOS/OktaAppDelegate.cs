

using Foundation;
using UIKit;

namespace Okta.Xamarin.iOS
{
	/// <summary>
	/// Okta specific app delegate that loads an instance of the specified generic application type T and initializes OktaContext when finished launching.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class OktaAppDelegate<T> : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate where T: global::Xamarin.Forms.Application, new()
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
			global::Xamarin.Forms.Forms.Init();
			LoadApplication(new T());
			bool result = base.FinishedLaunching(app, options);

			OktaContext.Init(new iOsOidcClient(Window.RootViewController, iOsOktaConfig.LoadFromPList("OktaConfig.plist")));

			return result;
		}

		public override bool OpenUrl
		(
			UIApplication application,
			NSUrl url,
			string sourceApplication,
			NSObject annotation
		)
		{
			return iOsOidcClient.IsOktaCallback(application, url, sourceApplication, annotation);
		}
	}

	/// <summary>
	/// Okta specific app delegate that initializes OktaContext when finished launching.
	/// </summary>
	public class OktaAppDelegate: global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{        
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
			global::Xamarin.Forms.Forms.Init();			

			bool result = base.FinishedLaunching(app, options);

			OktaContext.Init(new iOsOidcClient(Window.RootViewController, iOsOktaConfig.LoadFromPList("OktaConfig.plist")));

			return result;
		}

		public override bool OpenUrl
		(
			UIApplication application,
			NSUrl url,
			string sourceApplication,
			NSObject annotation
		)
		{
			return iOsOidcClient.IsOktaCallback(application, url, sourceApplication, annotation);
		}
	}
}