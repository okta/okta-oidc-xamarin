using System;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
	public interface IOidcClient
	{
		IOktaConfig Config { get; }

		Task<StateManager> AuthenticateAsync(string sessionToken);
		void ParseRedirectedUrl(Uri url);
		Task<StateManager> SignInWithBrowserAsync();
		Task SignOutOfOktaAsync(StateManager stateManager);
	}
}