// <copyright file="OktaXamarinUserAgent.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Reflection;

namespace Okta.Xamarin
{
	public class OktaXamarinUserAgent
    {
        private static Lazy<string> userAgent = new Lazy<string>(() => $"Okta-Xamarin-Sdk/{Assembly.GetExecutingAssembly().GetName().Version}");
        public static string Value => userAgent.Value;
    }
}
