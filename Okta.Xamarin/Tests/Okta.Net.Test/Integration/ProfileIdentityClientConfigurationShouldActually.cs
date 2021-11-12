using FluentAssertions;
using Okta.Net.Configuration;
using Okta.Net.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Okta.Net.Test.Integration
{
	public class ProfileIdentityClientConfigurationShouldActually
	{
		[Fact]
		public void HaveFile()
		{
			string testOktaDomain = "test okta domain";
			string testClientId = "test client id";

			ProfileIdentityClientConfiguration configuration = new ProfileIdentityClientConfiguration().Load();
			if (configuration.File.Exists)
			{
				configuration.File.Delete();
			}

			configuration.File.Exists.Should().BeFalse();

			configuration.OktaDomain = testOktaDomain;
			configuration.ClientId = testClientId;

			configuration.Save();
			configuration.File.Exists.Should().BeTrue();

			ProfileIdentityClientConfiguration reloaded = new ProfileIdentityClientConfiguration().Load();
			reloaded.OktaDomain.Should().BeEquivalentTo(testOktaDomain);
			reloaded.ClientId.Should().BeEquivalentTo(testClientId);

			configuration.File.Delete();
		}

		[Fact]
		public void  EqualDefault()
		{
			ProfileIdentityClientConfiguration configuration = new ProfileIdentityClientConfiguration().Load();
			if (configuration.File.Exists)
			{
				configuration.File.Delete();
				configuration = new ProfileIdentityClientConfiguration().Load();
			}

			configuration.Equals(IdentityClientConfiguration.Default).Should().BeTrue();
			configuration.OktaDomain.Should().BeEquivalentTo(IdentityClientConfiguration.Default.OktaDomain);
			configuration.ClientId.Should().BeEquivalentTo(IdentityClientConfiguration.Default.ClientId);
			configuration.IssuerUri.Should().BeEquivalentTo(IdentityClientConfiguration.Default.IssuerUri);
			configuration.RedirectUri.Should().BeEquivalentTo(IdentityClientConfiguration.Default.RedirectUri);
		}
	}
}
