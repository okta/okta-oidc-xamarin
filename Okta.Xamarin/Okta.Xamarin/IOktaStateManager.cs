// <copyright file="IOktaStateManager.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Okta.Xamarin
{
    /// <summary>
    /// State manager interface.
    /// </summary>
    public interface IOktaStateManager
    {
        /// <summary>
        /// The event that is raised before writing to secure storage.
        /// </summary>
        event EventHandler<SecureStorageEventArgs> SecureStorageWriteStarted;

        /// <summary>
        /// The event that is raised when writing to secure storage completes.
        /// </summary>
        event EventHandler<SecureStorageEventArgs> SecureStorageWriteCompleted;

        /// <summary>
        /// The event that is raised when an exception occurs writing to secure storage.
        /// </summary>
        event EventHandler<SecureStorageExceptionEventArgs> SecureStorageWriteException;

        /// <summary>
        /// The event that is raised before reading from secure storage.
        /// </summary>
        event EventHandler<SecureStorageEventArgs> SecureStorageReadStarted;

        /// <summary>
        /// The event that is raised when reading from secure storage completes.
        /// </summary>
        event EventHandler<SecureStorageEventArgs> SecureStorageReadCompleted;

        /// <summary>
        /// The event that is raised when an exception occurs reading from secure storage.
        /// </summary>
        event EventHandler<SecureStorageExceptionEventArgs> SecureStorageReadException;

        /// <summary>
        /// The event that is raised when an API exception occurs.
        /// </summary>
        event EventHandler<RequestExceptionEventArgs> RequestException;

        /// <summary>
        /// Gets the access token.
        /// </summary>
        string AccessToken { get; }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        IOktaConfig Config { get; set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        IOidcClient Client { get; set; }

        /// <summary>
        /// Gets the expiration time.
        /// </summary>
        DateTimeOffset? Expires { get; }

        /// <summary>
        /// Gets the id token.
        /// </summary>
        string IdToken { get; }

        /// <summary>
        /// Gets a value indicating whether or not there is a current non-expired <see cref="AccessToken"/>, which means that the user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Gets the last response received from the API. Primarily for debugging.
        /// </summary>
        HttpResponseMessage LastApiResponse { get; }

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        string RefreshToken { get; }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        string Scope { get; }

        /// <summary>
        /// Gets the type of the AccessToken, for example "Bearer".
        /// </summary>
        string TokenType { get; }

        /// <summary>
        /// Removes the local authentication state. A full sign out should consist of calling <see cref="OidcClient.SignOutOfOktaAsync"/>, then <see cref="RevokeAsync"/>, and then <see cref="Clear"/>.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the token of the specified kind.
        /// </summary>
        /// <param name="tokenKind">The token kind.</param>
        /// <returns>string.</returns>
        string GetToken(TokenKind tokenKind);

        /// <summary>
        /// Gets an instance of the generic type T representing the current user.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response as.</typeparam>
        /// <param name="authorizationServerId">The authorization server ID.</param>
        /// <returns>T.</returns>
        Task<T> GetUserAsync<T>();

        /// <summary>
        /// Gets information about the current user.
        /// </summary>
        /// <param name="authorizationServerId">The authorization server ID.</param>
        /// <returns>Dictionary{string, object}.</returns>
        Task<Dictionary<string, object>> GetUserAsync();

        /// <summary>
        /// Gets information about the current user as a ClaimsPrincipal.
        /// </summary>
        /// <param name="authorizationServerId">The authorization server ID.</param>
        /// <returns>ClaimsPrincipal.</returns>
        Task<ClaimsPrincipal> GetClaimsPrincipalAsync();

        /// <summary>
        /// Gets information about the state of the specified token.
        /// </summary>
        /// <param name="tokenKind">The kind of token.</param>
        /// <returns>Dictoinary{string, object}.</returns>
        Task<Dictionary<string, object>> IntrospectAsync(TokenKind tokenKind);

        /// <summary>
        /// Renews tokens.
        /// </summary>
        /// <param name="refreshIdToken">A value indicating whether to also renew the ID token.</param>
        /// <returns>Task{RenewResponse}.</returns>
        Task<RenewResponse> RenewAsync(bool refreshIdToken = false);

        /// <summary>
        /// Renews tokens.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="refreshIdToken">A value indicating whether to refresh the ID token.</param>
        /// <returns>Task{RenewResponse}.</returns>
        Task<RenewResponse> RenewAsync(string refreshToken, bool refreshIdToken = false);

        /// <summary>
        /// Revokes tokens associated with this OktaState.
        /// </summary>
        /// <param name="tokenKind">The kind of token to revoke.</param>
        /// <returns>Task.</returns>
        Task RevokeAsync(TokenKind tokenKind);

        /// <summary>
        /// Revoke the specified token.
        /// </summary>
        /// <param name="tokenKind">The kind of the token.</param>
        /// <param name="token">The token.</param>
        /// <returns>Task.</returns>
        Task RevokeAsync(TokenKind tokenKind, string token);

        /// <summary>
        /// Revoke the specified access token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Task.</returns>
        Task RevokeAccessTokenAsync(string token);

        /// <summary>
        /// Revoke the specified refresh token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Task.</returns>
        Task RevokeRefreshTokenAsync(string token);

        /// <summary>
        /// Stores the tokens securely in platform-specific secure storage.
        /// Subscribe to SecureStorageWriteException event for exception details
        /// if an exception occurs, see event <see cref="SecureStorageWriteException"/>.
        /// </summary>
        /// <returns>Task which tracks the progress of the save.</returns>
        Task WriteToSecureStorageAsync();

        /// <summary>
        /// Retrieves a stored state manager from secure storage if it exists.
        /// Returns null if a state manager is not found in secure storage or
        /// if an exception occurs.  In case of exception, subscribe to the
        /// SecureStorageReadException event for exception details,
        /// see event <see cref="SecureStorageReadException"/>.
        /// </summary>
        /// <returns><see cref="OktaStateManager"/>.</returns>
        Task<OktaStateManager> ReadFromSecureStorageAsync();

        /// <summary>
        /// Retrieves a stored state manager from secure storage if it exists.
        /// Returns null if a state manager is not found in secure storage or
        /// if an exception occurs.  In case of exception, subscribe to the
        /// SecureStorageReadException event for exception details,
        /// see event <see cref="SecureStorageReadException"/>.
        /// </summary>
        /// <param name="config">The IOktaConfig implementation to apply to the resulting state manager.</param>
        /// <returns><see cref="OktaStateManager"/>.</returns>
        Task<OktaStateManager> ReadFromSecureStorageAsync(IOktaConfig config);

        /// <summary>
        /// Retrieves a stored state manager from secure storage if it exists.
        /// Returns null if a state manager is not found in secure storage or
        /// if an exception occurs.  In case of exception, subscribe to the
        /// SecureStorageReadException event for exception details,
        /// see event <see cref="SecureStorageReadException"/>.
        /// </summary>
        /// <param name="config">The IOktaConfig implementation to apply to the resulting state manager.</param>
        /// <param name="client">The IOidcClient implementation to apply to the resulting state manager.</param>
        /// <returns><see cref="OktaStateManager"/>.</returns>
        Task<OktaStateManager> ReadFromSecureStorageAsync(IOktaConfig config, IOidcClient client);

        /// <summary>
        /// Calls Clear() then WriteToSecureStorageAsync, effectively writing a stateless state manager to secure storage.
        /// </summary>
        /// <returns>The current instance.</returns>
        Task<OktaStateManager> ClearSecureStorageStateAsync();

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <returns>The access token.</returns>
        string GetAccessToken();

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        /// <returns>The refresh token.</returns>
        string GetRefreshToken();

        /// <summary>
        /// Gets the id token.
        /// </summary>
        /// <returns>The id token.</returns>
        string GetIdToken();

        /// <summary>
        /// Convert the current state manager instance to the json equivalent.
        /// </summary>
        /// <param name="formatting">The formatting.</param>
        /// <returns>json string.</returns>
        string ToJson(Formatting formatting = Formatting.Indented);
    }
}
