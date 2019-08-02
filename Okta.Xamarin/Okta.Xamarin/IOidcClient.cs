using System;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
	public interface IOidcClient
	{
		IOktaConfig Config { get; }

		Task<StateManager> AuthenticateAsync(string sessionToken);
		Task ParseRedirectedUrl(Uri url);
		Task<StateManager> SignInWithBrowserAsync();
		Task SignOutOfOktaAsync(StateManager stateManager);
	}
}