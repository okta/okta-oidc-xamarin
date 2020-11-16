using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
    /// <summary>
    /// A high level container providing access to Okta functionality.
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    /// <typeparam name="TConfig"></typeparam>
    public class OktaContext<TClient, TConfig> : OktaContext
        where TClient: IOidcClient, new()
        where TConfig: IOktaConfig, new()
    {
        /// <summary>
        /// Gets or sets the default client.
        /// </summary>
        public new TClient OidcClient { get; set; }

        /// <summary>
        /// Gets or sets the default okta config.
        /// </summary>
        public new TConfig OktaConfig { get; set; }

        /// <summary>
        /// Sign in using the specified config.
        /// </summary>
        /// <param name="oktaConfig">The config to use.</param>
        /// <returns>OktaState.</returns>
        public async Task<OktaState> SignIn(TConfig oktaConfig = default)
        {
            return await this.SignIn(new TClient
            {
                Config = oktaConfig == null ? this.OktaConfig : oktaConfig,
            });
        }

        /// <summary>
        /// Sign in using the specified client.
        /// </summary>
        /// <param name="oidcClient">The client to use.</param>
        /// <returns>OktaState.</returns>
        public async Task<OktaState> SignIn(TClient oidcClient = default)
        {
            oidcClient = oidcClient == null ? OidcClient: oidcClient;
            return await SignIn((IOidcClient)oidcClient);
        }
    }

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
        /// Gets or sets the current global context.
        /// </summary>
        public static OktaContext Current
        {
            get => current.Value;
            set { current = new Lazy<OktaContext>(() => value); }
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
        public OktaState StateManager { get; set; } // TODO: rename this to OktaState

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
        public virtual async Task<OktaState> SignIn(IOidcClient oidcClient = null)
        {
            this.SignInStarted?.Invoke(this, new SignInEventArgs { StateManager = StateManager });
            oidcClient = oidcClient ?? this.OidcClient;
            this.StateManager = await oidcClient.SignInWithBrowserAsync();
            this.SignInCompleted?.Invoke(this, new SignInEventArgs { StateManager = StateManager });
            return this.StateManager;
        }

        /// <summary>
        /// Sign out using the specified client.
        /// </summary>
        /// <param name="oidcClient">The client</param>
        /// <returns>Task.</returns>
        public virtual async Task SignOut(IOidcClient oidcClient = null)
        {
            this.SignOutStarted?.Invoke(this, new SignOutEventArgs{ StateManager = this.StateManager });
            oidcClient = oidcClient ?? this.OidcClient;
            this.StateManager = await oidcClient.SignOutOfOktaAsync(this.StateManager);
            this.SignOutCompleted?.Invoke(this, new SignOutEventArgs { StateManager = this.StateManager });
        }
    }
}
