// <copyright file="OktaConfig.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
	/// <summary>
	/// Stores configuration for the Okta OIDC client
	/// </summary>
	public partial class OktaConfig : IOktaConfig
	{

		/// <summary>
		/// The client ID of your Okta Application.  Required.
		/// </summary>
		[JsonProperty("ClientId", Required = Required.Always)]
		public string ClientId { get; set; }

		/// <summary>
		/// This identifies the URL where the authorization code will be obtained from. If not specified, will default to  "{OktaDomain}/oauth2/{AuthorizationServerId}/v1/authorize".  Optional.
		/// </summary>
		[JsonProperty("AuthorizeUri", Required = Required.Always)]
		public string AuthorizeUri { get; set; }

		/// <summary>
		/// The location Okta should redirect to process a login. This is typically something like "{yourAppScheme}:/callback".  Required.
		/// </summary>
		[JsonProperty("RedirectUri", Required = Required.Always)]
		public string RedirectUri { get; set; }

		/// <summary>
		/// The location Okta should redirect to process a logout. This is typically something like "{yourAppScheme}:/logout".  Required.
		/// </summary>
		[JsonProperty("PostLogoutRedirectUri", Required = Required.Always)]
		public string PostLogoutRedirectUri { get; set; }

		/// <summary>
		/// The OAuth 2.0/OpenID Connect scopes to request when logging in, separated by spaces.  Optional, the default value is "openid profile".
		/// </summary>
		[JsonProperty("Scope", DefaultValueHandling = DefaultValueHandling.Populate)]
		public string Scope { get; set; } = "openid profile";

		/// <summary>
		/// A readonly list of OAuth 2.0/OpenID Connect scopes to request when logging in, as specified by <see cref="Scope" />.
		/// </summary>
		public IReadOnlyList<string> Scopes => Scope.Split(' ');

		/// <summary>
		/// Your Okta domain, i.e. https://dev-123456.oktapreview.com.  Do not include the "-admin" part of the domain.  Required.
		/// </summary>
		[JsonProperty("OktaDomain", Required = Required.Always)]
		public string OktaDomain { get; set; }

		/// <summary>
		/// The Okta Authorization Server to use.  Optional, the default value is "default".
		/// </summary>
		[JsonProperty("AuthorizationServerId", DefaultValueHandling = DefaultValueHandling.Populate)]
		public string AuthorizationServerId { get; set; } = "default";

		/// <summary>
		/// The clock skew allowed when validating tokens.  Optional, the default value is 2 minutes.  When parsed from a config file, an integer is interpreted as a number of seconds.
		/// </summary>
		[JsonProperty("ClockSkew", DefaultValueHandling = DefaultValueHandling.Populate)]
		public TimeSpan ClockSkew { get; set; } = TimeSpan.FromMinutes(2);



		/// <summary>
		/// Default constructor for an OktaConfig object, with optional properties set to defaults.  This config will not be valid until the required properties are set.
		/// </summary>
		public OktaConfig()
		{
		}

		/// <summary>
		/// Constructor for an OktaConfig object including all required properties, with optional properties set to defaults.
		/// </summary>
		/// <param name="clientId"><seealso cref="ClientId"/></param>
		/// <param name="oktaDomain"><seealso cref="OktaDomain"/></param>
		/// <param name="redirectUri"><seealso cref="RedirectUri"/></param>
		/// <param name="postLogoutRedirectUri"><seealso cref="PostLogoutRedirectUri"/></param>
		public OktaConfig(string clientId, string oktaDomain, string redirectUri, string postLogoutRedirectUri)
		{
			this.ClientId = clientId;
			this.OktaDomain = oktaDomain;
			this.RedirectUri = redirectUri;
			this.PostLogoutRedirectUri = postLogoutRedirectUri;
		}


		/// <summary>
		/// Instantiates a <see cref="OktaConfig"/> from a json string and validates it.  Throws an exception if required fields are missing or invalid.
		/// </summary>
		/// <param name="json">The json string to parse.  This should be an object with the various config entries as keys/values.  Please refer to the documentation or samples for examples.</param>
		/// <returns>Returns the <see cref="OktaConfig"/> with fields filled from <paramref name="json"/>.</returns>
		private static OktaConfig ParseJson(string json)
		{
			JObject root = JObject.Parse(json);
			if (root.ContainsKey("Okta"))
				root = (JObject)root["Okta"];

			OktaConfig config = new OktaConfig(root.Value<string>("ClientId"),
											   root.Value<string>("OktaDomain"),
											   root.Value<string>("RedirectUri"),
											   root.Value<string>("PostLogoutRedirectUri"));


			if (root.ContainsKey("Scope"))
			{
				config.Scope = root.Value<string>("Scope");
			}

			if (root.ContainsKey("AuthorizationServerId"))
			{
				config.AuthorizationServerId = root.Value<string>("AuthorizationServerId");
			}

			if (root.ContainsKey("ClockSkew"))
			{
				config.ClockSkew = TimeSpan.FromSeconds(root.Value<int>("ClockSkew"));
			}

			OktaConfigValidator<OktaConfig> validator = new OktaConfigValidator<OktaConfig>();
			validator.Validate(config);

			return config;
		}

		/// <summary>
		/// Instantiates a <see cref="OktaConfig"/> from a json file asynchronously and validates it.  Throws an exception if required fields are missing or invalid.
		/// </summary>
		/// <param name="filename">The file containing json to parse.  This is treated as a simple file path.  If you are attempting to load an embedded resource you need to implement that yourself and call <see cref="ParseJson(string)"/> instead.</param>
		/// <returns>Returns a Task which returns the <see cref="OktaConfig"/> with fields filled from <paramref name="filename"/>.</returns>
		public static async Task<OktaConfig> LoadFromJsonFileAsync(string filename)
		{
			using (StreamReader reader = File.OpenText(filename))
			{
				return ParseJson(await reader.ReadToEndAsync());
			}
		}


		/// <summary>
		/// Gets the Authorize Url used for logging in, which is either the <see cref="AuthorizeUri"/> if specified, or constructed from the <see cref="OktaDomain"/> and <see cref="AuthorizationServerId"/>.
		/// </summary>
		/// <returns>The computed Authorize Url used for logging in</returns>
		public string GetAuthorizeUrl()
		{
			if (string.IsNullOrEmpty(AuthorizeUri))
			{
				return $"{OktaDomain}/oauth2/{AuthorizationServerId}/v1/authorize";
			}
			else
			{
				return AuthorizeUri;
			}
		}

		/// <summary>
		/// Gets the Access Token Url used for retrieving a token, which is constructed from the <see cref="OktaDomain"/> and <see cref="AuthorizationServerId"/>.
		/// </summary>
		/// <returns>The computed Access Token Url used for retrieving a token</returns>
		public string GetAccessTokenUrl()
		{
			return $"{OktaDomain}/oauth2/{AuthorizationServerId}/v1/token";
		}

	}
}
