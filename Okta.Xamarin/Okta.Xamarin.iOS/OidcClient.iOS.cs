using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
	public partial class OidcClient
	{
		//public Context AndroidContext { get; set; }

		public void LaunchBrowser(string url)
		{
			//CustomTabsIntent.Builder builder = new CustomTabsIntent.Builder();
			//CustomTabsIntent customTabsIntent = builder.Build();
			//customTabsIntent.LaunchUrl(AndroidContext, global::Android.Net.Uri.Parse(url));
		}

		public OidcClient(object context, IOktaConfig config = null)
		{
			//this.AndroidContext = context;
			this.Config = config;
		}
	}
}
