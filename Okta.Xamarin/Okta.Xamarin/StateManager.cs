using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
	public class StateManager
	{
		#region InternalState/Tokens
		public object AuthState { get; set; }


		private string _accessToken;
		/// <summary>
		/// Return the known accessToken if it hasn't expired
		/// </summary>
		public string AccessToken
		{
			get
			{
				// TODO: https://github.com/okta/okta-oidc-ios/blob/566ee5fdf844ec9c3ee4c5aecafde7b25ad777fa/Okta/OktaOidc/OktaOidcStateManager.swift#L21
				return _accessToken;
			}
		}

		private string _idToken;

		/// <summary>
		/// Return the known IdToken if it is valid
		/// </summary>
		public string IdToken
		{
			get
			{
				//TODO: https://github.com/okta/okta-oidc-ios/blob/566ee5fdf844ec9c3ee4c5aecafde7b25ad777fa/Okta/OktaOidc/OktaOidcStateManager.swift#L34
				return _idToken;
			}
		}


		private string _refreshToken;

		/// <summary>
		/// A refresh token is a special token that is used to generate additional access and ID tokens. Make sure to include the `offline_access` scope in your configuration to silently renew the user's session in your application.
		/// </summary>
		public string RefreshToken { get; private set; }
		#endregion


		#region CTors
		public StateManager()
		{

		}

		public StateManager(string accessToken, string idToken, string refreshToken)
		{
			this._accessToken = accessToken;
			this._idToken = idToken;
			this._refreshToken = refreshToken;
		} 
		#endregion


		/// <summary>
		/// Stores the tokens securely in platform-specific secure storage.  This is an async method and should be awaited.
		/// </summary>
		/// <returns>Task which tracks the progress of the save</returns>
		public async Task WriteToSecureStorageAsync()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves a stored state manager for a given config.  This is an async method and should be awaited.
		/// </summary>
		/// <param name="config">the Okta configuration that corresponds to a manager you are interested in</param>
		/// <returns>If a state manager is found for the provided config, this Task will return the <see cref="StateManager"/>.</returns>
		public static async Task<StateManager> ReadFromSecureStorageAsync(IOktaConfig config)
		{
			throw new NotImplementedException();
		}


		public async Task RevokeAsync()
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// Renew expired tokens by exchanging a refresh token for new ones.  Make sure to include the `offline_access` scope in your configuration.  
		/// </summary>
		/// <returns>Returns the new access token, also accessible in <see cref="AccessToken"/></returns>
		public async Task<string> RenewAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<System.Security.Claims.ClaimsPrincipal> GetUserAsync()
		{
			throw new NotImplementedException();
		}
		public void Clear()
		{
			throw new NotImplementedException();
		}

	}
}
