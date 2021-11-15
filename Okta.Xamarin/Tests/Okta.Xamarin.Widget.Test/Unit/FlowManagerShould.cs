// <copyright file="FlowManagerShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using FluentAssertions;
using NSubstitute;
using Okta.Xamarin.Widget.Pipeline;
using Okta.Xamarin.Widget.Pipeline.Data;
using Okta.Xamarin.Widget.Pipeline.Identity;
using Okta.Xamarin.Widget.Pipeline.Identity.Data;
using Okta.Xamarin.Widget.Pipeline.Logging;
using Okta.Xamarin.Widget.Pipeline.Policy;
using Okta.Xamarin.Widget.Pipeline.Session;
using Okta.Xamarin.Widget.Pipeline.View;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Okta.Xamarin.Widget.Test.Unit
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

            PipelineManager flowProvider = new PipelineManager(serviceProvider);
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

            PipelineManager flowManager = new PipelineManager(serviceProvider);
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
            PipelineManager flowManager = new PipelineManager(serviceProvider);
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
            PipelineManager flowManager = new PipelineManager(serviceProvider);
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
            PipelineManager flowManager = new PipelineManager(serviceProvider);
            flowManager.FlowStarting += (sender, args) => throw new Exception($"testing that the {nameof(PipelineManager.FlowStartExceptionThrown)} event is fired");
            flowManager.FlowStartExceptionThrown += (sender, args) => eventFired = true;
            await flowManager.StartAsync();

            eventFired.Should().BeTrue();
       }
    }
}
