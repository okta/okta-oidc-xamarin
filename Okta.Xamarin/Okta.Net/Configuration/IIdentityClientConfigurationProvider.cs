using Okta.Net.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Configuration
{
	public interface IIdentityClientConfigurationProvider
	{
		IdentityClientConfiguration GetConfiguration();
	}
}
