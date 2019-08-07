using Android.App;
using Android.Content;
using Android.OS;
using System;

namespace Okta.Xamarin.Android
{
	/// <summary>
	/// This is used to handle the callback from the Chrome custom tab after a user logs in.  You need to create a new activity that inherits from this class and set the appropriate IntentFilter.  Please see the documentation for more information.
	/// </summary>
	public class ActivityCustomUrlSchemeInterceptor : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			global::Android.Net.Uri uri_android = Intent.Data;

			if (OidcClient.CaptureRedirectUrl(new Uri(uri_android.ToString())))
				this.Finish();

			return;
		}
	}
}