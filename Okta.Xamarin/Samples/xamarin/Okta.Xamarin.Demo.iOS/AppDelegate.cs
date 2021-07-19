using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Okta.Xamarin.iOS;
using UIKit;
using Xamarin.Forms;

namespace Okta.Xamarin.Demo.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : OktaAppDelegate<OktaDemoApp>
	{
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			bool result = base.FinishedLaunching(app, options);

			// Additional logic to execute can go here if necessary

			// for demo purposes go to the profile page after sign in and sign out
			OktaContext.AddSignInCompletedListener((sender, args) => Shell.Current.GoToAsync("//ProfilePage"));
			OktaContext.AddSignOutCompletedListener((sender, args) => Shell.Current.GoToAsync("//ProfilePage"));
			
			// -- /

			return result;
		}
	}
}
