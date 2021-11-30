// <copyright file="IdentityClientShouldActually.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Threading.Tasks;
using FluentAssertions;
using Okta.Xamarin.Oie.Client;
using Xunit;

namespace Okta.Xamarin.Oie.Test.Integration
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
