using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Okta.Xamarin.Android
{
	[Activity(Label = "ActivityCustomUrlSchemeInterceptor", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
	[IntentFilter(
			actions: new[] { Intent.ActionView },
			Categories = new[]
					{
						Intent.CategoryDefault,
						Intent.CategoryBrowsable
					},
			DataSchemes = new string[0],
			DataPath = "/redirect"
		)]
	public class ActivityCustomUrlSchemeInterceptor : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			global::Android.Net.Uri uri_android = Intent.Data;

			// Convert Android.Net.Url to C#/netxf/BCL System.Uri - common API
			Uri uri_netfx = new Uri(uri_android.ToString());

			// load redirect_url Page for parsing
			OidcClient.currentAuthenticator.ParseRedirectedUrl(uri_netfx);

			this.Finish();

			return;
		}
	}
}