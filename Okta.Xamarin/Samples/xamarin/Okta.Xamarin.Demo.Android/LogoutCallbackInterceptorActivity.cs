using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Okta.Xamarin.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Okta.Xamarin.Demo.Droid
{
	[Activity(Label = "LogoutCallbackInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleInstance)]
	[
		IntentFilter
		(
			actions: new[] { Intent.ActionView },
			Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
			DataSchemes = new[] { "com.okta.xamarin.android.logout" },
			DataPath = "/callback"
		)
	]
	public class LogoutCallbackInterceptorActivity : OktaLogoutCallbackInterceptorActivity<MainActivity>
	{
	}
}
