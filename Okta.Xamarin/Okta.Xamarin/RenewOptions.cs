using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin
{
	public class RenewOptions
	{
		public RenewOptions()
		{
			AuthorizationServerId = "default";
		}

		public string RefreshToken { get; set; }
		public bool RefreshIdToken { get; set; }

		public string AuthorizationServerId { get; set; }
	}
}
