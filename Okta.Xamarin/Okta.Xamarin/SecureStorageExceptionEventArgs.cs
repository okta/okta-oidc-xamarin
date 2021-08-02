using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin
{
	public class SecureStorageExceptionEventArgs: EventArgs
	{
		public Exception Exception { get; set; }
	}
}
