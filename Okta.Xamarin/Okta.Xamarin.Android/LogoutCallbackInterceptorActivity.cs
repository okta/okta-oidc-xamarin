using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace Okta.Xamarin.Android
{
	[Activity(Label = "LogoutCallbackInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
	[
		IntentFilter
		(
			actions: new[] { Intent.ActionView }, 
			Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
			DataSchemes = new[] { "com.okta.xamarin.android.logout" },
			DataPath = "/callback"
		)
	]
	public class LogoutCallbackInterceptorActivity : Activity
	{
		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			global::Android.Net.Uri uri_android = Intent.Data;

			if (OidcClient.InterceptLogoutCallback(new Uri(uri_android.ToString())))
			{
				this.Finish();
			}

			return;
		}
	}
}