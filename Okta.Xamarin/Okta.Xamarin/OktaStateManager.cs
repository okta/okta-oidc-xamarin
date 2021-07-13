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
        private IOidcClient client;

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
            this.Expires = expiresIn.HasValue ? new DateTimeOffset(DateTime.UtcNow.AddSeconds(expiresIn.Value)) : default;
            this.Scope = scope;
        }

        /// <inheritdoc/>
        public event EventHandler<RequestExceptionEventArgs> RequestException;

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
        public DateTimeOffset? Expires { get; private set; }

        /// <summary>
        /// Gets the most recent renew response or null.
        /// </summary>
        public RenewResponse RenewResponse { get; private set; }

        /// <inheritdoc/>
        public IOktaConfig Config { get; set; }

        /// <inheritdoc/>
        public IOidcClient Client
        {
            get => this.client;
            set
            {
                this.client = value;
                this.client.RequestException += (sender, args) => this.RequestException?.Invoke(sender, args);
            }
        }

        /// <inheritdoc/>
        public bool IsAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(this.AccessToken) && // there is an access token
                                (this.Expires == default(DateTimeOffset) || this.Expires > DateTime.UtcNow);    // and it's not yet expired
            }
        }

        /// <inheritdoc/>
        public HttpResponseMessage LastApiResponse { get => this.Client?.LastApiResponse; }

        /// <inheritdoc/>
        public string GetToken(TokenKind tokenKind)
        {
            switch (tokenKind)
            {
                case Xamarin.TokenKind.Invalid:
                case Xamarin.TokenKind.AccessToken:
                    return this.AccessToken;
                case Xamarin.TokenKind.IdToken:
                    return this.IdToken;
                case Xamarin.TokenKind.RefreshToken:
                default:
                    return this.RefreshToken;
            }
        }

        /// <inheritdoc/>
        public async Task WriteToSecureStorageAsync() // to be implemented on OKTA-363613 v2.1
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<OktaStateManager> ReadFromSecureStorageAsync(IOktaConfig config) // to be implemented on OKTA-363613 v2.1
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual async Task RevokeAsync(TokenKind tokenType)
        {
            switch (tokenType)
            {
                case Xamarin.TokenKind.AccessToken:
                    await this.Client.RevokeAccessTokenAsync(this.AccessToken);
                    break;
                case Xamarin.TokenKind.RefreshToken:
                default:
                    await this.Client.RevokeRefreshTokenAsync(this.RefreshToken);
                    break;
            }
        }

        /// <inheritdoc/>
        public async Task<T> GetUserAsync<T>(string authorizationServerId = "default")
        {
            return await this.Client.GetUserAsync<T>(this.AccessToken, authorizationServerId);
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, object>> GetUserAsync(string authorizationServerId = "default")
        {
            return await this.Client.GetUserAsync(this.AccessToken, authorizationServerId);
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, object>> IntrospectAsync(TokenKind tokenKind, string authorizationServerId = "default")
        {
            return await this.Client.IntrospectAsync(new IntrospectOptions
            {
                Token = this.GetToken(tokenKind),
                TokenKind = tokenKind,
                AuthorizationServerId = authorizationServerId,
            });
        }

        /// <inheritdoc/>
        public async Task<ClaimsPrincipal> GetClaimsPrincipalAsync(string authorizationServerId = "default")
        {
            return await this.Client.GetClaimsPincipalAsync(this.AccessToken, authorizationServerId);
        }

        /// <inheritdoc/>
        public async Task<RenewResponse> RenewAsync(bool refreshIdToken = false, string authorizationServerId = "default")
        {
            return await this.RenewAsync(this.RefreshToken, refreshIdToken, authorizationServerId);
        }

        /// <inheritdoc/>
        public async Task<RenewResponse> RenewAsync(string refreshToken, bool refreshIdToken = false, string authorizationServerId = "default")
        {
            RenewResponse renewResponse = await this.Client.RenewAsync<RenewResponse>(refreshToken, refreshIdToken, authorizationServerId);
            this.RenewResponse = renewResponse;
            this.TokenType = renewResponse.TokenType;
            this.AccessToken = renewResponse.AccessToken;
            this.RefreshToken = renewResponse.RefreshToken;
            this.Expires = new DateTimeOffset(DateTime.UtcNow.AddSeconds(renewResponse.ExpiresIn));
            if (refreshIdToken && !string.IsNullOrEmpty(renewResponse?.IdToken))
            {
                this.IdToken = renewResponse.IdToken;
            }

            return renewResponse;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this.AccessToken = string.Empty;
            this.IdToken = string.Empty;
            this.Scope = string.Empty;
            this.Expires = null;
            this.RefreshToken = string.Empty;
        }
    }
}
