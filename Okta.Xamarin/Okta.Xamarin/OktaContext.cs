// <copyright file="OktaContext.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
    /// <summary>
    /// A high level container providing access to Okta functionality.
    /// </summary>
    public class OktaContext
    {
        private static Lazy<OktaContext> current = new Lazy<OktaContext>(() => new OktaContext());

        /// <summary>
        /// The event that is raised when sign in is started.
        /// </summary>
        public event EventHandler<SignInEventArgs> SignInStarted;

        /// <summary>
        /// The event that is raised when sign in completes.
        /// </summary>
        public event EventHandler<SignInEventArgs> SignInCompleted;

        /// <summary>
        /// The event that is raised when sign out is started.
        /// </summary>
        public event EventHandler<SignOutEventArgs> SignOutStarted;

        /// <summary>
        /// The event that is raised when sign out completes.
        /// </summary>
        public event EventHandler<SignOutEventArgs> SignOutCompleted;

        /// <summary>
        /// The event that is raised when authentication fails.
        /// </summary>
        public event EventHandler<AuthenticationFailedEventArgs> AuthenticationFailed;

        /// <summary>
        /// The event that is raised when revoking a token is started.
        /// </summary>
        public event EventHandler<RevokeEventArgs> RevokeStarted;

        /// <summary>
        /// The event that is raised when a token revocation completes.
        /// </summary>
        public event EventHandler<RevokeEventArgs> RevokeCompleted;

        /// <summary>
        /// The event that is raised when getting user.
        /// </summary>
        public event EventHandler<GetUserEventArgs> GetUserStarted;

        /// <summary>
        /// The event that is raised when getting user is complete.
        /// </summary>
        public event EventHandler<GetUserEventArgs> GetUserCompleted;

        /// <summary>
        /// The event that is raised when introspection starts.
        /// </summary>
        public event EventHandler<IntrospectEventArgs> IntrospectStarted;

        /// <summary>
        /// The event that is raised when introspection completes.
        /// </summary>
        public event EventHandler<IntrospectEventArgs> IntrospectCompleted;

        /// <summary>
        /// The event that is raised when token renewal (refresh) is started.
        /// </summary>
        public event EventHandler<RenewEventArgs> RenewStarted;

        /// <summary>
        /// The event that is raised when token renewal (refresh) completes.
        /// </summary>
        public event EventHandler<RenewEventArgs> RenewCompleted;

        /// <summary>
        /// Gets or sets the current global context.
        /// </summary>
        public static OktaContext Current
        {
            get => current.Value;
            set { current = new Lazy<OktaContext>(() => value); }
        }

        OAuthException oAuthException;

        public OAuthException OAuthException
        {
            get
            {
                return oAuthException;
            }

            set
            {
                this.oAuthException = value;
                this.AuthenticationFailed?.Invoke(this, new AuthenticationFailedEventArgs(value));
            }
        }

        /// <summary>
        /// Gets or sets the default client.
        /// </summary>
        public IOidcClient OidcClient { get; set; }

        /// <summary>
        /// Gets or sets the default config.
        /// </summary>
        public IOktaConfig OktaConfig { get; set; }

        /// <summary>
        /// Gets or sets the default state.
        /// </summary>
        public IOktaStateManager StateManager { get; set; }

        /// <summary>
        /// Convenience method to add a listener to the OktaContext.Current.SignInStarted event.
        /// </summary>
        /// <param name="signInStartedEventHandler">The event handler.</param>
        public static void AddSignInStartedListener(EventHandler<SignInEventArgs> signInStartedEventHandler)
        {
            Current.SignInStarted += signInStartedEventHandler;
        }

        /// <summary>
        /// Convenience method to add a listener to the OktaContext.Current.SignInCompleted event.
        /// </summary>
        /// <param name="signInCompletedEventHandler">The event handler.</param>
        public static void AddSignInCompletedListener(EventHandler<SignInEventArgs> signInCompletedEventHandler)
        {
            Current.SignInCompleted += signInCompletedEventHandler;
        }

        /// <summary>
        /// Convenience method to add a listener to the OktaContext.Current.AuthenticationFailed event.
        /// </summary>
        /// <param name="authenticationFailedEventHandler">The event handler.</param>
        public static void AddAuthenticationFailedListener(EventHandler<AuthenticationFailedEventArgs> authenticationFailedEventHandler)
        {
            Current.AuthenticationFailed += authenticationFailedEventHandler;
        }

        /// <summary>
        /// Convenience method to add a listener to the OktaContext.Current.SignOutStarted event.
        /// </summary>
        /// <param name="signOutStartedEventHandler">The event handler.</param>
        public static void AddSignOutStartedListener(EventHandler<SignOutEventArgs> signOutStartedEventHandler)
        {
            Current.SignOutStarted += signOutStartedEventHandler;
        }

        /// <summary>
        /// Convenience method to add a listener to the OktaContext.Current.SignOutCompleted event.
        /// </summary>
        /// <param name="signOutCompletedEventHandler">The event handler.</param>
        public static void AddSignOutCompletedListener(EventHandler<SignOutEventArgs> signOutCompletedEventHandler)
        {
            Current.SignOutCompleted += signOutCompletedEventHandler;
        }

        /// <summary>
        /// Convenience method to add a listener to the OktaContext.Current.RevokedToken event.
        /// </summary>
        /// <param name="revokeStartedEventHandler">The event handler.</param>
        public static void AddRevokeStartedListener(EventHandler<RevokeEventArgs> revokeStartedEventHandler)
        {
            Current.RevokeStarted += revokeStartedEventHandler;
        }

        /// <summary>
        /// Convenience method to add a listener to the OktaContext.Current.RevokeCompleted event.
        /// </summary>
        /// <param name="revokeCompletedEventHandler">The event handler.</param>
        public static void AddRevokeCompletedListener(EventHandler<RevokeEventArgs> revokeCompletedEventHandler)
        {
            Current.RevokeCompleted += revokeCompletedEventHandler;
        }

        /// <summary>
        /// Convenience method to add a listener to the OktaContext.Current.GetUserStarted event.
        /// </summary>
        /// <param name="getUserEventHandler">The event handler.</param>
        public static void AddGetUserStartedListener(EventHandler<GetUserEventArgs> getUserEventHandler)
        {
            Current.GetUserStarted += getUserEventHandler;
        }

        /// <summary>
        /// Convenience method to add a listener to the OktaContext.Current.GetUsercompleted event.
        /// </summary>
        /// <param name="getUserEventHandler">The event handler.</param>
        public static void AddGetUserCompletedListener(EventHandler<GetUserEventArgs> getUserEventHandler)
        {
            Current.GetUserCompleted += getUserEventHandler;
        }

        /// <summary>
        /// Convenience method to add a listener to the OktaContext.Current.IntrospectStarted event.
        /// </summary>
        /// <param name="introspectEventHandler">The event handler.</param>
        public static void AddIntrospectStartedListener(EventHandler<IntrospectEventArgs> introspectEventHandler)
        {
            Current.IntrospectStarted += introspectEventHandler;
        }

        /// <summary>
        /// Convenience method to add a listener to the OktaContenxt.Current.InstrospectCompleted event.
        /// </summary>
        /// <param name="introspectEventHandler">The event handler.</param>
        public static void AddIntrospectCompletedListener(EventHandler<IntrospectEventArgs> introspectEventHandler)
        {
            Current.IntrospectCompleted += introspectEventHandler;
        }

        /// <summary>
        /// Convenience method to add a listener to the OktaContext.Current.RenewStarted event.
        /// </summary>
        /// <param name="renewEventHandler">The event handler.</param>
        public static void AddRenewStartedListener(EventHandler<RenewEventArgs> renewEventHandler)
        {
            Current.RenewStarted += renewEventHandler;
        }

        /// <summary>
        /// Convenience method to add a listener to the OktaContext.Current.RenewCompleted event.
        /// </summary>
        /// <param name="renewEventHandler">The event handler.</param>
        public static void AddRenewCompletedListener(EventHandler<RenewEventArgs> renewEventHandler)
        {
            Current.RenewCompleted += renewEventHandler;
        }

        /// <summary>
        /// Initialize OktaContext.Current with the specified default client.
        /// </summary>
        /// <param name="oidcClient">The client.</param>
        public static void Init(IOidcClient oidcClient)
        {
            Current.OidcClient = oidcClient;
        }

        /// <summary>
        /// Sign in using the specified client.
        /// </summary>
        /// <param name="oidcClient">The client.</param>
        /// <returns>OktaState.</returns>
        public virtual async Task<IOktaStateManager> SignInAsync(IOidcClient oidcClient = null)
        {
            this.SignInStarted?.Invoke(this, new SignInEventArgs { StateManager = this.StateManager });
            oidcClient = oidcClient ?? this.OidcClient;
            this.OktaConfig = oidcClient.Config;
            try
            {
                this.StateManager = await oidcClient.SignInWithBrowserAsync();
                if (oidcClient.OAuthException != null)
                {
                    this.OAuthException = oidcClient.OAuthException;
                }
            }
            catch (OAuthException ex)
            {
                this.OAuthException = ex;
            }

            this.SignInCompleted?.Invoke(this, new SignInEventArgs { StateManager = this.StateManager });
            return this.StateManager;
        }

        /// <summary>
        /// Sign out using the specified client.
        /// </summary>
        /// <param name="oidcClient">The client.</param>
        /// <returns>Task.</returns>
        public virtual async Task SignOutAsync(IOidcClient oidcClient = null)
        {
            this.SignOutStarted?.Invoke(this, new SignOutEventArgs { StateManager = this.StateManager });
            oidcClient = oidcClient ?? this.OidcClient;
            this.StateManager = await oidcClient.SignOutOfOktaAsync(this.StateManager);
            this.SignOutCompleted?.Invoke(this, new SignOutEventArgs { StateManager = this.StateManager });
        }

        /// <summary>
        /// Revoke token of the specified kind.
        /// </summary>
        /// <param name="tokenKind">The type of token to revoke.</param>
        /// <returns>Task.</returns>
        public virtual async Task RevokeAsync(TokenKind tokenKind)
        {
            string token = this.StateManager.GetToken(tokenKind);
            this.RevokeStarted?.Invoke(this, new RevokeEventArgs { StateManager = this.StateManager, TokenKind = tokenKind, Token = token });

            await this.StateManager.RevokeAsync(tokenKind);

            this.RevokeCompleted?.Invoke(this, new RevokeEventArgs { StateManager = this.StateManager, TokenKind = tokenKind });
        }

        /// <summary>
        /// Gets information about the specified kind of token.
        /// </summary>
        /// <param name="tokenKind">The kind of token to introspect.</param>
        /// <returns>Dictionary{string, object}.</returns>
        public virtual async Task<Dictionary<string, object>> IntrospectAsync(TokenKind tokenKind)
        {
            string token = this.StateManager.GetToken(tokenKind);
            this.IntrospectStarted?.Invoke(this, new IntrospectEventArgs { StateManager = this.StateManager, Token = token, TokenKind = tokenKind });
            Dictionary<string, object> result = await this.StateManager.IntrospectAsync(tokenKind);
            this.IntrospectCompleted?.Invoke(this, new IntrospectEventArgs { StateManager = this.StateManager, Token = token, TokenKind = tokenKind, Response = result });
            return result;
        }

        /// <summary>
        /// Renew tokens.
        /// </summary>
        /// <param name="refreshIdToken">A value indicating whether to renew the ID token, the default is false.</param>
        /// <param name="authorizationServerId">The authorization server id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public virtual async Task<RenewResponse> RenewAsync(bool refreshIdToken = false, string authorizationServerId = "default")
        {
            this.RenewStarted?.Invoke(this, new RenewEventArgs { StateManager = this.StateManager, RefreshIdToken = refreshIdToken, AuthorizationServerId = authorizationServerId });
            RenewResponse result = await this.StateManager.RenewAsync(refreshIdToken, authorizationServerId);
            this.RenewCompleted?.Invoke(this, new RenewEventArgs { StateManager = this.StateManager, Response = result, RefreshIdToken = refreshIdToken, AuthorizationServerId = authorizationServerId });

            return result;
        }

        /// <summary>
        /// Gets an instance of the generic type T representing the current user.
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public virtual async Task<T> GetUserAsync<T>()
        {
            this.GetUserStarted?.Invoke(this, new GetUserEventArgs { StateManager = this.StateManager });
            T user = await this.StateManager.GetUserAsync<T>();
            this.GetUserCompleted?.Invoke(this, new GetUserEventArgs { StateManager = this.StateManager, UserInfo = user });
            return user;
        }

        /// <summary>
        /// Gets information about the current user.
        /// </summary>
        public virtual async Task<Dictionary<string, object>> GetUserAsync()
        {
            this.GetUserStarted?.Invoke(this, new GetUserEventArgs { StateManager = this.StateManager });
            Dictionary<string, object> userInfo = await this.StateManager.GetUserAsync();
            this.GetUserCompleted?.Invoke(this, new GetUserEventArgs { StateManager = this.StateManager, UserInfo = userInfo });
            return userInfo;
        }

        /// <summary>
        /// Gets a ClaimsPrincipal representing the current user.
        /// </summary>
        /// <returns>ClaimsPrincipal.</returns>
        public virtual async Task<System.Security.Claims.ClaimsPrincipal> GetClaimsPrincipalAsync()
        {
            this.GetUserStarted?.Invoke(this, new GetUserEventArgs());
            ClaimsPrincipal claimsPrincipal = await this.StateManager.GetClaimsPrincipalAsync();
            this.GetUserCompleted?.Invoke(this, new GetUserEventArgs { StateManager = this.StateManager, UserInfo = claimsPrincipal });
            return claimsPrincipal;
        }

        /// <summary>
        /// Clears the local authentication state by removing tokens from the state manager.
        /// </summary>
        public void Clear()
        {
            this.StateManager.Clear();
        }

        /// <summary>
        /// Gets the token of the specified kind from the state manager.
        /// </summary>
        /// <param name="tokenKind">The kind of token to get.</param>
        /// <returns>token as string.</returns>
        public string GetToken(TokenKind tokenKind)
        {
            return this.StateManager.GetToken(tokenKind);
        }
    }
}
