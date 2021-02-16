using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace Okta.Xamarin.Android
{
	[Activity(Label = "LoginCallbackInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleInstance)]
	[
		IntentFilter
		(
			actions: new[] { Intent.ActionView }, 
			Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
			DataSchemes = new[] { "com.okta.xamarin.android.login" },
			DataPath = "/callback"
		)
	]
	public class LoginCallbackInterceptorActivity : Activity
	{
		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			global::Android.Net.Uri uri_android = Intent.Data;

			if (OidcClient.InterceptLoginCallback(new Uri(uri_android.ToString())))
			{
				var intent = new Intent(this, typeof(MainActivity));
				intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
				StartActivity(intent);
				this.Finish();
			}

			return;
		}
	}
}