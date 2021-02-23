// <copyright file="OktaContextShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Okta.Xamarin.Test
{
    public class OktaContextShould
    {
        [Fact]
        public void RaiseSignInEventsOnSignIn()
        {
            IOidcClient client = Substitute.For<IOidcClient>();
            client.SignInWithBrowserAsync().Returns(new OktaStateManager("testAccessToken", "testTokenType", "testIdToken", "testRefreshToken"));
            client.SignOutOfOktaAsync(Arg.Any<OktaStateManager>()).Returns(new OktaStateManager(string.Empty, string.Empty));

            OktaContext.Init(client);
            bool? signInStartedEventRaised = false;
            bool? signInCompletedEventRaised = false;
            OktaContext.AddSignInStartedListener((sender, eventArgs) => signInStartedEventRaised = true);
            OktaContext.AddSignInCompletedListener((sender, eventArgs) => signInCompletedEventRaised = true);

            OktaContext.Current.SignInAsync().Wait();

            Assert.True(signInStartedEventRaised);
            Assert.True(signInCompletedEventRaised);
        }

        [Fact]
        public void RaiseSignOutEventsOnSignOut()
        {
            IOidcClient client = Substitute.For<IOidcClient>();
            client.SignInWithBrowserAsync().Returns(new OktaStateManager("testAccessToken", "testTokenType", "testIdToken", "testRefreshToken"));
            client.SignOutOfOktaAsync(Arg.Any<OktaStateManager>()).Returns(new OktaStateManager(string.Empty, string.Empty));

            OktaContext.Init(client);
            bool? signOutStartedEventRaised = false;
            bool? signOutCompletedEventRaised = false;
            OktaContext.AddSignOutStartedListener((sender, eventArgs) => signOutStartedEventRaised = true);
            OktaContext.AddSignOutCompletedListener((sender, eventArgs) => signOutCompletedEventRaised = true);

            OktaContext.Current.SignInAsync().Wait();
            OktaContext.Current.SignOutAsync().Wait();

            Assert.True(signOutStartedEventRaised);
            Assert.True(signOutCompletedEventRaised);
        }

        [Fact]
        public void RaiseRevokeTokenEvents()
        {
            string testAccessToken = "test access token";
            string testRefreshToken = "test refresh token";
            bool? revokingTokenRaised = false;
            bool? revokedTokenRaised = false;
            OktaContext.Current.StateManager = new TestOktaStateManager(testAccessToken, testRefreshToken);
            OktaContext.Current.RevokeStarted += (sender, args) => revokingTokenRaised = true;
            OktaContext.Current.RevokeCompleted += (sender, args) => revokedTokenRaised = true;

            OktaContext.Current.RevokeTokenAsync(TokenKind.AccessToken).Wait();

            revokedTokenRaised.Should().BeTrue();
            revokingTokenRaised.Should().BeTrue();
        }

        [Fact]
        public void RaiseGetUserEvents()
        {
            IOktaStateManager testStateManager = Substitute.For<IOktaStateManager>();
            Task<ClaimsPrincipal> testClaimsPrincipal = Task.FromResult(new ClaimsPrincipal());
            testStateManager.GetClaimsPrincipalAsync().Returns(testClaimsPrincipal);
            OktaContext.Current.StateManager = testStateManager;
            bool? getUserStartedRaised = false;
            bool? getUserCompletedRaised = false;
            OktaContext.Current.GetUserStarted += (sender, args) => getUserStartedRaised = true;
            OktaContext.Current.GetUserCompleted += (sender, args) => getUserCompletedRaised = true;

            OktaContext.Current.GetClaimsPrincipalAsync().Wait();

            getUserStartedRaised.Should().BeTrue();
            getUserCompletedRaised.Should().BeTrue();
        }

        [Fact]
        public void RaiseIntrospectionEvents()
        {
            IOktaStateManager testStateManager = Substitute.For<IOktaStateManager>();
            Task<Dictionary<string, object>> testIntrospectResponse = Task.FromResult(new Dictionary<string, object>());
            testStateManager.IntrospectAsync(Arg.Any<TokenKind>()).Returns(testIntrospectResponse);
            OktaContext.Current.StateManager = testStateManager;

            bool? introspectStartedRaised = false;
            bool? introspectCompletedRaised = false;
            OktaContext.Current.IntrospectStarted += (sender, args) => introspectStartedRaised = true;
            OktaContext.Current.IntrospectCompleted += (sender, args) => introspectCompletedRaised = true;

            OktaContext.Current.IntrospectAsync(TokenKind.AccessToken).Wait();

            introspectStartedRaised.Should().BeTrue();
            introspectCompletedRaised.Should().BeTrue();
        }

        [Fact]
        public void RaiseRenewEvents()
        {
            IOktaStateManager testStateManager = Substitute.For<IOktaStateManager>();
            Task<RenewResponse> testRenewResponse = Task.FromResult(new RenewResponse());
            testStateManager.RenewAsync().Returns(testRenewResponse);
            OktaContext.Current.StateManager = testStateManager;

            bool? renewStartedRaised = false;
            bool? renewCompletedRaised = false;
            OktaContext.Current.RenewStarted += (sender, args) => renewStartedRaised = true;
            OktaContext.Current.RenewCompleted += (sender, args) => renewCompletedRaised = true;

            OktaContext.Current.RenewAsync().Wait();

            renewStartedRaised.Should().BeTrue();
            renewCompletedRaised.Should().BeTrue();
        }
    }
}
