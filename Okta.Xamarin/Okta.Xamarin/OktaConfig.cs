using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
	public class OktaConfig
	{

		/// <summary>
		/// The client ID of your Okta Application.  Required.
		/// </summary>
		[JsonProperty("ClientId")]
		public string ClientId { get; set; }

		/// <summary>
		/// OIDC callback path for the code flow.  Optional, not recommended to override.
		/// </summary>
		[JsonProperty("CallbackPath", DefaultValueHandling = DefaultValueHandling.Populate, NullValueHandling = NullValueHandling.Ignore)]
		public string CallbackPath { get; set; } = Defaults.CallbackPath;

		/// <summary>
		/// The location Okta should redirect to process a login. This is typically something like "{yourOktaScheme}:/callback".  Required.
		/// </summary>
		[JsonProperty("RedirectUri", DefaultValueHandling = DefaultValueHandling.Populate, NullValueHandling = NullValueHandling.Ignore)]
		public string RedirectUri { get; set; } = Defaults.RedirectUri;

		/// <summary>
		/// The location Okta should redirect to process a login. This is typically something like "{yourOktaScheme}:/logout".  Required.
		/// </summary>
		[JsonProperty("PostLogoutRedirectUri", DefaultValueHandling = DefaultValueHandling.Populate, NullValueHandling = NullValueHandling.Ignore)]
		public string PostLogoutRedirectUri { get; set; } = Defaults.PostLogoutRedirectUri;

		/// <summary>
		/// The OAuth 2.0/OpenID Connect scopes to request when logging in, seperated by spaces.  Optional, the default value is "openid profile".
		/// </summary>
		[JsonProperty("Scope", DefaultValueHandling = DefaultValueHandling.Populate, NullValueHandling = NullValueHandling.Ignore)]
		public string Scope { get; set; } = Defaults.Scope;
		/// <summary>
		/// A readlony list of OAuth 2.0/OpenID Connect scopes to request when logging in, as specified by <see cref="Scope" />.
		/// </summary>
		public IReadOnlyList<string> Scopes => Scope.Split(' ');

		/// <summary>
		/// Whether to retrieve additional claims from the UserInfo endpoint after login (not usually necessary).  Optional, the default value is <see cref="false"/>.
		/// </summary>
		[JsonProperty("GetClaimsFromUserInfoEndpoint", DefaultValueHandling = DefaultValueHandling.Populate, NullValueHandling = NullValueHandling.Ignore)]
		public bool GetClaimsFromUserInfoEndpoint { get; set; } = false;

		/// <summary>
		/// Your Okta domain, i.e. https://dev-123456.oktapreview.com.  Do not include the "-admin" part of the domain.  Required.
		/// </summary>
		[JsonProperty("OktaDomain")]
		public string OktaDomain { get; set; }

		/// <summary>
		/// The Okta Authorization Server to use.  optional, the default value is "default".
		/// </summary>
		[JsonProperty("AuthorizationServerId", DefaultValueHandling = DefaultValueHandling.Populate, NullValueHandling = NullValueHandling.Ignore)]
		public string AuthorizationServerId { get; set; } = Defaults.AuthorizationServerId;

		/// <summary>
		/// The clock skew allowed when validating tokens.  Optional, the default value is 2 minutes.  When parsed from a config file, an integer is interpreted as a number of seconds.
		/// </summary>
		[JsonProperty("ClockSkew", DefaultValueHandling = DefaultValueHandling.Populate, NullValueHandling = NullValueHandling.Ignore)]
		public TimeSpan ClockSkew { get; set; } = Defaults.ClockSkew;




		/// <summary>
		/// Instantiates a <see cref="OktaConfig"/> from a json string and validates it.  Throws an exception if required fields are missing or invalid.
		/// </summary>
		/// <param name="json">The json string to parse.  This should be an object with the various confige entries as keys/values.  Please refer to the documentation or samples for examples.</param>
		/// <returns>Returns the <see cref="OktaConfig"/> with fields filled from <paramref name="json"/>.</returns>
		private static OktaConfig ParseJson(string json)
		{
			OktaConfig config = JsonConvert.DeserializeObject<OktaConfig>(json);

			OktaConfigValidator<OktaConfig> validator = new OktaConfigValidator<OktaConfig>();
			validator.Validate(config);

			return config;
		}

		/// <summary>
		/// Instantiates a <see cref="OktaConfig"/> from a json file asynchronously and validates it.  Throws an exception if required fields are missing or invalid.
		/// </summary>
		/// <param name="filename">The file containing json to parse.  This is treated as a simple file path.  If you are attempting to load an embedded resource you need to impliment that yourself and call <see cref="ParseJson(string)"/> instead.</param>
		/// <returns>Returns a Task which returns the <see cref="OktaConfig"/> with fields filled from <paramref name="filename"/>.</returns>
		public static async Task<OktaConfig> LoadfromJsonFileAsync(string filename)
		{
			using (StreamReader reader = File.OpenText(filename))
			{
				return ParseJson(await reader.ReadToEndAsync());
			}
		}




		/// <summary>
		/// These are default values for optional configuration fields.
		/// </summary>
		public static readonly OktaConfig Defaults = new OktaConfig()
		{
			CallbackPath = "/authorization-code/callback",

			Scope = "openid profile",

			AuthorizationServerId = "default",
			ClockSkew = TimeSpan.FromMinutes(2)
		};
	}
}
