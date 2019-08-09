// <copyright file="OktaMiddlewareExtensions.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
	/// <summary>
	/// Tracks the current login state, including any access tokens, refresh tokens, scope, etc
	/// </summary>
	public class StateManager
	{

		public string TokenType { get; private set; }
		public string AccessToken { get; private set; }
		public string IdToken { get; private set; }
		public string RefreshToken { get; private set; }
		public string Scope { get; private set; }
		public DateTime Expires { get; private set; }

		#region CTors
		public StateManager()
		{

		}

		public StateManager(string accessToken, string tokenType, string idToken = null, string refreshToken = null, int? expiresIn = null, string scope = null)
		{
			this.TokenType = tokenType;
			this.AccessToken = accessToken;
			this.IdToken = idToken;
			this.RefreshToken = refreshToken;
			this.Expires = expiresIn.HasValue ? DateTime.UtcNow.AddSeconds(expiresIn.Value) : DateTime.MaxValue;
			this.Scope = scope;
		}
		#endregion

		/// <summary>
		/// Whether or not there is a current non-expired <see cref="AccessToken"/>, indicating the user is currently successfully authenticated
		/// </summary>
		public bool IsAuthenticated
		{
			get { return (!string.IsNullOrEmpty(AccessToken) && Expires < DateTime.UtcNow); }
		}


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
