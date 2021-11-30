using FluentAssertions;
using NSubstitute;
using Okta.Xamarin.Oie;
using Okta.Xamarin.Oie.Configuration;
using Okta.Xamarin.Oie.Data;
using Okta.Xamarin.Oie.Client;
using Okta.Xamarin.Oie.Client.Data;
using Okta.Xamarin.Oie.Client.View;
using Okta.Xamarin.Oie.Logging;
using Okta.Xamarin.Oie.Session;
using Okta.Xamarin.Oie.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Okta.Xamarin.Oie.Test.Unit
{
    public class DataProviderShould
    {
        [Fact]
        public async Task CallIdentityClientInteract()
        {
            ServiceProvider serviceProvider = new ServiceProvider();
            IIdentityClient identityClient = Substitute.For<IIdentityClient>();
            IIdentityInteraction session = Substitute.For<IIdentityInteraction>();
            identityClient.InteractAsync(Arg.Any<IIdentityInteraction>()).Returns(Task.FromResult(session));
            serviceProvider.RegisterService(identityClient);
            // prevent unresolvable services
            serviceProvider.RegisterService(Substitute.For<IIdentityClientConfigurationProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityDataProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityViewModelProvider>());
            serviceProvider.RegisterService(Substitute.For<ISessionProvider>());
            serviceProvider.RegisterService(Substitute.For<IStorageProvider>());
            serviceProvider.RegisterService(Substitute.For<ILoggingProvider>());
            serviceProvider.RegisterService(Substitute.For<IViewProvider>());
            // --

            IdentityDataProvider identityDataProvider = new IdentityDataProvider(serviceProvider);
            await identityDataProvider.StartSessionAsync();

            await identityClient.Received().InteractAsync();
        }

        [Fact]
        public async Task CallIdentityClientIntrospect()
        {
            string testInteractionHandle = "test interaction handle";
            ServiceProvider serviceProvider = new ServiceProvider();
            IIdentityClient identityClient = Substitute.For<IIdentityClient>();
            serviceProvider.RegisterService(identityClient);
            // prevent unresolvable services
            serviceProvider.RegisterService(Substitute.For<IIdentityClientConfigurationProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityDataProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityViewModelProvider>());
            serviceProvider.RegisterService(Substitute.For<IStorageProvider>());
            serviceProvider.RegisterService(Substitute.For<ILoggingProvider>());
            serviceProvider.RegisterService(Substitute.For<IViewProvider>());
            // --

            IdentityDataProvider identityDataProvider = new IdentityDataProvider(serviceProvider);
            await identityDataProvider.GetFormDataAsync(testInteractionHandle);

            await identityClient.Received().IntrospectAsync(testInteractionHandle);
        }

        [Fact]
        public async Task FireSessionStartingEvent()
        {
            ServiceProvider serviceProvider = new ServiceProvider();
            IIdentityClient identityClient = Substitute.For<IIdentityClient>();
            IIdentityInteraction session = Substitute.For<IIdentityInteraction>();
            identityClient.InteractAsync(Arg.Any<IIdentityInteraction>()).Returns(Task.FromResult(session));
            serviceProvider.RegisterService(identityClient);
            // prevent unresolvable services
            serviceProvider.RegisterService(Substitute.For<IIdentityClientConfigurationProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityDataProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityViewModelProvider>());
            serviceProvider.RegisterService(Substitute.For<ISessionProvider>());
            serviceProvider.RegisterService(Substitute.For<IStorageProvider>());
            serviceProvider.RegisterService(Substitute.For<ILoggingProvider>());
            serviceProvider.RegisterService(Substitute.For<IViewProvider>());
            // --

            IdentityDataProvider identityDataProvider = new IdentityDataProvider(serviceProvider);
            bool? eventFired = false;
            identityDataProvider.SessionStarting += (sender, args) => eventFired = true;
            await identityDataProvider.StartSessionAsync();

            eventFired.Should().BeTrue();
        }

        [Fact]
        public async Task FireSessionStartedEvent()
        {
            ServiceProvider serviceProvider = new ServiceProvider();
            IIdentityClient identityClient = Substitute.For<IIdentityClient>();
            IIdentityInteraction session = Substitute.For<IIdentityInteraction>();
            identityClient.InteractAsync(Arg.Any<IIdentityInteraction>()).Returns(Task.FromResult(session));
            serviceProvider.RegisterService(identityClient);
            // prevent unresolvable services
            serviceProvider.RegisterService(Substitute.For<IIdentityClientConfigurationProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityDataProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityViewModelProvider>());
            serviceProvider.RegisterService(Substitute.For<ISessionProvider>());
            serviceProvider.RegisterService(Substitute.For<IStorageProvider>());
            serviceProvider.RegisterService(Substitute.For<ILoggingProvider>());
            serviceProvider.RegisterService(Substitute.For<IViewProvider>());
            // --

            IdentityDataProvider identityDataProvider = new IdentityDataProvider(serviceProvider);
            bool? eventFired = false;
            identityDataProvider.SessionStarted += (sender, args) => eventFired = true;
            await identityDataProvider.StartSessionAsync();

            eventFired.Should().BeTrue();
        }

        [Fact]
        public async Task FireSessionStartExceptionThrownEvent()
        {
            ServiceProvider serviceProvider = new ServiceProvider();
            IIdentityClient identityClient = Substitute.For<IIdentityClient>();
            IIdentityInteraction session = Substitute.For<IIdentityInteraction>();
            identityClient.InteractAsync(Arg.Any<IIdentityInteraction>()).Returns(Task.FromResult(session));
            serviceProvider.RegisterService(identityClient);
            // prevent unresolvable services
            serviceProvider.RegisterService(Substitute.For<IIdentityClientConfigurationProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityDataProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityViewModelProvider>());
            serviceProvider.RegisterService(Substitute.For<ISessionProvider>());
            serviceProvider.RegisterService(Substitute.For<IStorageProvider>());
            serviceProvider.RegisterService(Substitute.For<ILoggingProvider>());
            serviceProvider.RegisterService(Substitute.For<IViewProvider>());
            // --

            IdentityDataProvider identityDataProvider = new IdentityDataProvider(serviceProvider);
            bool? eventFired = false;
            identityDataProvider.SessionStarted += (sender, args) => throw new Exception($"testing that the {nameof(identityDataProvider.SessionStartExceptionThrown)} event fires");
            identityDataProvider.SessionStartExceptionThrown += (sender, args) => eventFired = true;
            await identityDataProvider.StartSessionAsync();

            eventFired.Should().BeTrue();
        }

        [Fact]
        public async Task FireGetFormDataStartedEvent()
        {
            string testInteractionHandle = "test interaction handle";
            ServiceProvider serviceProvider = new ServiceProvider();
            IIdentityClient identityClient = Substitute.For<IIdentityClient>();
            IIdentityInteraction session = Substitute.For<IIdentityInteraction>();
            identityClient.InteractAsync(Arg.Any<IIdentityInteraction>()).Returns(Task.FromResult(session));
            serviceProvider.RegisterService(identityClient);
            // prevent unresolvable services
            serviceProvider.RegisterService(Substitute.For<IIdentityClientConfigurationProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityDataProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityViewModelProvider>());
            serviceProvider.RegisterService(Substitute.For<ISessionProvider>());
            serviceProvider.RegisterService(Substitute.For<IStorageProvider>());
            serviceProvider.RegisterService(Substitute.For<ILoggingProvider>());
            serviceProvider.RegisterService(Substitute.For<IViewProvider>());
            // --

            IdentityDataProvider identityDataProvider = new IdentityDataProvider(serviceProvider);
            bool? eventFired = false;
            identityDataProvider.GetFormDataStarted += (sender, args) => eventFired = true;
            await identityDataProvider.GetFormDataAsync(testInteractionHandle);

            eventFired.Should().BeTrue();
        }

        [Fact]
        public async Task FireGetFormDataCompletedEvent()
        {
            string testInteractionHandle = "test interaction handle";
            ServiceProvider serviceProvider = new ServiceProvider();
            IIdentityClient identityClient = Substitute.For<IIdentityClient>();
            IIdentityInteraction session = Substitute.For<IIdentityInteraction>();
            identityClient.InteractAsync(Arg.Any<IIdentityInteraction>()).Returns(Task.FromResult(session));
            serviceProvider.RegisterService(identityClient);
            // prevent unresolvable services
            serviceProvider.RegisterService(Substitute.For<IIdentityClientConfigurationProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityDataProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityViewModelProvider>());
            serviceProvider.RegisterService(Substitute.For<ISessionProvider>());
            serviceProvider.RegisterService(Substitute.For<IStorageProvider>());
            serviceProvider.RegisterService(Substitute.For<ILoggingProvider>());
            serviceProvider.RegisterService(Substitute.For<IViewProvider>());
            // --

            IdentityDataProvider identityDataProvider = new IdentityDataProvider(serviceProvider);
            bool? eventFired = false;
            identityDataProvider.GetFormDataCompleted += (sender, args) => eventFired = true;
            await identityDataProvider.GetFormDataAsync(testInteractionHandle);

            eventFired.Should().BeTrue();
        }

        [Fact]
        public async Task FireGetFormDataExceptionThrownEvent()
        {
            string testInteractionHandle = "test interaction handle";
            ServiceProvider serviceProvider = new ServiceProvider();
            IIdentityClient identityClient = Substitute.For<IIdentityClient>();
            IIdentityInteraction session = Substitute.For<IIdentityInteraction>();
            identityClient.InteractAsync(Arg.Any<IIdentityInteraction>()).Returns(Task.FromResult(session));
            serviceProvider.RegisterService(identityClient);
            // prevent unresolvable services
            serviceProvider.RegisterService(Substitute.For<IIdentityClientConfigurationProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityDataProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityViewModelProvider>());
            serviceProvider.RegisterService(Substitute.For<ISessionProvider>());
            serviceProvider.RegisterService(Substitute.For<IStorageProvider>());
            serviceProvider.RegisterService(Substitute.For<ILoggingProvider>());
            serviceProvider.RegisterService(Substitute.For<IViewProvider>());
            // --

            IdentityDataProvider identityDataProvider = new IdentityDataProvider(serviceProvider);
            bool? eventFired = false;
            identityDataProvider.GetFormDataStarted += (sender, args) => throw new Exception($"testing that the {nameof(identityDataProvider.GetFormDataExcpetionThrown)} event fires");
            identityDataProvider.GetFormDataExcpetionThrown += (sender, args) => eventFired = true;
            await identityDataProvider.GetFormDataAsync(testInteractionHandle);

            eventFired.Should().BeTrue();
        }


        [Fact]
        public async Task FireGetViewModelStartedEvent()
        {
            ServiceProvider serviceProvider = new ServiceProvider();
            // prevent unresolvable services
            serviceProvider.RegisterService(Substitute.For<IIdentityClient>());
            serviceProvider.RegisterService(Substitute.For<IIdentityClientConfigurationProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityDataProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityViewModelProvider>());
            serviceProvider.RegisterService(Substitute.For<ISessionProvider>());
            serviceProvider.RegisterService(Substitute.For<IStorageProvider>());
            serviceProvider.RegisterService(Substitute.For<ILoggingProvider>());
            serviceProvider.RegisterService(Substitute.For<IViewProvider>());
            // --

            IdentityDataProvider identityDataProvider = new IdentityDataProvider(serviceProvider);
            bool? eventFired = false;
            identityDataProvider.GetViewModelStarted += (sender, args) => eventFired = true;
            await identityDataProvider.GetViewModelAsync(Substitute.For<IIdentityIntrospection>());

            eventFired.Should().BeTrue();
        }

        [Fact]
        public async Task FireGetViewModelCompletedEvent()
        {
            ServiceProvider serviceProvider = new ServiceProvider();
            // prevent unresolvable services
            serviceProvider.RegisterService(Substitute.For<IIdentityClient>());
            serviceProvider.RegisterService(Substitute.For<IIdentityClientConfigurationProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityDataProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityViewModelProvider>());
            serviceProvider.RegisterService(Substitute.For<ISessionProvider>());
            serviceProvider.RegisterService(Substitute.For<IStorageProvider>());
            serviceProvider.RegisterService(Substitute.For<ILoggingProvider>());
            serviceProvider.RegisterService(Substitute.For<IViewProvider>());
            // --

            IdentityDataProvider identityDataProvider = new IdentityDataProvider(serviceProvider);
            bool? eventFired = false;
            identityDataProvider.GetViewModelCompleted += (sender, args) => eventFired = true;
            await identityDataProvider.GetViewModelAsync(Substitute.For<IIdentityIntrospection>());

            eventFired.Should().BeTrue();
        }

        [Fact]
        public async Task FireGetViewModelExceptionThrownEvent()
        {
            ServiceProvider serviceProvider = new ServiceProvider();
            // prevent unresolvable services
            serviceProvider.RegisterService(Substitute.For<IIdentityClient>());
            serviceProvider.RegisterService(Substitute.For<IIdentityClientConfigurationProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityDataProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityViewModelProvider>());
            serviceProvider.RegisterService(Substitute.For<ISessionProvider>());
            serviceProvider.RegisterService(Substitute.For<IStorageProvider>());
            serviceProvider.RegisterService(Substitute.For<ILoggingProvider>());
            serviceProvider.RegisterService(Substitute.For<IViewProvider>());
            // --

            IdentityDataProvider identityDataProvider = new IdentityDataProvider(serviceProvider);
            bool? eventFired = false;
            identityDataProvider.GetViewModelStarted += (sender, args) => throw new Exception($"testing that the {nameof(identityDataProvider.GetViewModelExceptionThrown)} event fires");
            identityDataProvider.GetViewModelExceptionThrown += (sender, args) => eventFired = true;
            await identityDataProvider.GetViewModelAsync(Substitute.For<IIdentityIntrospection>());

            eventFired.Should().BeTrue();
        }

        [Fact]
        public async Task CallViewModelProviderGetViewModel()
        {
            ServiceProvider serviceProvider = new ServiceProvider();
            IIdentityViewModelProvider viewModelProvider = Substitute.For<IIdentityViewModelProvider>();
            // prevent unresolvable services
            serviceProvider.RegisterService(Substitute.For<IIdentityClient>());
            serviceProvider.RegisterService(Substitute.For<IIdentityClientConfigurationProvider>());
            serviceProvider.RegisterService(Substitute.For<IIdentityDataProvider>());
            serviceProvider.RegisterService(viewModelProvider);
            serviceProvider.RegisterService(Substitute.For<ISessionProvider>());
            serviceProvider.RegisterService(Substitute.For<IStorageProvider>());
            serviceProvider.RegisterService(Substitute.For<ILoggingProvider>());
            serviceProvider.RegisterService(Substitute.For<IViewProvider>());
            // --

            IdentityDataProvider identityDataProvider = new IdentityDataProvider(serviceProvider);
            IIdentityIntrospection identityForm = Substitute.For<IIdentityIntrospection>();
            await identityDataProvider.GetViewModelAsync(identityForm);

            await viewModelProvider.Received().GetViewModelAsync(identityForm);
        }
    }
}
