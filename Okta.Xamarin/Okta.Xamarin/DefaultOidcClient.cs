// <copyright file="DefaultOidcClient.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Reflection;

namespace Okta.Xamarin
{
    public class DefaultOidcClient : OidcClient
    {
		private static Lazy<string> userAgent = new Lazy<string>(() => $"Okta-Xamarin-Sdk/{Assembly.GetExecutingAssembly().GetName().Version}");

		protected override void CloseBrowser()
        {
        }

		protected override string GetUserAgent()
		{
			return userAgent.Value;
		}

		protected override void LaunchBrowser(string url)
        {
        }
    }
}
