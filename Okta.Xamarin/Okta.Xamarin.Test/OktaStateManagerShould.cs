// <copyright file="OktaStateManagerShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using FluentAssertions;
using System;
using Xunit;

namespace Okta.Xamarin.Test
{

    public class OktaStateManagerShould
    {
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

        [Fact]
        public void ClearState()
        {
            string testAccessToken = "test access token";
            string testRefreshToken = "test refresh token";
            string testIdToken = "test id token";
            string testScope = "test scope";
            TestOktaStateManager testOktaStateManager = new TestOktaStateManager(testAccessToken, testIdToken, testRefreshToken, 300 /* seconds */, testScope);
            testOktaStateManager.AccessToken.Should().Be(testAccessToken);
            testOktaStateManager.IdToken.Should().Be(testIdToken);
            testOktaStateManager.RefreshToken.Should().Be(testRefreshToken);
            testOktaStateManager.Scope.Should().Be(testScope);
            testOktaStateManager.Expires.Should().NotBeNull();

            testOktaStateManager.Clear();

            testOktaStateManager.AccessToken.Should().BeNullOrEmpty();
            testOktaStateManager.IdToken.Should().BeNullOrEmpty();
            testOktaStateManager.RefreshToken.Should().BeNullOrEmpty();
            testOktaStateManager.Scope.Should().BeNullOrEmpty();
            testOktaStateManager.Expires.Should().BeNull();
        }
    }
}
