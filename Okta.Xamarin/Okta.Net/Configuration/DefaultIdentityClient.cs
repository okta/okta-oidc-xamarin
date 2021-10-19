using Okta.Net.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Net.Configuration
{
	public class DefaultIdentityClient : IdentityClient
	{
		public DefaultIdentityClient() : base(new CustomIdentityClientConfigurationProvider(() => IdentityClientConfiguration.Default))
		{ 
		}
	}
}
