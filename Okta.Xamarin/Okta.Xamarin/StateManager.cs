using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Xamarin
{
	public class StateManager
	{
		public string AccessToken { get; private set; }
		public string IdToken { get; private set; }
		public string RefreshToken { get; private set; }

		public StateManager()
		{

		}

		public StateManager(string accessToken, string idToken, string refreshToken)
		{
			this.AccessToken = accessToken;
			this.IdToken = idToken;
			this.RefreshToken = refreshToken;
		}

		public bool IsAuthenticated
		{
			get
			{
				throw new NotImplementedException();
			}

		}

		public async Task WriteToSecureStorageAsync()
		{
			throw new NotImplementedException();
		}
		public async Task RevokeAsync()
		{
			throw new NotImplementedException();
		}
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

		public static async Task<StateManager> ReadFromSecureStorageAsync()
		{
			throw new NotImplementedException();
		}
	}
}
