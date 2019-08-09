// <copyright file="OktaMiddlewareExtensions.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;

namespace Okta.Xamarin
{
	public interface IOktaConfig
	{
		string AuthorizationServerId { get; set; }
		string AuthorizeUri { get; set; }
		string ClientId { get; set; }
		TimeSpan ClockSkew { get; set; }
		string OktaDomain { get; set; }
		string PostLogoutRedirectUri { get; set; }
		string RedirectUri { get; set; }
		string Scope { get; set; }
		IReadOnlyList<string> Scopes { get; }

		string GetAuthorizeUri();
		string GetAccessTokenUrl();
	}
}