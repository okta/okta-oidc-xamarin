using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Okta.Xamarin.Android;
using Okta.Xamarin.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Okta.Xamarin.Demo.Droid
{
	[Activity(Label = "LogincCallbackInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleInstance)]
	[
		IntentFilter
		(
			actions: new[] { Intent.ActionView },
			Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
			DataSchemes = new[] { "com.okta.xamarin.android.login" },
			DataPath = "/callback"
		)
	]
	public class LoginCallbackInterceptorActivity : OktaLoginCallbackInterceptorActivity<MainActivity>
	{
	}
}