using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace Okta.Xamarin.Android
{
	[Activity(Label = "LoginCallbackInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
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

			if (await OidcClient.InterceptLoginCallbackAsync(new Uri(uri_android.ToString())))
			{
				this.Finish();
			}

			return;
		}
	}
}