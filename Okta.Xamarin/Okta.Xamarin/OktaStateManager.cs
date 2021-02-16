// <copyright file="OktaState.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
    /// <summary>
    /// Tracks the current login state, including any access tokens, refresh tokens, scope, etc.
    /// </summary>
    public class OktaStateManager
    {
        private HttpClient client = new HttpClient();

        /// <summary>
        /// Gets the token type.
        /// </summary>
        public string TokenType { get; private set; } // TODO: delete this

        /// <summary>
        /// Gets the access token.
        /// </summary>
        public string AccessToken { get; private set; }

        /// <summary>
        /// Gets the id token.
        /// </summary>
        public string IdToken { get; private set; }

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        public string RefreshToken { get; private set; }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        public string Scope { get; private set; }

        /// <summary>
        /// Gets the expiration time.
        /// </summary>
        public DateTime Expires { get; private set; }

        public IOktaConfig Config { get; set; }

        #region CTors
        public OktaStateManager()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OktaStateManager"/> class.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="tokenType"></param>
        /// <param name="idToken"></param>
        /// <param name="refreshToken"></param>
        /// <param name="expiresIn"></param>
        /// <param name="scope"></param>
        public OktaStateManager(string accessToken, string tokenType, string idToken = null, string refreshToken = null, int? expiresIn = null, string scope = null)
        {
            this.TokenType = tokenType;
            this.AccessToken = accessToken;
            this.IdToken = idToken;
            this.RefreshToken = refreshToken;
            this.Expires = expiresIn.HasValue ? DateTime.UtcNow.AddSeconds(expiresIn.Value) : DateTime.MaxValue;
            this.Scope = scope;
        }
        #endregion

        /// <summary>
        /// Gets a value indicating whether or not there is a current non-expired <see cref="AccessToken"/>, indicating the user is currently successfully authenticated
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(AccessToken) && // there is an access token
                                Expires > DateTime.UtcNow;    // and it's not yet expired
            }
        }

        /// <summary>
        /// Gets or sets the last response received from the API.
        /// </summary>
        public HttpResponseMessage LastApiResponse { get; set; }

        /// <summary>
        /// Gets the token of the specified type.
        /// </summary>
        /// <param name="tokenType">The token type.</param>
        /// <returns>string</returns>
        public string GetToken(TokenType tokenType)
        {
            switch (tokenType)
            {
                case Xamarin.TokenType.AccessToken:
                    return AccessToken;
                    break;
                case Xamarin.TokenType.RefreshToken:
                default:
                    return RefreshToken;
                    break;
            }
        }

        /// <summary>
        /// Stores the tokens securely in platform-specific secure storage.  This is an async method and should be awaited.
        /// </summary>
        /// <returns>Task which tracks the progress of the save</returns>
        public async Task WriteToSecureStorageAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a stored state manager for a given config.  This is an async method and should be awaited.
        /// </summary>
        /// <param name="config">the Okta configuration that corresponds to a manager you are interested in</param>
        /// <returns>If a state manager is found for the provided config, this Task will return the <see cref="OktaStateManager"/>.</returns>
        public static async Task<OktaStateManager> ReadFromSecureStorageAsync(IOktaConfig config)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Revokes tokens associated with this OktaState.
        /// </summary>
        /// <returns></returns>
        public virtual async Task RevokeAsync(TokenType tokenType)
        {
            switch (tokenType)
            {
                case Xamarin.TokenType.AccessToken:
                    await RevokeAccessTokenAsync();
                    break;
                case Xamarin.TokenType.RefreshToken:
                default:
                    await RevokeRefreshTokenAsync();
                    break;
            }
        }

        /// <summary>
        /// Revokes the access token.
        /// </summary>
        /// <returns>Task</returns>
        protected async Task RevokeAccessTokenAsync()
        {
            _ = await PerformRequestAsync(HttpMethod.Post, "/revoke", new Dictionary<string, string>
                {
                    {"Bearer", AccessToken }
                }, new Dictionary<string, string>
                {
                    { "token", AccessToken },
                    { "token_type_hint", "access_token" },
                    { "client_id", Config.ClientId }
                });
        }

        /// <summary>
        /// Revokes the refresh token.
        /// </summary>
        /// <returns>Task</returns>
        protected async Task RevokeRefreshTokenAsync()
        {
            _ = await PerformRequestAsync(HttpMethod.Post, "/revoke", new Dictionary<string, string>
                {
                    {"Bearer", AccessToken }
                }, new Dictionary<string, string>
                {
                    { "token", RefreshToken },
                    { "token_type_hint", "refresh_token" },
                    { "client_id", Config.ClientId }
                });
        }

        /// <summary>
        /// Renew expired tokens by exchanging a refresh token for new ones.  Make sure to include the <c>offline_access</c> scope in your configuration.  
        /// </summary>
        /// <returns>Returns the new access token, also accessible in <see cref="AccessToken"/></returns>
        public async Task<string> RenewAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calls the OpenID Connect UserInfo endpoint with the stored access token to return user claim information.  This is an async method and should be awaited.
        /// </summary>
        /// <returns>A Task with a<see cref="System.Security.Claims.ClaimsPrincipal"/> representing the current user</returns>
        public async Task<System.Security.Claims.ClaimsPrincipal> GetUserAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the local authentication state by removing cached tokens in the keychain. A full sign out should consist of calling <see cref="OidcClient.SignOutOfOktaAsync"/>, then <see cref="RevokeAsync"/>, and then <see cref="Clear"/>.
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        protected async Task<string> PerformRequestAsync(HttpMethod httpMethod, string path, Dictionary<string, string> headers, Dictionary<string, string> formUrlEncodedContent)
        {
            return await PerformRequestAsync(httpMethod, path, headers, formUrlEncodedContent.Select(kvp => kvp).ToArray());
        }

        protected virtual async Task<string> PerformRequestAsync(HttpMethod httpMethod, string path, Dictionary<string, string> headers, params KeyValuePair<string, string>[] formUrlEncodedContent)
        {
            FormUrlEncodedContent content = null;
            if((bool)formUrlEncodedContent?.Any())
            {
                content = new FormUrlEncodedContent(formUrlEncodedContent.ToList());
            }
            if (!path.StartsWith("/"))
            {
                path = $"/{path}";
            }

            var request = new HttpRequestMessage(httpMethod, $"{GetBasePath()}{path}")
            {
                Content = content
            };
            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            LastApiResponse = response;
            string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return responseText;
        }

        protected virtual string GetBasePath()
        {
            return $"{Config?.OktaDomain}/oauth2/v1";
        }
    }
}
