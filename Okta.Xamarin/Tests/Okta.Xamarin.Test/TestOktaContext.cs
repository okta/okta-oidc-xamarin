using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Xamarin.Test
{
	public class TestOktaContext: OktaContext
	{
		public void RaiseLoadStateStartedEvent()
		{
			this.OnLoadStateStarted();
		}

		public void RaiseLoadCompletedEvent(OktaStateManager oktaStateManager)
		{
			this.OnLoadStateCompleted(oktaStateManager);
		}

		public void RaiseLoadStateExceptionEvent(Exception ex)
		{
			this.OnLoadStateException(ex);
		}

		public void RaiseSecureStorageWriteStartedEvent()
		{
			this.OnSecureStorageWriteStarted(this, new SecureStorageEventArgs());
		}

		public void RaiseSecureStorageWriteCompletedEvent()
		{
			this.OnSecureStorageWriteCompleted(this, new SecureStorageEventArgs());
		}
		
		public void RaiseSecureStorageWriteExceptionEvent(Exception ex)
		{
			this.OnSecureStorageWriteException(this, new SecureStorageExceptionEventArgs() { Exception = ex });
		}

		public void RaiseSecureStorageReadStartedEvent()
		{
			this.OnSecureStorageReadStarted(this, new SecureStorageEventArgs());
		}

		public void RaiseSecureStorageReadCompletedEvent()
		{
			this.OnSecureStorageReadCompleted(this, new SecureStorageEventArgs());
		}

		public void RaiseSecureStorageReadExceptionEvent(Exception ex)
		{
			this.OnSecureStorageReadException(this, new SecureStorageExceptionEventArgs() { Exception = ex });
		}

		public void RaiseRequestExceptionEvent(Exception ex)
		{
			this.OnRequestException(this, new RequestExceptionEventArgs(ex, HttpMethod.Get, "test path", new Dictionary<string, string>(), "test auth server id", new KeyValuePair<string, string>[] { }));
		}

		public void RaiseSignInStartedEvent()
		{
			this.OnSignInStarted();
		}

		public void RaiseSignInCompletedEvent()
		{
			this.OnSignInCompleted();
		}

		public void RaiseSignOutStartedEvent()
		{
			this.OnSignOutStarted();
		}

		public void RaiseSignOutCompletedEvent()
		{
			this.OnSignOutCompleted();
		}

		public void RaiseAuthenticationFailedEvent(OAuthException ex)
		{
			this.OnAuthenticationFailed(ex);
		}

		public void RaiseRevokeStartedEvent(TokenKind tokenKind, string token)
		{
			this.OnRevokeStarted(tokenKind, token);
		}

		public void RaiseRevokeCompletedEvent(TokenKind tokenKind)
		{
			this.OnRevokeCompleted(tokenKind);
		}

		public void RaiseRevokeExceptionEvent(TokenKind tokenKind, Exception ex)
		{
			this.OnRevokeException(tokenKind, ex);
		}

		public void RaiseGetUserStartedEvent()
		{
			this.OnGetUserStarted();
		}

		public void RaiseGetUserCompletedEvent(object userInfo)
		{
			this.OnGetUserCompleted(userInfo);
		}

		public void RaiseIntrospectStartedEvent(TokenKind tokenKind, string token)
		{
			this.OnIntrospectStarted(tokenKind, token);
		}

		public void RaiseIntrospectCompletedEvent(TokenKind tokenKind, string token, Dictionary<string, object> result)
		{
			this.OnIntrospectCompleted(tokenKind, token, result);
		}

		public void RaiseRenewStartedEvent(string refreshToken, bool refreshIdToken, string authorizationServerId)
		{
			this.OnRenewStarted(refreshToken, refreshIdToken, authorizationServerId);
		}

		public void RaiseRenewCompletedEvent(string refreshToken, bool refreshIdToken, string authorizationServerId, RenewResponse response)
		{
			this.OnRenewCompleted(refreshToken, refreshIdToken, authorizationServerId, response);
		}

		public void RaiseRenewException(Exception ex)
		{
			this.OnRenewException(ex);
		}
	}
}
