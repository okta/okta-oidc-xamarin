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
	public class IdentityClientShouldActually
	{
		[Fact]
		public async Task CallInteractWithoutException()
		{
			IdentityClient identityClient = new IdentityClient();
			bool? exceptionThrown = false;
			identityClient.InteractExceptionThrown += (sender, args) => exceptionThrown = true;
			identityClient.IntrospectExceptionThrown += (sender, args) => exceptionThrown = true;

			IIdentityInteraction response = await identityClient.InteractAsync();

			response.Raw.Should().NotBeNullOrEmpty();
			exceptionThrown.Should().BeFalse();
		}

		[Fact]
		public async Task PopulateResponseInteractionHandle()
		{
			IdentityClient identityClient = new IdentityClient();
	
			IIdentityInteraction response = await identityClient.InteractAsync();

			response.InteractionHandle.Should().NotBeNullOrWhiteSpace();
		}


		[Fact]
		public async Task CallIntrospectWithoutException()
		{
			IdentityClient identityClient = new IdentityClient();
			bool? exceptionThrown = false;
			identityClient.InteractExceptionThrown += (sender, args) => exceptionThrown = true;
			identityClient.IntrospectExceptionThrown += (sender, args) => exceptionThrown = true;

			IIdentityInteraction response = await identityClient.InteractAsync();
			IIdentityIntrospection form = await identityClient.IntrospectAsync(response);
			form.Raw.Should().NotBeNullOrEmpty();
			exceptionThrown.Should().BeFalse();
		}
	}
}
