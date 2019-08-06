using Android.Content;
using Android.Support.CustomTabs;

namespace Okta.Xamarin
{
	public partial class OidcClient
	{
		public Context AndroidContext { get; set; }

		public void LaunchBrowser(string url)
		{
			CustomTabsIntent.Builder builder = new CustomTabsIntent.Builder();
			CustomTabsIntent customTabsIntent = builder.Build();
			customTabsIntent.LaunchUrl(AndroidContext, global::Android.Net.Uri.Parse(url));
		}

		public OidcClient(Context context, IOktaConfig config = null)
		{
			this.AndroidContext = context;
			this.Config = config;
		}

		private void CloseBrowser()
		{
			// not needed on Android
		}
	}
}
