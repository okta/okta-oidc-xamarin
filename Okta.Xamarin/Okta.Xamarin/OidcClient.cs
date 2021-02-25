// <copyright file="OidcClient.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
    /// <summary>
    /// A client for logging into Okta via Oidc.
    /// </summary>
    public abstract class OidcClient : IOidcClient
    {
        OAuthException oauthException;

        /// <summary>
        /// Gets the OAuthException that occurred if any.  Will be null if no exception occurred.
        /// </summary>
        public OAuthException OAuthException 
        {
            get
            {
                return this.oauthException;
            }

            set
            {
                this.oauthException = value;
            }
        }

        /// <summary>
        /// The configuration for this Client.
        /// </summary>
        public IOktaConfig Config { get; set; }

        /// <summary>
        /// Gets or sets the last API response, primarily for debugging.
        /// </summary>
        public HttpResponseMessage LastApiResponse { get; private set; }

        /// <summary>
        /// Maintains a list of all currently active Clients, by state.  This is used after the intent/universal link callback from login to continue the state machine.
        /// </summary>
        internal static Dictionary<string, IOidcClient> authenticatorsByState = new Dictionary<string, IOidcClient>();

        internal static Dictionary<string, IOidcClient> loggingOutClientsByState = new Dictionary<string, IOidcClient>();

        /// <summary>
        /// The <see cref="HttpClient"/> for use in getting an auth token.  Microsoft guidance specifies that this should be reused for performance reasons.
        /// </summary>
        protected HttpClient client = new HttpClient();

        /// <summary>
        /// A <see cref="OktaConfigValidator"/> used to validate any configuration used by Clients
        /// </summary>
        protected static readonly OktaConfigValidator<IOktaConfig> validator = new OktaConfigValidator<IOktaConfig>();

        /// <summary>
        /// Start the authorization flow.  This is an async method and should be awaited.
        /// </summary>
        /// <returns>In case of successful authorization, this Task will return a valid <see cref="OktaStateManager"/>.  Clients are responsible for further storage and maintenance of the manager.</returns>
        public Task<IOktaStateManager> SignInWithBrowserAsync()
        {
            validator.Validate(Config);

            currentTask = new TaskCompletionSource<IOktaStateManager>();
            GenerateStateCodeVerifierAndChallenge();
            authenticatorsByState.Add(State, this);
            this.LaunchBrowser(this.GenerateAuthorizeUrl());

            return currentTask.Task;
        }

        protected abstract void LaunchBrowser(string url);

        protected abstract void CloseBrowser();

        /// <summary>
        /// This method will end the user's Okta session in the browser.
        /// </summary>
        /// <param name="stateManager">The state manager associated with the login that you wish to log out</param>
        /// <returns>Task which tracks the progress of the logout</returns>
        public Task<IOktaStateManager> SignOutOfOktaAsync(IOktaStateManager stateManager)
        {
            validator.Validate(this.Config);
            this.currentTask = new TaskCompletionSource<IOktaStateManager>();
            if (!stateManager.IsAuthenticated)
            {
                this.currentTask.SetResult(stateManager);
                return this.currentTask.Task;
            }

            this.GenerateStateCodeVerifierAndChallenge();
            loggingOutClientsByState.Add(this.State, this);
            this.LaunchBrowser(this.GenerateLogoutUrl(new LogoutOptions(stateManager, this.Config, this.State)));
            return currentTask.Task;
        }

        /// <summary>
        /// Complete the authorization of a valid session obtained via the <see cref="https://github.com/okta/okta-auth-dotnet">AuthN SDK</see>.
        /// </summary>
        /// <param name="sessionToken">A valid session  token obtained via the <see cref="https://github.com/okta/okta-auth-dotnet">AuthN SDK</see></param>
        /// <returns>In case of successful authorization, this Task will return a valid <see cref="OktaStateManager"/>.  Clients are responsible for further storage and maintenance of the manager.</returns>
        public async Task<IOktaStateManager> AuthenticateAsync(string sessionToken)
        {
            throw new NotImplementedException("AuthN SDK is deprecated");
        }

        /// <summary>
        /// Gets introspection details.
        /// </summary>
        /// <param name="options">IntrospectionOptions</param>
        /// <returns>Dicationary{string, object}.</returns>
        public async Task<Dictionary<string, object>> IntrospectAsync(IntrospectOptions options)
        {
            return await IntrospectAsync(options.TokenKind, options.Token, options.AuthorizationServerId);
        }

        /// <summary>
        /// Gets introspection details.
        /// </summary>
        /// <param name="accessToken">The access token used to authroize the request.</param>
        /// <param name="tokenKind">The type of the token to introspect.</param>
        /// <param name="token">The target of introspection.</param>
        /// <param name="authorizationServerId">Authorization server ID.</param>
        /// <returns>Dictionary{string, object}.</returns>
        public async Task<Dictionary<string, object>> IntrospectAsync(TokenKind tokenKind, string token, string authorizationServerId = "default")
        {
            string tokenHint;
            switch (tokenKind)
            {
                case Xamarin.TokenKind.IdToken:
                    tokenHint = "id_token";
                    break;
                case Xamarin.TokenKind.RefreshToken:
                    tokenHint = "refresh_token";
                    break;
                case Xamarin.TokenKind.Invalid:
                case Xamarin.TokenKind.AccessToken:
                default:
                    tokenHint = "access_token";
                    break;
            }

            return await IntrospectAsync(token, tokenHint, authorizationServerId);
        }

        /// <summary>
        /// Gets information about the current user.
        /// </summary>
        /// <param name="accessToken">The access token used to authorize the request.</param>
        /// <param name="authorizationServerId">The authorization server id.</param>
        /// <returns>Dictionary{string, object}.</returns>
        public async Task<Dictionary<string, object>> GetUserAsync(string accessToken, string authorizationServerId = "default")
        {
            return await GetUserAsync<Dictionary<string, object>>(accessToken, authorizationServerId);
        }

        /// <summary>
        /// Gets an instance of the generic type T representing the current user.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response as.</typeparam>
        /// <param name="accessToken">The access token used to authorize the request.</param>
        /// <param name="authorizationServerId">The authorization server id.</param>
        /// <returns>T.</returns>
        public async Task<T> GetUserAsync<T>(string accessToken, string authorizationServerId = "default")
        {
            string userInfoJson = await GetUserInfoJsonAsync(accessToken, authorizationServerId);
            return JsonConvert.DeserializeObject<T>(userInfoJson);
        }

        /// <summary>
        /// Gets a claims principal representing the current user.
        /// </summary>
        /// <param name="accessToken">The access token used to authorize the request.</param>
        /// <param name="authorizationServerId">The authorization server id.</param>
        /// <returns>ClaimsPrincipal</returns>
        public async Task<ClaimsPrincipal> GetClaimsPincipalAsync(string accessToken, string authorizationServerId = "default")
        {
            string userInfoJson = await GetUserInfoJsonAsync(accessToken, authorizationServerId);

            UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(userInfoJson); // OKTA-371439 added to ensure proper mapping of all claims to ClaimsPrincipal

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.ToClaims(), "Okta");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return claimsPrincipal;
        }

        /// <summary>
        /// Renews tokens using the specified refresh token.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response as.</typeparam>
        /// <param name="refreshToken">The refresh token</param>
        /// <param name="refreshIdToken">A value indicating whether the id token should be refreshed.</param>
        /// <param name="authorizationServerId">The authorization server id.</param>
        /// <returns>T.</returns>
        public async Task<T> RenewAsync<T>(string refreshToken, bool refreshIdToken = false, string authorizationServerId = "default")
        {
            string responseJson = await GetRenewJsonAsync(refreshToken, refreshIdToken, authorizationServerId);
            return JsonConvert.DeserializeObject<T>(responseJson);
        }

        protected async Task<string> GetRenewJsonAsync(string refreshToken, bool refreshIdToken = false, string authorizationServerId = "default")
        {
            string scope = "offline_access";
            if (refreshIdToken)
            {
                scope += " openid";
            }

            return await PerformAuthorizationServerRequestAsync(HttpMethod.Post, $"/token?client_id={Config.ClientId}", new Dictionary<string, string>(), new Dictionary<string, string>()
            {
                { "grant_type", "refresh_token" },
                { "redirect_uri", Config.RedirectUri },
                { "scope", scope },
                { "refresh_token", refreshToken },
            }, authorizationServerId);
        }

        protected async Task<string> GetIntrospectJsonAsync(string token, string tokenTypeHint, string authorizationServerId = "default")
        {
            return await PerformAuthorizationServerRequestAsync(HttpMethod.Post, $"/introspect?client_id={Config.ClientId}", new Dictionary<string, string>(), new Dictionary<string, string>
            {
                { "token", token },
                { "token_type_hint", tokenTypeHint },
                { "client_id", Config.ClientId },
            }, authorizationServerId);
        }

        protected async Task<Dictionary<string, object>> IntrospectAsync(string token, string tokenTypeHint, string authorizationServerId = "default")
        {
            string responseJson = await GetIntrospectJsonAsync(token, tokenTypeHint, authorizationServerId);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);
        }

        protected async Task<string> GetUserInfoJsonAsync(string accessToken, string authorizationServerId = "default")
        {
            return await PerformAuthorizationServerRequestAsync(HttpMethod.Get, "/userinfo", new Dictionary<string, string>
            {
                {"Authorization", $"Bearer {accessToken}" }
            }, authorizationServerId);
        }

        protected async Task<string> PerformRequestAsync(HttpMethod httpMethod, string path, Dictionary<string, string> headers)
        {
            return await PerformRequestAsync(httpMethod, path, headers, new Dictionary<string, string>());
        }

        protected async Task<string> PerformRequestAsync(HttpMethod httpMethod, string path, Dictionary<string, string> headers, Dictionary<string, string> formUrlEncodedContent)
        {
            return await PerformRequestAsync(httpMethod, path, headers, formUrlEncodedContent.Select(kvp => kvp).ToArray());
        }

        protected virtual async Task<string> PerformRequestAsync(HttpMethod httpMethod, string path, Dictionary<string, string> headers, params KeyValuePair<string, string>[] formUrlEncodedContent)
        {
            FormUrlEncodedContent content = null;
            if ((bool)formUrlEncodedContent?.Any())
            {
                content = new FormUrlEncodedContent(formUrlEncodedContent.ToList());
            }

            if (!path.StartsWith("/"))
            {
                path = $"/{path}";
            }

            var request = GetHttpRequestMessage(httpMethod, $"{GetBasePath()}{path}", headers);
            request.Content = content;

            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            LastApiResponse = response;
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        protected virtual async Task<string> PerformAuthorizationServerRequestAsync(HttpMethod httpMethod, string path, Dictionary<string, string> headers, Dictionary<string, string> formUrlEncodedContent, string authorizationServerId = "default")
        {
            return await PerformAuthorizationServerRequestAsync(httpMethod, path, headers, authorizationServerId, formUrlEncodedContent.ToArray());
        }

        protected virtual async Task<string> PerformAuthorizationServerRequestAsync(HttpMethod httpMethod, string path, Dictionary<string, string> headers, string authorizationServerId = "default", params KeyValuePair<string, string>[] formUrlEncodedContent)
        {
            FormUrlEncodedContent content = null;
            if ((bool)formUrlEncodedContent?.Any())
            {
                content = new FormUrlEncodedContent(formUrlEncodedContent.ToList());
            }

            if (!path.StartsWith("/"))
            {
                path = $"/{path}";
            }

            var request = GetHttpRequestMessage(httpMethod, $"{GetAuthorizationServerBasePath(authorizationServerId)}{path}", headers);
            request.Content = content;

            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            LastApiResponse = response;
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        protected virtual string GetBasePath()
        {
            return $"{Config?.OktaDomain}/oauth2/v1";
        }

        protected virtual string GetAuthorizationServerBasePath(string authorizationServerId = "default")
        {
            return $"{Config?.OktaDomain}/oauth2/{authorizationServerId}/v1";
        }

        /// <summary>
        /// Gets a request message.
        /// </summary>
        /// <param name="method">The http method.</param>
        /// <param name="path">The path.</param>
        /// <param name="headers">The headers.</param>
        /// <returns>HttpRequestMessage</returns>
        protected HttpRequestMessage GetHttpRequestMessage(HttpMethod method, string path, Dictionary<string, string> headers)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(method, path);
            requestMessage.Headers.Add("Accept", "application/json");
            requestMessage.Headers.Add("User-Agent", OktaXamarinUserAgent.Value);
            foreach (string key in headers.Keys)
            {
                requestMessage.Headers.Add(key, headers[key]);
            }

            return requestMessage;
        }

        /// <summary>
        /// After a user logs in and is redirected back to the app via an intent or universal link, this method is called to parse the returned token and continue the flow
        /// </summary>
        /// <param name="url">The full callback url that the user was directed to</param>
        /// <returns>A Task which is complete when the login flow is completed.  The actual return value <see cref="OktaStateManager"/> or <see cref="OAuthException"/> is returned to the original Task returned from <see cref="SignInWithBrowserAsync"/>.</returns>
        private async Task ParseRedirectedUrl(Uri url) // TODO: refactor this implementation for readability and maintenance.
        {
            this.CloseBrowser();

            var queryData = System.Web.HttpUtility.ParseQueryString(url.Query).ToDictionary();

            // check if there is an error
            if (queryData.ContainsKey("error"))
            {
                OAuthException = new OAuthException()
                {
                    ErrorTitle = queryData["error"],
                    ErrorDescription = queryData.GetValueOrDefault("error_description"),
                    RequestUrl = url.ToString()
                };

                currentTask.SetException(OAuthException);

                return;
            }

            // confirm that the url matches the redirect url we expect
            if (!url.ToString().ToLower().StartsWith(Config.RedirectUri.ToLower()))
            {
                OAuthException = new OAuthException()
                {
                    ErrorTitle = "RedirectUri mismatch",
                    ErrorDescription = $"Expected RedirectUri of {Config.RedirectUri}, got {url.ToString()} instead.",
                    RequestUrl = url.ToString()
                };

                currentTask.SetException(OAuthException);

                return;
            }

            // confirm we received a code
            string code = queryData["code"];
            if (string.IsNullOrWhiteSpace(code))
            {
                OAuthException = new OAuthException()
                {
                    ErrorTitle = "No code returned in authorize request",
                    RequestUrl = url.ToString()
                };

                currentTask.SetException(OAuthException);
                return;
            }

            OAuthException = null;
            // now exchange authorization code for an access token
            await ExchangeAuthCodeForToken(code);
        }

        private async Task ClearStateAsync()
        {
            this.CloseBrowser();
            loggingOutClientsByState.Remove(State);
            currentTask.SetResult(new OktaStateManager());
        }

        /// <summary>
        /// Exchange authorization code for an access token
        /// </summary>
        /// <param name="code">The authorization code received from the login</param>
        /// <returns>A Task which is complete when the login flow is completed.  The actual return value <see cref="OktaStateManager"/> or <see cref="OAuthException"/> is returned to the original Task returned from <see cref="SignInWithBrowserAsync"/>.</returns>
        private async Task ExchangeAuthCodeForToken(string code)
        {
            List<KeyValuePair<string, string>> kvdata = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", this.Config.RedirectUri),
                new KeyValuePair<string, string>("client_id", this.Config.ClientId),
                new KeyValuePair<string, string>("code_verifier", CodeVerifier),
            };
            var content = new FormUrlEncodedContent(kvdata);

            var request = new HttpRequestMessage(HttpMethod.Post, this.Config.GetAccessTokenUrl())
                {Content = content, Method = HttpMethod.Post};
            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

            string text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            Dictionary<string, string> data = Helpers.JsonDecode(text);

            if (data.ContainsKey("error"))
            {
                OAuthException = new OAuthException()
                {
                    ErrorTitle = data["error"],
                    ErrorDescription = data.GetValueOrDefault("error_description"),
                    RequestUrl = this.Config.GetAccessTokenUrl(),
                    ExtraData = kvdata
                };

                currentTask.SetException(OAuthException);

                return;
            }

            // TODO: add a StateManager constructor that takes Dictionary<string, string>
            OktaStateManager stateManager = new OktaStateManager(
                data["access_token"],
                data["token_type"],
                data.GetValueOrDefault("id_token"),
                data.GetValueOrDefault("refresh_token"),
                data.ContainsKey("expires_in") ? (int?)(int.Parse(data["expires_in"])) : null,
                data.GetValueOrDefault("scope") ?? this.Config.Scope)
            {
                Config = Config,
                Client = this,
            };

            currentTask.SetResult(stateManager);
        }

        /// <summary>
        /// The internal OAuth state used to track requests from this client.
        /// </summary>
        protected string State { get; set; }

        /// <summary>
        /// The PKCE code that is used to verify the integrity of the token exchange.
        /// </summary>
        protected string CodeVerifier { get; set; }

        /// <summary>
        /// A SHA256 hash of the <see cref="CodeVerifier"/> used for PKCE.
        /// </summary>
        protected string CodeChallenge { get; set; }


        /// <summary>
        /// Tracks the current state machine used by <see cref="SignInWithBrowserAsync"/> across the login callback.
        /// </summary>
        protected TaskCompletionSource<IOktaStateManager> currentTask;

        /// <summary>
        /// Generates a cryptographically random <see cref="State"/> and <see cref="CodeVerifier"/>, and computes the <see cref="CodeChallenge"/> for use in PKCE
        /// </summary>
        private void GenerateStateCodeVerifierAndChallenge()
        {
            State = GenerateSecureRandomString(16);
            CodeVerifier = GenerateSecureRandomString(64);
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(CodeVerifier));
                CodeChallenge = Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(bytes);
            }
        }

        private string GenerateSecureRandomString(int byteCount)
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[byteCount];
                rng.GetBytes(data);

                return Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(data);
            }
        }

        private string GenerateLogoutUrl(LogoutOptions logoutOptions)
        {
            var baseUri = new Uri(this.Config.GetLogoutUrl());
            var logoutUri = $"{baseUri.AbsoluteUri}{logoutOptions.ToQueryString(true)}";
            return logoutUri;
        }

        /// <summary>
        /// Determines the AuthorizeUrl including login query parameters based on the <see cref="Config"/>
        /// </summary>
        /// <returns>The url ready to be used for login</returns>
        protected string GenerateAuthorizeUrl()
        {
            var baseUri = new Uri(this.Config.GetAuthorizeUrl());
            string url = baseUri.AbsoluteUri;

            // remove fragment if any
            if (url.Contains('#'))
            {
                url = url.Substring(0, url.IndexOf('#'));
            }

            // if url already has a query, then append to it
            if (!baseUri.PathAndQuery.Contains('?'))
            {
                url += "?";
            }
            else
            {
                url += "&";
            }

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

        public static async Task<bool> InterceptLoginCallbackAsync(Uri uri)
        {
            return await Task.Run(() => InterceptLoginCallback(uri));
        }

        public static bool InterceptLoginCallback(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (string.IsNullOrEmpty(uri.Query))
            {
                return false;
            }

            // get the state from the uri
            string uriString = uri.Query.ToString();
            var parsed = System.Web.HttpUtility.ParseQueryString(uriString);
            string state = parsed["state"];

            if (string.IsNullOrEmpty(state))
            {
                return false;
            }

            if (OidcClient.authenticatorsByState.ContainsKey(state))
            {
                // state is valid for a current client, so continue the flow with that client
                _ = ((OidcClient)authenticatorsByState[state]).ParseRedirectedUrl(uri);
                return true;
            }
            else
            {
                // there is no client matching that state.  Rather than throw an error, return false, as it's possible the application is handling callbacks from multiple different universal links
                return false;
            }
        }

        public static bool InterceptLogoutCallback(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (string.IsNullOrEmpty(uri.Query))
            {
                return false;
            }

            // get the state from the uri
            string uriString = uri.Query.ToString();
            var parsed = System.Web.HttpUtility.ParseQueryString(uriString);
            string state = parsed["state"];

            if (string.IsNullOrEmpty(state))
            {
                return false;
            }

            if (OidcClient.loggingOutClientsByState.ContainsKey(state))
            {
                _ = ((OidcClient)loggingOutClientsByState[state]).ClearStateAsync();
                return true;
            }
            else
            {
                // there is no client matching that state.  Rather than throw an error, return false, as it's possible the application is handling callbacks from multiple different universal links
                return false;
            }
        }

        /// <summary>
        /// Call after a user logs in and is redirected back to the app via an intent or universal link.  This method determines the appropriate <see cref="OidcClient"/> to continue the flow based on the <see cref="State"/>.
        /// </summary>
        /// <param name="uri">The full callback url that the user was directed to</param>
        /// <returns><see langword="true"/> if this url can be handled by an <see cref="OidcClient"/>, or <see langword="false"/> if it is some other url which is not handled by the login flow.</returns>
        [Obsolete("Use InterceptLoginCallback instead")]
        public static bool CaptureRedirectUrl(Uri uri)
        {
            return InterceptLoginCallback(uri);
        }

        public async Task RevokeAccessTokenAsync(string accessToken)
        {
            _ = await this.PerformAuthorizationServerRequestAsync(HttpMethod.Post, "/revoke", new Dictionary<string, string>(), new Dictionary<string, string> { { "token", accessToken }, { "token_type_hint", "access_token" }, { "client_id", this.Config.ClientId } });
        }

        public async Task RevokeRefreshTokenAsync(string accessToken, string refreshToken)
        {
            _ = await this.PerformAuthorizationServerRequestAsync(HttpMethod.Post, "/revoke", new Dictionary<string, string>(), new Dictionary<string, string> { { "token", refreshToken }, { "token_type_hint", "refresh_token" }, { "client_id", this.Config.ClientId } });
        }
    }
}