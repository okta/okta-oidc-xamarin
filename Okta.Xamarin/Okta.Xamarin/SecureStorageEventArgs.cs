using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin
{
	public class SecureStorageEventArgs : EventArgs
	{
		public IOktaStateManager OktaStateManager { get; set; }
	}
}
