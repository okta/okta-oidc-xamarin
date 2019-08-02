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

			// Convert Android.Net.Url to C#/netxf/BCL System.Uri - common API
			Uri uri_netfx = new Uri(uri_android.ToString());


			string uriString = uri_netfx.Query.ToString();
			System.Diagnostics.Debug.WriteLine(uriString);

			var parsed = System.Web.HttpUtility.ParseQueryString(uriString);
			System.Diagnostics.Debug.WriteLine(parsed.ToString());

			string state = parsed["state"];

			if (OidcClient.currentAuthenticatorbyState.ContainsKey(state))
			{
				// load redirect_url Page for parsing
				OidcClient.currentAuthenticatorbyState[state].ParseRedirectedUrl(uri_netfx);
			}
			else
			{
				//throw new InvalidOperationException("OAuth callback did not include expected state in url: " + uri_netfx.ToString());
			}
			this.Finish();

			return;
		}
	}
}