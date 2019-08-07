using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
	public partial class OidcClient : IOidcClient
	{
		public IOktaConfig Config { get; private set; }

		public static Dictionary<string, IOidcClient> currentAuthenticatorbyState = new Dictionary<string, IOidcClient>();

		private HttpClient client = new HttpClient();

		private static readonly OktaConfigValidator<IOktaConfig> validator = new OktaConfigValidator<IOktaConfig>();



		/// <summary>
		/// Start the authorization flow.  This is an async method and should be awaited.
		/// </summary>
		/// <returns>In case of successful authorization, this Task will return a valid <see cref="StateManager"/>.  Clients are responsible for further storage and maintenance of the manager.</returns>
		public Task<StateManager> SignInWithBrowserAsync()
		{
			validator.Validate(Config);

			currentTask = new TaskCompletionSource<StateManager>();
			GenerateStateCodeVerifierAndChallenge();
			currentAuthenticatorbyState.Add(State, this);
			this.LaunchBrowser(this.GenerateAuthorizeUrl());
			
			return currentTask.Task;
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



		//private static string CreateIssuerUrl(string oktaDomain, string authorizationServerId)
		//{
		//	if (string.IsNullOrEmpty(oktaDomain))
		//	{
		//		throw new ArgumentNullException(nameof(oktaDomain));
		//	}

		//	if (string.IsNullOrEmpty(authorizationServerId))
		//	{
		//		return oktaDomain;
		//	}

		//	return oktaDomain.EndsWith("/") ? oktaDomain : $"{oktaDomain}/"
		//		+ "oauth2/" + authorizationServerId;
		//}

		public async Task ParseRedirectedUrl(Uri url)
		{
			Debug.WriteLine("ParseRedirectedUrl " + url.ToString());
			this.CloseBrowser();

			var all = System.Web.HttpUtility.ParseQueryString(url.Query).ToDictionary();



			// check if there is an error
			if (all.ContainsKey("error"))
			{
				string description = all["error"];
				if (all.ContainsKey("error_description"))
				{
					description = all["error_description"];
				}
				currentTask.SetException(new OAuthException()
				{
					ErrorTitle = all["error"],
					ErrorDescription = all.GetValueOrDefault("error_description"),
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

			string code = all["code"];
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
			List<KeyValuePair<string, string>> kvdata = new List<KeyValuePair<string, string>>();
			kvdata.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
			kvdata.Add(new KeyValuePair<string, string>("code", code));
			kvdata.Add(new KeyValuePair<string, string>("redirect_uri", this.Config.RedirectUri));
			kvdata.Add(new KeyValuePair<string, string>("client_id", this.Config.ClientId));
			kvdata.Add(new KeyValuePair<string, string>("code_verifier", CodeVerifier));
			var content = new FormUrlEncodedContent(kvdata);

			var request = new HttpRequestMessage(HttpMethod.Post, this.Config.GetAccessTokenUrl()) { Content = content, Method = HttpMethod.Post };
			HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
			
			string text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			var data = Helpers.JsonDecode(text);

			if (data.ContainsKey("error"))
			{
				string description = data["error"];
				if (data.ContainsKey("error_description"))
				{
					description = data["error_description"];
				}
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

		private string State { get; set; }
		private string CodeVerifier { get; set; }
		private string CodeChallenge { get; set; }
		private TaskCompletionSource<StateManager> currentTask;

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

		private string GenerateAuthorizeUrl()
		{
			var baseUri = new Uri(this.Config.GetAuthorizeUri());
			string url = baseUri.AbsoluteUri;

			// remove fragement if any
			if (url.Contains('#'))
				url = url.Substring(0, url.IndexOf('#'));

			// if url already has a query, then append to it
			if (!baseUri.PathAndQuery.Contains('?'))
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

		public static bool CaptureRedirectUrl(Uri uri)
		{
			string uriString = uri.Query.ToString();
			var parsed = System.Web.HttpUtility.ParseQueryString(uriString);
			string state = parsed["state"];

			if (OidcClient.currentAuthenticatorbyState.ContainsKey(state))
			{
				OidcClient.currentAuthenticatorbyState[state].ParseRedirectedUrl(uri);
				return true;
			}
			else
			{
				return false;
			}
		}

	}
}
