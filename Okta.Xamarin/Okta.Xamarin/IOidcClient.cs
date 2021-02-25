// <copyright file="IOidcClient.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
    /// <summary>
    /// An interface defining the cross-platform surface area of the OidcClient
    /// </summary>
    public interface IOidcClient
    {
        /// <summary>
        /// Gets the OAuthException that occurred if any.  Will be null if no exception occurred.
        /// </summary>
        OAuthException OAuthException{ get; }

        /// <summary>
        /// The configuration for this client instance.
        /// </summary>
        IOktaConfig Config { get; set; }

        /// <summary>
        /// Complete the authorization of a valid session obtained via the <see cref="https://github.com/okta/okta-auth-dotnet">AuthN SDK</see>.
        /// </summary>
        /// <param name="sessionToken">A valid session  token obtained via the <see cref="https://github.com/okta/okta-auth-dotnet">AuthN SDK</see></param>
        /// <returns>In case of successful authorization, this Task will return a valid <see cref="OktaStateManager"/>.  Clients are responsible for further storage and maintenance of the manager.</returns>
        Task<IOktaStateManager> AuthenticateAsync(string sessionToken);

        /// <summary>
        /// Start the authorization flow.  This is an async method and should be awaited.
        /// </summary>
        /// <returns>In case of successful authorization, this Task will return a valid <see cref="OktaStateManager"/>.  Clients are responsible for further storage and maintenance of the manager.</returns>
        Task<IOktaStateManager> SignInWithBrowserAsync();

        /// <summary>
        /// This method will end the user's Okta session in the browser.  This is an async method and should be awaited.
        /// </summary>
        /// <param name="stateManager">The state manager associated with the login that you wish to log out</param>
        /// <returns>Task which tracks the progress of the logout</returns>
        Task<IOktaStateManager> SignOutOfOktaAsync(IOktaStateManager stateManager);

        /// <summary>
        /// Revokes the specified access token.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <returns>Task.</returns>
        Task RevokeAccessTokenAsync(string accessToken);

        /// <summary>
        /// Revokes the specified refresh token.
        /// </summary>
        /// <param name="accessToken">The access token used for authorization.</param>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns>Task.</returns>
        Task RevokeRefreshTokenAsync(string accessToken, string refreshToken);

        /// <summary>
        /// Gets user information.
        /// </summary>
        /// <param name="accessToken">The access token used for authorization.</param>
        /// <param name="authorizationServerId">The authorization server id or null.</param>
        /// <returns>Task{Dictionary{string, object}}</returns>
        Task<Dictionary<string, object>> GetUserAsync(string accessToken, string authorizationServerId = "default");

        /// <summary>
        /// Gets user information.
        /// </summary>
        /// <typeparam name="T">The generic type to deserialize the response as.</typeparam>
        /// <param name="accessToken">The access token used for authorization.</param>
        /// <param name="authorizationServerId">The authorization server id or null.</param>
        /// <returns>Task{T}.</returns>
        Task<T> GetUserAsync<T>(string accessToken, string authorizationServerId = "default");

        /// <summary>
        /// Gets user information as a ClaimsPrincipal instance.
        /// </summary>
        /// <typeparam name="T">The generic type to deserialize the response as.</typeparam>
        /// <param name="accessToken">The access token used for authorization.</param>
        /// <param name="authorizationServerId">The authorization server id or null.</param>
        /// <returns>Task{ClaimsPrincipal}.</returns>
        Task<ClaimsPrincipal> GetClaimsPincipalAsync(string accessToken, string authorizationServerId = "default");
    }
}
