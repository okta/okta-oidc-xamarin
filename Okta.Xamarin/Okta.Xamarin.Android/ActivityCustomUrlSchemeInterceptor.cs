using Android.App;
using Android.Content;
using Android.OS;
using System;

namespace Okta.Xamarin.Android
{
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