using Okta.Net.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Net.Configuration
{
	public class CustomIdentityClientConfigurationProvider : IIdentityClientConfigurationProvider
	{
		public CustomIdentityClientConfigurationProvider(Func<IdentityClientConfiguration> implementation)
		{
			this.Implementation = implementation;
		}

		public IdentityClientConfiguration GetConfiguration()
		{
			return Implementation();
		}

		protected Func<IdentityClientConfiguration> Implementation{ get; }
	}
}
