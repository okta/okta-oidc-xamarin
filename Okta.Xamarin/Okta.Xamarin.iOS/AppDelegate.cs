using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Okta.Xamarin.Views;
using UIKit;
using Xamarin.Forms;

namespace Okta.Xamarin.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : OktaAppDelegate<OktaApp>
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
			OktaContext.Current.SignInCompleted += (sender, args) => Shell.Current.GoToAsync("//ProfilePage");
			OktaContext.Current.SignOutCompleted += (sender, args) => Shell.Current.GoToAsync("//ProfilePage");

			return result;
        }
	}
}
