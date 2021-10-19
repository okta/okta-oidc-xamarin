using Okta.Net.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Net
{
	public class FlowManagerEventArgs : EventArgs
	{
		public IFlowManager FlowManager { get; set; }
		public IIdentityInteraction Session { get; set; }
		public IIdentityIntrospection Form { get; set; }

		public Exception Exception { get; set; }
	}
}
