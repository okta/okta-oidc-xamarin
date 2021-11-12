using Okta.Net.Configuration;
using Okta.Net.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Okta.Net.Test.Integration
{
	public class ProfileIdentityClientConfigurationProviderShouldActually
	{
		[Fact]
		public void GetDefaultConfiguration()
		{
			ProfileIdentityClientConfigurationProvider configurationProvider = new ProfileIdentityClientConfigurationProvider();

			IdentityClientConfiguration configuration = configurationProvider.GetConfiguration();

			configuration.Equals(IdentityClientConfiguration.Default).Should().BeTrue();
			configuration.OktaDomain.Should().BeEquivalentTo(IdentityClientConfiguration.Default.OktaDomain);
			configuration.ClientId.Should().BeEquivalentTo(IdentityClientConfiguration.Default.ClientId);
			configuration.IssuerUri.Should().BeEquivalentTo(IdentityClientConfiguration.Default.IssuerUri);
			configuration.RedirectUri.Should().BeEquivalentTo(IdentityClientConfiguration.Default.RedirectUri);
		}
	}
}
