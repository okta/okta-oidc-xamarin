// <copyright file="OktaState.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
    /// <summary>
    /// Tracks the current login state, including any access tokens, refresh tokens, scope, etc.
    /// </summary>
    public class OktaStateManager : IOktaStateManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OktaStateManager"/> class.
        /// </summary>
        public OktaStateManager()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OktaStateManager"/> class.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="tokenType">The token type.</param>
        /// <param name="idToken">The ID token.</param>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="expiresIn">Expires in.</param>
        /// <param name="scope">Scope.</param>
        public OktaStateManager(string accessToken, string tokenType, string idToken = null, string refreshToken = null, int? expiresIn = null, string scope = null)
        {
            this.TokenType = tokenType;
            this.AccessToken = accessToken;
            this.IdToken = idToken;
            this.RefreshToken = refreshToken;
            this.Expires = expiresIn.HasValue ? DateTime.UtcNow.AddSeconds(expiresIn.Value) : DateTime.MaxValue;
            this.Scope = scope;
        }

        /// <inheritdoc/>
        public string TokenType { get; private set; }

        /// <inheritdoc/>
        public string AccessToken { get; private set; }

        /// <inheritdoc/>
        public string IdToken { get; private set; }

        /// <inheritdoc/>
        public string RefreshToken { get; private set; }

        /// <inheritdoc/>
        public string Scope { get; private set; }

        /// <inheritdoc/>
        public DateTime? Expires { get; private set; }

        /// <summary>
        /// Gets the most recent renew response or null.
        /// </summary>
        public RenewResponse RenewResponse { get; private set; }

        /// <inheritdoc/>
        public IOktaConfig Config { get; set; }

        /// <inheritdoc/>
        public IOidcClient Client { get; set; }

        /// <inheritdoc/>
        public bool IsAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(AccessToken) && // there is an access token
                                Expires > DateTime.UtcNow;    // and it's not yet expired
            }
        }

        /// <inheritdoc/>
        public HttpResponseMessage LastApiResponse { get => Client?.LastApiResponse; }

        /// <inheritdoc/>
        public string GetToken(TokenKind tokenKind)
        {
            switch (tokenKind)
            {
                case Xamarin.TokenKind.Invalid:
                case Xamarin.TokenKind.AccessToken:
                    return AccessToken;
                case Xamarin.TokenKind.IdToken:
                    return IdToken;
                case Xamarin.TokenKind.RefreshToken:
                default:
                    return RefreshToken;
            }
        }

        /// <inheritdoc/>
        public async Task WriteToSecureStorageAsync() // to be implemented on OKTA-363613 v1.1
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<OktaStateManager> ReadFromSecureStorageAsync(IOktaConfig config) // to be implemented on OKTA-363613 v1.1
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual async Task RevokeAsync(TokenKind tokenType)
        {
            switch (tokenType)
            {
                case Xamarin.TokenKind.AccessToken:
                    await Client.RevokeAccessTokenAsync(AccessToken);
                    break;
                case Xamarin.TokenKind.RefreshToken:
                default:
                    await Client.RevokeRefreshTokenAsync(RefreshToken);
                    break;
            }
        }

        /// <inheritdoc/>
        public async Task<T> GetUserAsync<T>(string authorizationServerId = "default")
        {
            return await Client.GetUserAsync<T>(AccessToken, authorizationServerId);
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, object>> GetUserAsync(string authorizationServerId = "default")
        {
            return await Client.GetUserAsync(AccessToken, authorizationServerId);
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, object>> IntrospectAsync(TokenKind tokenKind, string authorizationServerId = "default")
        {
            return await Client.IntrospectAsync(new IntrospectOptions
            {
                Token = GetToken(tokenKind),
                TokenKind = tokenKind,
                AuthorizationServerId = authorizationServerId,
            });
        }

        /// <inheritdoc/>
        public async Task<ClaimsPrincipal> GetClaimsPrincipalAsync(string authorizationServerId = "default")
        {
            return await Client.GetClaimsPincipalAsync(AccessToken, authorizationServerId);
        }

        /// <inheritdoc/>
        public async Task<RenewResponse> RenewAsync(bool refreshIdToken = false, string authorizationServerId = "default")
        {
            RenewResponse renewResponse = await Client.RenewAsync<RenewResponse>(RefreshToken, refreshIdToken, authorizationServerId);
            RenewResponse = renewResponse;
            TokenType = renewResponse.TokenType;
            AccessToken = renewResponse.AccessToken;
            RefreshToken = renewResponse.RefreshToken;
            if(refreshIdToken && !string.IsNullOrEmpty(renewResponse?.IdToken))
            {
                IdToken = renewResponse.IdToken;
            }

            return renewResponse;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            AccessToken = string.Empty;
            IdToken = string.Empty;
            RefreshToken = string.Empty;
            Scope = string.Empty;
            Expires = null;
        }
    }
}
