using Okta.Net.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Configuration
{
	public class ProfileIdentityClientConfigurationProvider : IIdentityClientConfigurationProvider
	{
		public IdentityClientConfiguration GetConfiguration()
		{
			return new ProfileIdentityClientConfiguration().Load();
		}
	}
}
