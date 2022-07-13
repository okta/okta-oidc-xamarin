// <copyright file="OidcClient.iOS.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Foundation;
using System.Threading.Tasks;
using UIKit;

namespace Okta.Xamarin.iOS
{
	public class OktaPlatform : OktaPlatformBase
	{
		public static bool IsOktaCallback(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation
		)
		{
			return iOsOidcClient.IsOktaCallback(application, url, sourceApplication, annotation);
		}

		public static async Task<OktaContext> InitAsync(UIViewController iOSViewController)
		{
			return await InitAsync(iOSViewController, iOsOktaConfig.LoadFromPList("OktaConfig.plist"));
		}

		public static async Task<OktaContext> InitAsync(UIViewController iOSViewController, IOktaConfig config)
		{
			return await InitAsync(new iOsOidcClient(iOSViewController, config));
		}
	}
}
