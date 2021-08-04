// <copyright file="OktaStateManager.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Okta.Xamarin.Services;

namespace Okta.Xamarin
{
    /// <summary>
    /// Tracks the current login state, including any access tokens, refresh tokens, scope, etc.
    /// </summary>
    public class OktaStateManager : IOktaStateManager
    {
        /// <summary>
        /// The key used to store and retrieve a representation of the state manager in secure storage. 
        /// </summary>
        public const string StoreKey = "OktaState";

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
        public event EventHandler<SecureStorageExceptionEventArgs> SecureStorageWriteException;

        /// <inheritdoc/>
        public event EventHandler<SecureStorageExceptionEventArgs> SecureStorageReadException;

        /// <inheritdoc/>
        public event EventHandler<SecureStorageEventArgs> SecureStorageWriteStarted;

        /// <inheritdoc/>
        public event EventHandler<SecureStorageEventArgs> SecureStorageWriteCompleted;

        /// <inheritdoc/>
        public event EventHandler<SecureStorageEventArgs> SecureStorageReadStarted;

        /// <inheritdoc/>
        public event EventHandler<SecureStorageEventArgs> SecureStorageReadCompleted;

        /// <inheritdoc/>
        public string TokenType { get; set; }

        /// <inheritdoc/>
        public string AccessToken { get; set; }

        /// <inheritdoc/>
        public string IdToken { get; set; }

        /// <inheritdoc/>
        public string RefreshToken { get; set; }

        /// <inheritdoc/>
        public string Scope { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset? Expires { get; set; }

        /// <summary>
        /// Gets the most recent renew response or null.
        /// </summary>
        [JsonIgnore]
        public RenewResponse RenewResponse { get; private set; }

        /// <inheritdoc/>
        [JsonIgnore]
        public IOktaConfig Config { get; set; }

        /// <inheritdoc/>
        [JsonIgnore]
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
        [JsonIgnore]
        public bool IsAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(this.AccessToken) && // there is an access token
                                (this.Expires == default(DateTimeOffset) || this.Expires > DateTime.UtcNow);    // and it's not yet expired
            }
        }

        /// <inheritdoc/>
        [JsonIgnore]
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

        /// <summary>
        /// Copies the state of the specified state manager to the current instance.
        /// </summary>
        /// <param name="oktaStateManager">The state manager to copy.</param>
        protected void Copy(OktaStateManager oktaStateManager)
        {
            if (oktaStateManager == null)
            {
                return;
            }

            this.Config = oktaStateManager.Config;
            this.Client = oktaStateManager.Client;
            this.TokenType = oktaStateManager.TokenType;
            this.AccessToken = oktaStateManager.AccessToken;
            this.IdToken = oktaStateManager.IdToken;
            this.RefreshToken = oktaStateManager.RefreshToken;
            this.Scope = oktaStateManager.Scope;
            this.Expires = oktaStateManager.Expires;
        }

        /// <inheritdoc/>
        public async Task WriteToSecureStorageAsync()
        {
            try
            {
                this.OnSecureStorageWriteStarted(new SecureStorageEventArgs { OktaStateManager = this });
                SecureKeyValueStore secureKeyValueStore = OktaContext.GetService<SecureKeyValueStore>();
                await secureKeyValueStore.SetAsync(StoreKey, this.ToJson());
                this.OnSecureStorageWriteCompleted(new SecureStorageEventArgs { OktaStateManager = this });
            }
            catch (Exception ex)
            {
                this.OnSecureStorageWriteException(new SecureStorageExceptionEventArgs { Exception = ex });
            }
        }

        /// <summary>
        /// Raises the SecureStorageWriteStarted event.
        /// </summary>
        /// <param name="eventArgs">The event arguments.</param>
        protected void OnSecureStorageWriteStarted(SecureStorageEventArgs eventArgs)
        {
            this.SecureStorageWriteStarted?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Raises the SecureStorageWriteCompleted event.
        /// </summary>
        /// <param name="eventArgs">The event arguments.</param>
        protected void OnSecureStorageWriteCompleted(SecureStorageEventArgs eventArgs)
        {
            this.SecureStorageWriteCompleted?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Raises the SecureStorageWriteException event.
        /// </summary>
        /// <param name="eventArgs">The event arguments.</param>
        protected void OnSecureStorageWriteException(SecureStorageExceptionEventArgs eventArgs)
        {
            this.SecureStorageWriteException?.Invoke(this, eventArgs);
        }

        /// <inheritdoc/>
        public async Task<OktaStateManager> ReadFromSecureStorageAsync()
        {
            return await this.ReadFromSecureStorageAsync(OktaContext.GetService<IOktaConfig>());
        }

        /// <inheritdoc/>
        public async Task<OktaStateManager> ReadFromSecureStorageAsync(IOktaConfig config)
        {
            return await this.ReadFromSecureStorageAsync(config, OktaContext.GetService<IOidcClient>());
        }

        /// <inheritdoc/>
        public async Task<OktaStateManager> ReadFromSecureStorageAsync(IOktaConfig config, IOidcClient client)
        {
            try
            {
                this.OnSecureStorageReadStarted(new SecureStorageEventArgs { OktaStateManager = this });
                SecureKeyValueStore secureKeyValueStore = OktaContext.GetService<SecureKeyValueStore>();
                OktaStateManager oktaStateManager = await secureKeyValueStore.GetAsync<OktaStateManager>(StoreKey);
                if (oktaStateManager != null)
                {
                    oktaStateManager.Config = config;
                    oktaStateManager.Client = client;
                }

                this.OnSecureStorageReadCompleted(new SecureStorageEventArgs { OktaStateManager = oktaStateManager });
                return oktaStateManager;
            }
            catch (Exception ex)
            {
                this.OnSecureStorageReadException(new SecureStorageExceptionEventArgs { Exception = ex });
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<OktaStateManager> ClearSecureStorageStateAsync()
        {
            this.Clear();
            await this.WriteToSecureStorageAsync();
            return this;
        }

        protected void OnSecureStorageReadStarted(SecureStorageEventArgs eventArgs)
        {
            this.SecureStorageReadStarted?.Invoke(this, eventArgs);
        }

        protected void OnSecureStorageReadCompleted(SecureStorageEventArgs eventArgs)
        {
            this.SecureStorageReadCompleted?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Raises the SecureStorageReadException event.
        /// </summary>
        /// <param name="eventArgs">The event arguments.</param>
        protected void OnSecureStorageReadException(SecureStorageExceptionEventArgs eventArgs)
        {
            this.SecureStorageReadException?.Invoke(this, eventArgs);
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

        /// <inheritdoc/>
        public string ToJson(Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }
    }
}
