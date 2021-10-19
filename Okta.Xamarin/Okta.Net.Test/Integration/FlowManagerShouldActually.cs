using FluentAssertions;
using Okta.Net.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Okta.Net.Test.Integration
{
	public class FlowManagerShouldActually
	{
		[Fact]
		public async Task GetFormOnStart()
		{
			FlowManager flowManager = new FlowManager();

			IIdentityIntrospection form = await flowManager.StartAsync();

			form.Should().NotBeNull();
			form.HasException.Should().BeFalse();
			form.Raw.Should().NotBeNullOrWhiteSpace();


		}
	}
}
