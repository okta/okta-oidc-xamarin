using Okta.Net.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Okta.Net.Test.Integration
{
	public class DefaultIdentityClientShouldActually
	{
		[Fact]
		public async Task CallInteractWithoutException()
		{
			DefaultIdentityClient defaultIdentityClient = new DefaultIdentityClient();
			await defaultIdentityClient.InteractAsync();
		}
	}
}
