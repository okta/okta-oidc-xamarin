using FluentAssertions;
using NSubstitute;
using Okta.Net.Data;
using Okta.Net.Identity;
using Okta.Net.Identity.Data;
using Okta.Net.Policy;
using Okta.Net.Session;
using Okta.Net.View;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Okta.Net.Test.Unit
{
    public class FlowManagerShould
    {
       [Fact]
	   public async Task CallDataProviderOnStart()
	   {
			ServiceProvider serviceProvider = new ServiceProvider();
			serviceProvider.RegisterService(Substitute.For<IIdentityClient>());
			serviceProvider.RegisterService(Substitute.For<IPolicyProvider>());
			serviceProvider.RegisterService(Substitute.For<ISessionProvider>());
			serviceProvider.RegisterService(Substitute.For<IStorageProvider>());
			serviceProvider.RegisterService(Substitute.For<ILoggingProvider>());
			serviceProvider.RegisterService(Substitute.For<IViewProvider>());

			IdentityInteraction testSession = new IdentityInteraction() { InteractionHandle = "test interaction handle" };
			IIdentityDataProvider dataProvider = Substitute.For<IIdentityDataProvider>();
			dataProvider.StartSessionAsync().Returns(testSession);
			serviceProvider.RegisterService(dataProvider);

			FlowManager flowProvider = new FlowManager(serviceProvider);
			await flowProvider.StartAsync();

			await dataProvider.Received().StartSessionAsync();
			await dataProvider.Received().GetFormDataAsync(testSession.InteractionHandle);
	   }

	   [Fact]
	   public async Task CallSessionProviderOnStart()
	   {
			ServiceProvider serviceProvider = new ServiceProvider();
			ISessionProvider sessionProvider = Substitute.For<ISessionProvider>();
			serviceProvider.RegisterService(Substitute.For<IIdentityClient>());
			serviceProvider.RegisterService(Substitute.For<IPolicyProvider>());
			serviceProvider.RegisterService(sessionProvider);
			serviceProvider.RegisterService(Substitute.For<IStorageProvider>());
			serviceProvider.RegisterService(Substitute.For<ILoggingProvider>());
			serviceProvider.RegisterService(Substitute.For<IViewProvider>());

			string testState = "test state";
			IdentityInteraction testSession = new IdentityInteraction() { State = testState, InteractionHandle = "test interaction handle" };
			IIdentityDataProvider dataProvider = Substitute.For<IIdentityDataProvider>();
			dataProvider.StartSessionAsync().Returns(testSession);
			serviceProvider.RegisterService(dataProvider);

			FlowManager flowManager = new FlowManager(serviceProvider);
			await flowManager.StartAsync();

			sessionProvider.Received().Set(testState, testSession.ToJson());
		}

		[Fact]
		public async Task FireFlowStartingEvent()
		{
			ServiceProvider serviceProvider = new ServiceProvider();
			serviceProvider.RegisterService(Substitute.For<IIdentityClient>());
			serviceProvider.RegisterService(Substitute.For<IPolicyProvider>());
			serviceProvider.RegisterService(Substitute.For<ISessionProvider>());
			serviceProvider.RegisterService(Substitute.For<IStorageProvider>());
			serviceProvider.RegisterService(Substitute.For<ILoggingProvider>());
			serviceProvider.RegisterService(Substitute.For<IViewProvider>());

			string testState = "test state";
			IdentityInteraction testSession = new IdentityInteraction() { State = testState, InteractionHandle = "test interaction handle" };
			IIdentityDataProvider dataProvider = Substitute.For<IIdentityDataProvider>();
			dataProvider.StartSessionAsync().Returns(testSession);
			serviceProvider.RegisterService(dataProvider);

			bool? eventFired = false;
			FlowManager flowManager = new FlowManager(serviceProvider);
			flowManager.FlowStarting += (sender, args) => eventFired = true;
			await flowManager.StartAsync();

			eventFired.Should().BeTrue();
		}

		[Fact]
		public async Task FireFlowCompletedEvent()
		{
			ServiceProvider serviceProvider = new ServiceProvider();
			serviceProvider.RegisterService(Substitute.For<IIdentityClient>());
			serviceProvider.RegisterService(Substitute.For<IPolicyProvider>());
			serviceProvider.RegisterService(Substitute.For<ISessionProvider>());
			serviceProvider.RegisterService(Substitute.For<IStorageProvider>());
			serviceProvider.RegisterService(Substitute.For<ILoggingProvider>());
			serviceProvider.RegisterService(Substitute.For<IViewProvider>());

			string testState = "test state";
			IdentityInteraction testSession = new IdentityInteraction() { State = testState, InteractionHandle = "test interaction handle" };
			IIdentityDataProvider dataProvider = Substitute.For<IIdentityDataProvider>();
			dataProvider.StartSessionAsync().Returns(testSession);
			serviceProvider.RegisterService(dataProvider);

			bool? eventFired = false;
			FlowManager flowManager = new FlowManager(serviceProvider);
			flowManager.FlowStartCompleted += (sender, args) => eventFired = true;
			await flowManager.StartAsync();

			eventFired.Should().BeTrue();
		}

		[Fact]
		public async Task FireFlowStartExceptionThrownEvent()
		{
			ServiceProvider serviceProvider = new ServiceProvider();
			serviceProvider.RegisterService(Substitute.For<IIdentityClient>());
			serviceProvider.RegisterService(Substitute.For<IPolicyProvider>());
			serviceProvider.RegisterService(Substitute.For<ISessionProvider>());
			serviceProvider.RegisterService(Substitute.For<IStorageProvider>());
			serviceProvider.RegisterService(Substitute.For<ILoggingProvider>());
			serviceProvider.RegisterService(Substitute.For<IViewProvider>());

			string testState = "test state";
			IdentityInteraction testSession = new IdentityInteraction() { State = testState, InteractionHandle = "test interaction handle" };
			IIdentityDataProvider dataProvider = Substitute.For<IIdentityDataProvider>();
			dataProvider.StartSessionAsync().Returns(testSession);
			serviceProvider.RegisterService(dataProvider);

			bool? eventFired = false;
			FlowManager flowManager = new FlowManager(serviceProvider);
			flowManager.FlowStarting += (sender, args) => throw new Exception($"testing that the {nameof(FlowManager.FlowStartExceptionThrown)} event is fired");
			flowManager.FlowStartExceptionThrown += (sender, args) => eventFired = true;
			await flowManager.StartAsync();

			eventFired.Should().BeTrue();
		}
	}
}
