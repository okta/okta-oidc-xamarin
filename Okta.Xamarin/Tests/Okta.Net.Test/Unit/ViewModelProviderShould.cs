using FluentAssertions;
using NSubstitute;
using Okta.Net.Identity;
using Okta.Net.Identity.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Okta.Net.Test.Unit
{
	public class ViewModelProviderShould
	{
		[Fact]
		public async Task FireGetViewModelStartedEvent()
		{
			IdentityViewModelProvider viewModelProvider = new IdentityViewModelProvider();
			bool? eventFired = false;
			viewModelProvider.GetViewModelStarted += (sender, args) => eventFired = true;
			await viewModelProvider.GetViewModelAsync(Substitute.For<IIdentityIntrospection>());

			eventFired.Should().BeTrue();
		}

		[Fact]
		public async Task FireGetViewModelCompletedEvent()
		{
			IdentityViewModelProvider viewModelProvider = new IdentityViewModelProvider();
			bool? eventFired = false;
			viewModelProvider.GetViewModelCompleted += (sender, args) => eventFired = true;
			await viewModelProvider.GetViewModelAsync(Substitute.For<IIdentityIntrospection>());

			eventFired.Should().BeTrue();
		}

		[Fact]
		public async Task FireGetViewModelExceptionThrownEvent()
		{
			IdentityViewModelProvider viewModelProvider = new IdentityViewModelProvider();
			bool? eventFired = false;
			viewModelProvider.GetViewModelStarted += (sender, args) => throw new Exception($"Testing that the {nameof(IdentityViewModelProvider.GetViewModelExceptionThrown)} event is raised");
			viewModelProvider.GetViewModelExceptionThrown += (sender, args) => eventFired = true;
			await viewModelProvider.GetViewModelAsync(Substitute.For<IIdentityIntrospection>());

			eventFired.Should().BeTrue();
		}
	}
}
