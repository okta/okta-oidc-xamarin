using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin
{
	public class AuthCodeTokenExchangeFailedEventArgs : EventArgs
	{
		public IOidcClient OidcClient { get; set; }
		public Exception Exception { get; set; }
	}
}
