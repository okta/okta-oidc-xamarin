// <copyright file="IOktaConfig.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;

namespace Okta.Xamarin
{
	/// <summary>
	/// An interface defining the cross-platform surface area of the OktaConfig
	/// </summary>
	public interface IOktaConfig
	{
		/// <summary>
		/// The client ID of your Okta Application.  Required.
		/// </summary>
		string ClientId { get; set; }

		/// <summary>
		/// This identifies the URL where the authorization code will be obtained from. If not specified, will default to  "{OktaDomain}/oauth2/{AuthorizationServerId}/v1/authorize".  Optional.
		/// </summary>
		string AuthorizeUri { get; set; }

		/// <summary>
		/// The location Okta should redirect to process a login. This is typically something like "{yourAppScheme}:/callback".  Required.
		/// </summary>
		string RedirectUri { get; set; }

		/// <summary>
		/// The location Okta should redirect to process a logout. This is typically something like "{yourAppScheme}:/logout".  Required.
		/// </summary>
		string PostLogoutRedirectUri { get; set; }

		/// <summary>
		/// The OAuth 2.0/OpenID Connect scopes to request when logging in, separated by spaces.  Optional, the default value is "openid profile".
		/// </summary>
		string Scope { get; set; }

		/// <summary>
		/// A readonly list of OAuth 2.0/OpenID Connect scopes to request when logging in, as specified by <see cref="Scope" />.
		/// </summary>
		IReadOnlyList<string> Scopes { get; }

		/// <summary>
		/// Your Okta domain, i.e. https://dev-123456.oktapreview.com.  Do not include the "-admin" part of the domain.  Required.
		/// </summary>
		string OktaDomain { get; set; }

		/// <summary>
		/// The Okta Authorization Server to use.  Optional, the default value is "default".
		/// </summary>
		string AuthorizationServerId { get; set; }

		/// <summary>
		/// The clock skew allowed when validating tokens.  Optional, the default value is 2 minutes.  When parsed from a config file, an integer is interpreted as a number of seconds.
		/// </summary>
		TimeSpan ClockSkew { get; set; }

		/// <summary>
		/// Gets the Authorize Url used for logging in, which is either the <see cref="AuthorizeUri"/> if specified, or constructed from the <see cref="OktaDomain"/> and <see cref="AuthorizationServerId"/>.
		/// </summary>
		/// <returns>The computed Authorize Url used for logging in</returns>
		string GetAuthorizeUrl();

		/// <summary>
		/// Gets the Access Token Url used for retrieving a token, which is constructed from the <see cref="OktaDomain"/> and <see cref="AuthorizationServerId"/>.
		/// </summary>
		/// <returns>The computed Access Token Url used for retrieving a token</returns>
		string GetAccessTokenUrl();
	}
}