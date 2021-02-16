// <copyright file="OktaStateManagerShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using FluentAssertions;
using Xunit;

namespace Okta.Xamarin.Test
{

    public class OktaStateManagerShould
    {
        [Fact]
        public void GetBasePath()
        {
            string testDomain = "https://test.domain.com";
            TestOktaStateManager testOktaStateManager = new TestOktaStateManager
            {
                Config = new OktaConfig
                {
                    OktaDomain = testDomain
                }
            };
            string expected = $"{testDomain}/oauth2/v1";
            string actual = testOktaStateManager.CallGetBasePath();

            actual.Should().Be(expected);
        }

        [Fact]
        public void GetToken()
        {
            string testAccessToken = "test access token";
            string testRefreshToken = "test refresh token";
            TestOktaStateManager testOktaStateManager = new TestOktaStateManager(testAccessToken, testRefreshToken);

            string retrievedAccessToken = testOktaStateManager.GetToken(TokenType.AccessToken);
            string retrievedRefreshToken = testOktaStateManager.GetToken(TokenType.RefreshToken);

            retrievedAccessToken.Should().Be(testAccessToken);
            retrievedRefreshToken.Should().Be(testRefreshToken);
        }
    }
}
