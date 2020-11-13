using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Foundation;
using UIKit;

namespace Okta.Xamarin.iOS
{
	public class OktaAppDelegate<T> : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate where T: global::Xamarin.Forms.Application, new()
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
			global::Xamarin.Forms.Forms.Init();
			LoadApplication(new T());
			bool result = base.FinishedLaunching(app, options);

			OktaContext.Init(new OidcClient(Window.RootViewController, OktaConfig.LoadFromPList("OktaConfig.plist")));

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
			return OidcClient.IsOktaCallback(application, url, sourceApplication, annotation);
		}
	}

	public class OktaAppDelegate: global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{        
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
			global::Xamarin.Forms.Forms.Init();			

			bool result = base.FinishedLaunching(app, options);

			OktaContext.Init(new OidcClient(Window.RootViewController, OktaConfig.LoadFromPList("OktaConfig.plist")));

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
			return OidcClient.IsOktaCallback(application, url, sourceApplication, annotation);
		}
	}
}