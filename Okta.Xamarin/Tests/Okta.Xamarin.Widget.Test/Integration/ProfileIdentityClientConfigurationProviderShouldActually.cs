// <copyright file="ProfileIdentityClientConfigurationProviderShouldActually.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using FluentAssertions;
using Okta.Xamarin.Widget.Pipeline.Configuration;
using Okta.Xamarin.Widget.Pipeline.Identity;
using Xunit;

namespace Okta.Xamarin.Widget.Test.Integration
{
    public class ProfileIdentityClientConfigurationProviderShouldActually
    {
        [Fact]
        public void GetDefaultConfiguration()
        {
            ProfileIdentityClientConfigurationProvider configurationProvider = new ProfileIdentityClientConfigurationProvider();

            IdentityClientConfiguration configuration = configurationProvider.GetConfiguration();

            configuration.Equals(IdentityClientConfiguration.Default).Should().BeTrue();
            configuration.OktaDomain.Should().BeEquivalentTo(IdentityClientConfiguration.Default.OktaDomain);
            configuration.ClientId.Should().BeEquivalentTo(IdentityClientConfiguration.Default.ClientId);
            configuration.IssuerUri.Should().BeEquivalentTo(IdentityClientConfiguration.Default.IssuerUri);
            configuration.RedirectUri.Should().BeEquivalentTo(IdentityClientConfiguration.Default.RedirectUri);
        }
    }
}
