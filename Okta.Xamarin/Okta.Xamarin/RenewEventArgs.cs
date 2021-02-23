using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin
{
	public class RenewEventArgs : EventArgs
	{
		public IOktaStateManager StateManager{ get; set; }

		public bool RefreshIdToken { get; set; }
		public string AuthorizationServerId { get; set; }

		public RenewResponse Response { get; set; }
	}
}
