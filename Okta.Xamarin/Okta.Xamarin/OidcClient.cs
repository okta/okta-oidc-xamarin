// <copyright file="OidcClient.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
	/// <summary>
	/// A client for logging into Okta via Oidc
	/// </summary>
	public partial class OidcClient : IOidcClient
	{
		/// <summary>
		/// The configuration for this Client.  Must be set in the constructor.
		/// </summary>
		public IOktaConfig Config { get; private set; }

		/// <summary>
		/// Maintains a list of all currently active Clients, by state.  This is used after the intent/universal link callback from login to continue the state machine.
		/// </summary>
		internal static Dictionary<string, IOidcClient> currentAuthenticatorbyState = new Dictionary<string, IOidcClient>();

		/// <summary>
		/// The <see cref="HttpClient"/> for use in getting an auth token.  Microsoft guidance specifies that this should be reused for performance reasons.
		/// </summary>
		private HttpClient client = new HttpClient();

		/// <summary>
		/// A <see cref="OktaConfigValidator"/> used to validate any configuration used by Clients
		/// </summary>
		private static readonly OktaConfigValidator<IOktaConfig> validator = new OktaConfigValidator<IOktaConfig>();



		/// <summary>
		/// Start the authorization flow.  This is an async method and should be awaited.
		/// </summary>
		/// <returns>In case of successful authorization, this Task will return a valid <see cref="StateManager"/>.  Clients are responsible for further storage and maintenance of the manager.</returns>
		public
#if WINDOWS_UWP
			async
#endif
			Task<StateManager> SignInWithBrowserAsync()
		{
			validator.Validate(Config);

			currentTask = new TaskCompletionSource<StateManager>();
			GenerateStateCodeVerifierAndChallenge();
			currentAuthenticatorbyState.Add(State, this);

#if !WINDOWS_UWP
			this.LaunchBrowser(this.GenerateAuthorizeUrl());
			return currentTask.Task;
#else
			await this.LaunchBrowserAsync(this.GenerateAuthorizeUrl()).ConfigureAwait(false);
			return await currentTask.Task;
#endif
		}

		/// <summary>
		/// This method will end the user's Okta session in the browser.  This is an async method and should be awaited.
		/// </summary>
		/// <param name="stateManager">The state manager associated with the login that you wish to log out</param>
		/// <returns>Task which tracks the progress of the logout</returns>
		public async Task SignOutOfOktaAsync(StateManager stateManager)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Complete the authorization of a valid session obtained via the <see cref="https://github.com/okta/okta-auth-dotnet">AuthN SDK</see>.
		/// </summary>
		/// <param name="sessionToken">A valid session  token obtained via the <see cref="https://github.com/okta/okta-auth-dotnet">AuthN SDK</see></param>
		/// <returns>In case of successful authorization, this Task will return a valid <see cref="StateManager"/>.  Clients are responsible for further storage and maintenance of the manager.</returns>
		public async Task<StateManager> AuthenticateAsync(string sessionToken)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// After a user logs in and is redirected back to the app via an intent or universal link, this method is called to parse the returned token and continue the flow
		/// </summary>
		/// <param name="url">The full callback url that the user was directed to</param>
		/// <returns>A Task which is complete when the login flow is completed.  The actual return value <see cref="StateManager"/> or <see cref="OAuthException"/> is returned to the original Task returned from <see cref="SignInWithBrowserAsync"/>.</returns>
		private async Task ParseRedirectedUrl(Uri url)
		{
			this.CloseBrowser();

			var queryData = System.Web.HttpUtility.ParseQueryString(url.Query).ToDictionary();

			// check if there is an error
			if (queryData.ContainsKey("error"))
			{
				currentTask.SetException(new OAuthException()
				{
					ErrorTitle = queryData["error"],
					ErrorDescription = queryData.GetValueOrDefault("error_description"),
					RequestUrl = url.ToString()
				});

				return;
			}

			// confirm that the url matches the redirect url we expect
			if (!url.ToString().ToLower().StartsWith(Config.RedirectUri.ToLower()))
			{
				currentTask.SetException(new OAuthException()
				{
					ErrorTitle = "RedirectUri mismatch",
					ErrorDescription = $"Expected RedirectUri of {Config.RedirectUri}, got {url.ToString()} instead.",
					RequestUrl = url.ToString()
				});

				return;
			}

			// confirm we received a code
			string code = queryData["code"];
			if (string.IsNullOrWhiteSpace(code))
			{
				currentTask.SetException(new OAuthException()
				{
					ErrorTitle = "No code returned in authorize request",
					RequestUrl = url.ToString()
				});
				return;
			}

			// now exchange authorization code for an access token
			await ExchangeAuthCodeForToken(code);
		}

		/// <summary>
		/// Exchange authorization code for an access token
		/// </summary>
		/// <param name="code">The authorization code received from the login</param>
		/// <returns>A Task which is complete when the login flow is completed.  The actual return value <see cref="StateManager"/> or <see cref="OAuthException"/> is returned to the original Task returned from <see cref="SignInWithBrowserAsync"/>.</returns>
		private async Task ExchangeAuthCodeForToken(string code)
		{
			List<KeyValuePair<string, string>> kvdata = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("grant_type", "authorization_code"),
				new KeyValuePair<string, string>("code", code),
				new KeyValuePair<string, string>("redirect_uri", this.Config.RedirectUri),
				new KeyValuePair<string, string>("client_id", this.Config.ClientId),
				new KeyValuePair<string, string>("code_verifier", CodeVerifier)
			};
			var content = new FormUrlEncodedContent(kvdata);

			var request = new HttpRequestMessage(HttpMethod.Post, this.Config.GetAccessTokenUrl()) { Content = content, Method = HttpMethod.Post };
			HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

			string text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			Dictionary<string, string> data = Helpers.JsonDecode(text);

			if (data.ContainsKey("error"))
			{
				currentTask.SetException(new OAuthException()
				{
					ErrorTitle = data["error"],
					ErrorDescription = data.GetValueOrDefault("error_description"),
					RequestUrl = this.Config.GetAccessTokenUrl(),
					ExtraData = kvdata
				});

				return;
			}

			StateManager state = new StateManager(
				data["access_token"],
				data["token_type"],
				data.GetValueOrDefault("id_token"),
				data.GetValueOrDefault("refresh_token"),
				data.ContainsKey("expires_in") ? (int?)(int.Parse(data["expires_in"])) : null,
				data.GetValueOrDefault("scope") ?? this.Config.Scope);

			currentTask.SetResult(state);
		}

		/// <summary>
		/// The internal OAuth state used to track requests from this client
		/// </summary>
		private string State { get; set; }
		/// <summary>
		/// The PKCE code that is used to verify the integrity of the token exchange
		/// </summary>
		private string CodeVerifier { get; set; }
		/// <summary>
		/// A SHA256 hash of the <see cref="CodeVerifier"/> used for PKCE
		/// </summary>
		private string CodeChallenge { get; set; }
		/// <summary>
		/// Tracks the current state machine used by <see cref="SignInWithBrowserAsync"/> across the login callback
		/// </summary>
		private TaskCompletionSource<StateManager> currentTask;

		/// <summary>
		/// Generates a cryptographically random <see cref="State"/> and <see cref="CodeVerifier"/>, and computes the <see cref="CodeChallenge"/> for use in PKCE
		/// </summary>
		private void GenerateStateCodeVerifierAndChallenge()
		{
			using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
			{
				byte[] stateData = new byte[16];
				rng.GetBytes(stateData);

				State = Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(stateData).Substring(0, 16);
			}

			using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
			{
				byte[] tokenData = new byte[64];
				rng.GetBytes(tokenData);

				CodeVerifier = Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(tokenData);
			}

			using (SHA256 sha256Hash = SHA256.Create())
			{
				byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(CodeVerifier));
				CodeChallenge = Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(bytes);
			}
		}

		/// <summary>
		/// Determines the AuthorizeUrl including login query parameters based on the <see cref="Config"/>
		/// </summary>
		/// <returns>The url ready to be used for login</returns>
		private string GenerateAuthorizeUrl()
		{
			var baseUri = new Uri(this.Config.GetAuthorizeUrl());
			string url = baseUri.AbsoluteUri;

			// remove fragment if any
			if (url.Contains("#"))
				url = url.Substring(0, url.IndexOf('#'));

			// if url already has a query, then append to it
			if (!baseUri.PathAndQuery.Contains("?"))
				url += "?";
			else
				url += "&";

			// add authorize url query parameters
			url += "response_type=code" +
				 "&client_id=" + this.Config.ClientId +
				 "&redirect_uri=" + System.Uri.EscapeDataString(this.Config.RedirectUri) +
				 "&state=" + State +
				 "&code_challenge=" + CodeChallenge +
				 "&code_challenge_method=S256" +
				 "&scope=" + System.Uri.EscapeDataString(this.Config.Scope);

			return url;
		}

		/// <summary>
		/// Call after a user logs in and is redirected back to the app via an intent or universal link.  This method determines the appropriate <see cref="OidcClient"/> to continue the flow based on the <see cref="State"/>.
		/// </summary>
		/// <param name="uri">The full callback url that the user was directed to</param>
		/// <returns><see langword="true"/> if this url can be handled by an <see cref="OidcClient"/>, or <see langword="false"/> if it is some other url which is not handled by the login flow.</returns>
		public static bool CaptureRedirectUrl(Uri uri)
		{
			if (uri == null)
				throw new ArgumentNullException(nameof(uri));

			if (string.IsNullOrEmpty(uri.Query))
				return false;

			// get the state from the uri
			string uriString = uri.Query.ToString();
			var parsed = System.Web.HttpUtility.ParseQueryString(uriString);
			string state = parsed["state"];

			if (string.IsNullOrEmpty(state))
				return false;

			if (OidcClient.currentAuthenticatorbyState.ContainsKey(state))
			{
				// state is valid for a current client, so continue the flow with that client
				((OidcClient)OidcClient.currentAuthenticatorbyState[state]).ParseRedirectedUrl(uri);
				return true;
			}
			else
			{
				// there is no client matching that state.  Rather than throw an error, return false, as it's possible the application is handling callbacks from multiple different universal links
				return false;
			}
		}

	}
}
