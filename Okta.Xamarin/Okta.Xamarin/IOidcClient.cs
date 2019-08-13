// <copyright file="IOidcClient.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;

namespace Okta.Xamarin
{
	/// <summary>
	/// An interface defining the cross-platform surface area of the OidcClient
	/// </summary>
	public interface IOidcClient
	{
		/// <summary>
		/// The configuration for this Client.  Must be set in the constructor.
		/// </summary>
		IOktaConfig Config { get; }

		/// <summary>
		/// Complete the authorization of a valid session obtained via the <see cref="https://github.com/okta/okta-auth-dotnet">AuthN SDK</see>.
		/// </summary>
		/// <param name="sessionToken">A valid session  token obtained via the <see cref="https://github.com/okta/okta-auth-dotnet">AuthN SDK</see></param>
		/// <returns>In case of successful authorization, this Task will return a valid <see cref="StateManager"/>.  Clients are responsible for further storage and maintenance of the manager.</returns>
		Task<StateManager> AuthenticateAsync(string sessionToken);

		/// <summary>
		/// Start the authorization flow.  This is an async method and should be awaited.
		/// </summary>
		/// <returns>In case of successful authorization, this Task will return a valid <see cref="StateManager"/>.  Clients are responsible for further storage and maintenance of the manager.</returns>
		Task<StateManager> SignInWithBrowserAsync();

		/// <summary>
		/// This method will end the user's Okta session in the browser.  This is an async method and should be awaited.
		/// </summary>
		/// <param name="stateManager">The state manager associated with the login that you wish to log out</param>
		/// <returns>Task which tracks the progress of the logout</returns>
		Task SignOutOfOktaAsync(StateManager stateManager);
	}
}