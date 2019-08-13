// <copyright file="OidcClient.Android.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Android.Content;
using Android.Support.CustomTabs;

namespace Okta.Xamarin
{
	public partial class OidcClient
	{
		/// <summary>
		/// Stores a reference to the current Android <see cref="Context"/>, for use in launching the browser for login
		/// </summary>
		private Context AndroidContext { get; set; }

		/// <summary>
		/// Launches a Chrome custom tab to the specified url
		/// </summary>
		/// <param name="url">The url to launch in a Chrome custom tab</param>
		private void LaunchBrowser(string url)
		{
			CustomTabsIntent.Builder builder = new CustomTabsIntent.Builder();
			CustomTabsIntent customTabsIntent = builder.Build();
			customTabsIntent.LaunchUrl(AndroidContext, global::Android.Net.Uri.Parse(url));
		}

		/// <summary>
		/// Creates a new Android Okta OidcClient, attached to the provided <see cref="Context"/> and based on the specified <see cref="OktaConfig"/>
		/// </summary>
		/// <param name="context">A reference to the current Android <see cref="Context"/>, for use in launching the browser for login</param>
		/// <param name="config">The <see cref="OktaConfig"/> to use for this client.  The config must be valid at the time this is called.</param>
		public OidcClient(Context context, IOktaConfig config)
		{
			this.AndroidContext = context;
			this.Config = config;
			validator.Validate(Config);
		}

		/// <summary>
		/// Called by the cross-platform code to close the browser used for login after the redirect.  On android this is handled automatically, so an implementation is not needed.
		/// </summary>
		private void CloseBrowser()
		{
			// not needed on Android
		}
	}
}
