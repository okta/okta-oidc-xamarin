using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System;

namespace Okta.Xamarin.Android
{
	[Activity(Label = "OktaCallbackInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleInstance)]
	public class OktaLoginCallbackInterceptorActivity<TMain> : Activity
	{
		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			global::Android.Net.Uri uri_android = Intent.Data;

			if (OidcClient.InterceptLoginCallback(new Uri(uri_android.ToString())))
			{
				var intent = new Intent(this, typeof(TMain));
				intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
				StartActivity(intent);
				this.Finish();
			}

			return;
		}
	}
}