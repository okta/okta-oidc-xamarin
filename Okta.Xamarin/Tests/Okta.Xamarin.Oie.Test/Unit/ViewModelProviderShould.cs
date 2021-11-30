// <copyright file="ViewModelProviderShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using FluentAssertions;
using NSubstitute;
using Okta.Xamarin.Oie.Client;
using Okta.Xamarin.Oie.Client.View;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Okta.Xamarin.Oie.Test.Unit
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
