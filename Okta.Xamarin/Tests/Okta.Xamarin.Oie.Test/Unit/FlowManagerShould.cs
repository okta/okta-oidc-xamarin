// <copyright file="FlowManagerShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using FluentAssertions;
using NSubstitute;
using Okta.Xamarin.Oie;
using Okta.Xamarin.Oie.Data;
using Okta.Xamarin.Oie.Client;
using Okta.Xamarin.Oie.Client.Data;
using Okta.Xamarin.Oie.Logging;
using Okta.Xamarin.Oie.Session;
using Okta.Xamarin.Oie.Views;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Okta.Xamarin.Oie.Test.Unit
{
    public class FlowManagerShould
    {
       [Fact]
       public async Task CallDataProviderOnStart()
       {
            ServiceProvider serviceProvider = new ServiceProvider();
            serviceProvider.RegisterService(Substitute.For<IIdentityClient>());
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
