// <copyright file="OidcClient.iOS.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Foundation;
using System;
using System.Threading.Tasks;
using UIKit;

namespace Okta.Xamarin.iOS
{
	public class OktaPlatform : OktaPlatformBase
	{
		/// <summary>
		/// Detrmines if the specified url matches the configured `RedirectUri` or the configured `PostLogoutRedirectUri`.  Designed to be called from AppDelegate.OpenUrl.
		/// </summary>
		/// <param name="application">The main application.</param>
		/// <param name="url">The url.</param>
		/// <param name="sourceApplication">The source application.</param>
		/// <param name="annotation">The annotation</param>
		/// <returns>True if a match is found.</returns>
		public static bool IsOktaCallback(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation
		)
		{
			return iOsOidcClient.IsOktaCallback(application, url, sourceApplication, annotation);
		}

		/// <summary>
		/// Initialize the Okta platform.
		/// </summary>
		/// <param name="iOSWindow">The main application window.</param>
		/// <returns>OktaContext.</returns>
		public static async Task<OktaContext> InitAsync(UIWindow iOSWindow)
		{
			return await InitAsync(iOSWindow, iOsOktaConfig.LoadFromPList("OktaConfig.plist"));
		}

		/// <summary>
		/// Initalize the Okta platform.
		/// </summary>
		/// <param name="iOSWindow">The main application window.</param>
		/// <param name="config">The configuration.</param>
		/// <returns>OktaContext.</returns>
		public static async Task<OktaContext> InitAsync(UIWindow iOSWindow, IOktaConfig config)
		{
			return await InitAsync(new iOsOidcClient(iOSWindow, config));
		}

		[Obsolete("Use InitAsync(UIWindow iOSWindow) instead.")]
		public static async Task<OktaContext> InitAsync(UIViewController iOSViewController)
		{
			return await InitAsync(iOSViewController, iOsOktaConfig.LoadFromPList("OktaConfig.plist"));
		}

		[Obsolete("Use InitAsync(UIWindow iOSWindow, IOktaConfig config) instead.")]
		public static async Task<OktaContext> InitAsync(UIViewController iOSViewController, IOktaConfig config)
		{
			return await InitAsync(new iOsOidcClient(iOSViewController, config));
		}
	}
}
