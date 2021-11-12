// <copyright file="ProfileIdentityClientConfigurationShouldActually.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using FluentAssertions;
using Okta.Xamarin.Widget.Pipeline.Configuration;
using Okta.Xamarin.Widget.Pipeline.Identity;
using Xunit;

namespace Okta.Xamarin.Widget.Test.Integration
{
    public class ProfileIdentityClientConfigurationShouldActually
    {
        [Fact]
        public void HaveFile()
        {
            string testOktaDomain = "test okta domain";
            string testClientId = "test client id";

            ProfileIdentityClientConfiguration configuration = new ProfileIdentityClientConfiguration().Load();
            if (configuration.File.Exists)
            {
                configuration.File.Delete();
            }

            configuration.File.Exists.Should().BeFalse();

            configuration.OktaDomain = testOktaDomain;
            configuration.ClientId = testClientId;

            configuration.Save();
            configuration.File.Exists.Should().BeTrue();

            ProfileIdentityClientConfiguration reloaded = new ProfileIdentityClientConfiguration().Load();
            reloaded.OktaDomain.Should().BeEquivalentTo(testOktaDomain);
            reloaded.ClientId.Should().BeEquivalentTo(testClientId);

            configuration.File.Delete();
        }

        [Fact]
        public void EqualDefault()
        {
            ProfileIdentityClientConfiguration configuration = new ProfileIdentityClientConfiguration().Load();
            if (configuration.File.Exists)
            {
                configuration.File.Delete();
                configuration = new ProfileIdentityClientConfiguration().Load();
            }

            configuration.Equals(IdentityClientConfiguration.Default).Should().BeTrue();
            configuration.OktaDomain.Should().BeEquivalentTo(IdentityClientConfiguration.Default.OktaDomain);
            configuration.ClientId.Should().BeEquivalentTo(IdentityClientConfiguration.Default.ClientId);
            configuration.IssuerUri.Should().BeEquivalentTo(IdentityClientConfiguration.Default.IssuerUri);
            configuration.RedirectUri.Should().BeEquivalentTo(IdentityClientConfiguration.Default.RedirectUri);
        }
    }
}
