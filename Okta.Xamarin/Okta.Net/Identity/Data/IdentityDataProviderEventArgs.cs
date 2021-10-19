using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Net.Identity.Data
{
	public class IdentityDataProviderEventArgs : EventArgs
	{
		public IIdentityDataProvider DataProvider { get; set; }
		public IIdentityIntrospection Form { get; set; }
		public Exception Exception { get; set; }
	}
}
