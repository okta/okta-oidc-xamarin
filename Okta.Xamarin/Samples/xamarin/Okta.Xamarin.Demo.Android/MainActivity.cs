using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Okta.Xamarin.Android;

namespace Okta.Xamarin.Demo.Droid
{
	[Activity(Label = "Okta.Xamarin", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
	public class MainActivity : OktaMainActivity<OktaDemoApp>
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{

			base.OnCreate(savedInstanceState);
		}

		public override void OnSignInCompleted(object sender, SignInEventArgs signInEventArgs)
		{
			// for demo purposes go to the profile page
			Shell.Current.GoToAsync("//ProfilePage", true);
		}

		public override void OnSignOutCompleted(object sender, SignOutEventArgs signOutEventArgs)
		{
			// for demo purposes go to the profile page
			Shell.Current.GoToAsync("//ProfilePage", true);
		}
	}
}
