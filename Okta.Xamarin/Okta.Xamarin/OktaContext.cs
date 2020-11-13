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
        public new TClient OidcClient { get; set; }
        public new TConfig OktaConfig { get; set; }

        public async Task<OktaState> SignIn(TConfig oktaConfig = default)
        {
            return await SignIn(new TClient 
            {
                Config = oktaConfig == null ? OktaConfig : oktaConfig 
            });
        }

        public async Task<OktaState> SignIn(TClient oidcClient = default)
        {
            oidcClient = oidcClient == null ? OidcClient: oidcClient;
            return await SignIn((IOidcClient)oidcClient);
        }
    }

    public class OktaContext
    {
        public event EventHandler<SignInEventArgs> SignInStarted;

        public event EventHandler<SignInEventArgs> SignInCompleted;

        public event EventHandler<SignOutEventArgs> SignOutStarted;

        public event EventHandler<SignOutEventArgs> SignOutCompleted;

        private static Lazy<OktaContext> current = new Lazy<OktaContext>(() => new OktaContext());

        public static OktaContext Current
        {
            get => current.Value;
            set { current = new Lazy<OktaContext>(() => value); }
        }

        public IOidcClient OidcClient { get; set; }

        public IOktaConfig OktaConfig { get; set; }

        public OktaState StateManager { get; set; }

		/// <summary>
		/// Convenience method to add a listener to the OktaContext.Current.SignInStarted event.
		/// </summary>
		/// <param name="signInStartedEventHandler"></param>
		public static void AddSignInStartedListener(EventHandler<SignInEventArgs> signInStartedEventHandler)
        {
            Current.SignInStarted += signInStartedEventHandler;
		}

		/// <summary>
		/// Convenience method to add a listener to the OktaContext.Current.SignInCompleted event.
		/// </summary>
		/// <param name="signInCompletedEventHandler"></param>
		public static void AddSignInCompletedListener(EventHandler<SignInEventArgs> signInCompletedEventHandler)
		{
			Current.SignInCompleted += signInCompletedEventHandler;
		}

		public static void AddSignOutStartedListener(EventHandler<SignOutEventArgs> signOutStartedEventHandler)
		{
			Current.SignOutStarted += signOutStartedEventHandler;
		}

		public static void AddSignOutCompletedListener(EventHandler<SignOutEventArgs> signOutCompletedEventHandler)
		{
			Current.SignOutCompleted += signOutCompletedEventHandler;
		}

        public static void Init(IOidcClient oidcClient)
        {
            Current.OidcClient = oidcClient;
        }

        public virtual async Task<OktaState> SignIn(IOidcClient oidcClient = null)
        {
            this.SignInStarted?.Invoke(this, new SignInEventArgs { StateManager = StateManager });
            oidcClient = oidcClient ?? this.OidcClient;
            this.StateManager = await oidcClient.SignInWithBrowserAsync();
            this.SignInCompleted?.Invoke(this, new SignInEventArgs { StateManager = StateManager });
            return this.StateManager;
        }

        public virtual async Task SignOut(IOidcClient oidcClient = null)
        {
            this.SignOutStarted?.Invoke(this, new SignOutEventArgs{ StateManager = this.StateManager });
            oidcClient = oidcClient ?? this.OidcClient;
            this.StateManager = await oidcClient.SignOutOfOktaAsync(this.StateManager);
            this.SignOutCompleted?.Invoke(this, new SignOutEventArgs { StateManager = this.StateManager });
        }
    }
}
