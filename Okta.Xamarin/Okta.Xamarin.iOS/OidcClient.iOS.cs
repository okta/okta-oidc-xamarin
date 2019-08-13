// <copyright file="OidcClient.iOS.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Foundation;
using SafariServices;
using System;
using UIKit;

namespace Okta.Xamarin
{
	public partial class OidcClient
	{
#pragma warning disable IDE1006 // Naming Styles
		/// <summary>
		/// Stores a reference to the current iOS <see cref="UIKit.UIViewController"/>, for use in launching the browser for login
		/// </summary>
		private UIKit.UIViewController iOSViewController { get; set; }
#pragma warning restore IDE1006 // Naming Styles

		/// <summary>
		/// Stores the current Safari view controller, so that it can be programmatically closed
		/// </summary>
		private SFSafariViewController SafariViewController;

		/// <summary>
		/// Launches a Safari view controller to the specified url
		/// </summary>
		/// <param name="url">The url to launch in a Safari view controller</param>
		private void LaunchBrowser(string url)
		{
			SafariViewController = new SFSafariViewController(Foundation.NSUrl.FromString(url));
			iOSViewController.PresentViewControllerAsync(SafariViewController, true);
		}

		/// <summary>
		/// Creates a new iOS Okta OidcClient, attached to the provided <see cref="UIKit.UIViewController"/> and based on the specified <see cref="OktaConfig"/>
		/// </summary>
		/// <param name="iOSViewController">A reference to the current iOS <see cref="UIKit.UIViewController"/>, for use in launching the browser for login</param>
		/// <param name="config">The <see cref="OktaConfig"/> to use for this client.  The config must be valid at the time this is called.</param>
		public OidcClient(UIKit.UIViewController iOSViewController, IOktaConfig config)
		{
			while (iOSViewController.PresentedViewController != null)
			{
				iOSViewController = iOSViewController.PresentedViewController;
			}
			this.iOSViewController = iOSViewController;
			this.Config = config;
			validator.Validate(Config);
		}

		/// <summary>
		/// Called to close the Safari view controller used for login after the redirect
		/// </summary>
		private void CloseBrowser()
		{
			if (SafariViewController != null)
			{
				SafariViewController.DismissViewControllerAsync(false);
			}

		}

		/// <summary>
		/// This is used to handle the callback from the Safari view controller after a user logs in.  You need to forward calls to AppDelegate.OpenUrl to this function.  Please see the documentation for more information.
		/// </summary>
		/// <returns><see langword="true"/> if this url can be handled by an <see cref="OidcClient"/>, or <see langword="false"/> if it is some other url which is not handled by the login flow.</returns>
		public static bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
			if (OidcClient.CaptureRedirectUrl(new Uri(url.AbsoluteString)))
			{
				return true;
			}

			return false;
		}
	}
}
