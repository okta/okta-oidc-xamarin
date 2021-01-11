// <copyright file="OktaState.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
    /// <summary>
    /// Tracks the current login state, including any access tokens, refresh tokens, scope, etc.
    /// </summary>
    public class OktaState
    {
        /// <summary>
        /// Gets the token type.
        /// </summary>
        public string TokenType { get; private set; }

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
        
        #region CTors
        public OktaState()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OktaState"/> class.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="tokenType"></param>
        /// <param name="idToken"></param>
        /// <param name="refreshToken"></param>
        /// <param name="expiresIn"></param>
        /// <param name="scope"></param>
        public OktaState(string accessToken, string tokenType, string idToken = null, string refreshToken = null, int? expiresIn = null, string scope = null)
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
        /// <returns>If a state manager is found for the provided config, this Task will return the <see cref="OktaState"/>.</returns>
        public static async Task<OktaState> ReadFromSecureStorageAsync(IOktaConfig config)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Revokes tokens associated with this OktaState.
        /// </summary>
        /// <returns></returns>
        public async Task RevokeAsync()
        {
            throw new NotImplementedException();
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
    }
}
