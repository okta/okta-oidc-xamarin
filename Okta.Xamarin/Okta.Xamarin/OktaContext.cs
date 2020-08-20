using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
	public class OktaContext<Cl, Co> : OktaContext 
		where Cl: IOidcClient, new()
		where Co: IOktaConfig, new()
	{
		public new Cl OidcClient { get; set; }
		public new Co OktaConfig { get; set; }

		public async Task<StateManager> SignIn(Co oktaConfig = default)
		{
			return await SignIn(new Cl 
			{
				Config = oktaConfig == null ? OktaConfig : oktaConfig 
			});
		}

		public async Task<StateManager> SignIn(Cl oidcClient = default)
		{
			oidcClient = oidcClient == null ? OidcClient: oidcClient;
			return await SignIn((IOidcClient)oidcClient);
		}
	}

	public class OktaContext
	{
		static Lazy<OktaContext> _current = new Lazy<OktaContext>(() => new OktaContext());
		public static OktaContext Current
		{
			get { return _current.Value; }
			set { _current = new Lazy<OktaContext>(() => value); }
		}

		public IOidcClient OidcClient { get; set; }
		public IOktaConfig OktaConfig { get; set; }

		public StateManager StateManager { get; set; }

		public static void Init(IOidcClient oidcClient)
		{
			Current.OidcClient = oidcClient;
		}

		public EventHandler<SignInEventArgs> SignInStarted;
		public EventHandler<SignInEventArgs> SignInCompleted;

		public virtual async Task<StateManager> SignIn(IOidcClient oidcClient = null)
		{
			SignInStarted?.Invoke(this, new SignInEventArgs { StateManager = StateManager });
			oidcClient = oidcClient ?? OidcClient;
			StateManager = await oidcClient.SignInWithBrowserAsync();
			SignInCompleted?.Invoke(this, new SignInEventArgs { StateManager = StateManager });
			return StateManager;
		}
	}
}
