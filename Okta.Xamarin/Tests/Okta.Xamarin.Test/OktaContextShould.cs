// <copyright file="OktaContextShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Okta.Xamarin.Services;
using Xunit;

namespace Okta.Xamarin.Test
{
    public class OktaContextShould
    {
        [Fact]
        public void RaiseSecureStorageWriteEventsOnSaveStateAsync()
        {
            TestOktaStateManager testOktaStateManager = new TestOktaStateManager();
            IOktaConfig testConfig = Substitute.For<IOktaConfig>();
            IOidcClient testClient = Substitute.For<IOidcClient>();
            SecureKeyValueStore testSecureKeyValueStore = Substitute.For<SecureKeyValueStore>();
            testSecureKeyValueStore.GetAsync<OktaStateManager>(OktaStateManager.StoreKey).Returns(testOktaStateManager);
            OktaContext.RegisterServiceImplementation<SecureKeyValueStore>(testSecureKeyValueStore);
            OktaContext.RegisterServiceImplementation<IOktaConfig>(testConfig);
            OktaContext.RegisterServiceImplementation<IOidcClient>(testClient);
            OktaContext oktaContext = new OktaContext() { StateManager = testOktaStateManager };
            bool? startedEventWasRaised = false;
            bool? completedEventWasRaised = false;
            oktaContext.SecureStorageWriteStarted += (sender, args) => startedEventWasRaised = true;
            oktaContext.SecureStorageWriteCompleted += (sender, args) => completedEventWasRaised = true;

            oktaContext.SaveStateAsync().Wait();

            startedEventWasRaised.Should().BeTrue();
            completedEventWasRaised.Should().BeTrue();
        }

        [Fact]
        public void RaiseSecureStorageReadEventsOnLoadStateAsync()
        {
            TestOktaStateManager testOktaStateManager = new TestOktaStateManager();
            IOktaConfig testConfig = Substitute.For<IOktaConfig>();
            IOidcClient testClient = Substitute.For<IOidcClient>();
            SecureKeyValueStore testSecureKeyValueStore = Substitute.For<SecureKeyValueStore>();
            testSecureKeyValueStore.GetAsync<OktaStateManager>(OktaStateManager.StoreKey).Returns(testOktaStateManager);
            OktaContext.RegisterServiceImplementation<SecureKeyValueStore>(testSecureKeyValueStore);
            OktaContext.RegisterServiceImplementation<IOktaConfig>(testConfig);
            OktaContext.RegisterServiceImplementation<IOidcClient>(testClient);
            OktaContext oktaContext = new OktaContext() { StateManager = testOktaStateManager };
            bool? startedEventWasRaised = false;
            bool? completedEventWasRaised = false;
            oktaContext.SecureStorageReadStarted += (sender, args) => startedEventWasRaised = true;
            oktaContext.SecureStorageReadCompleted += (sender, args) => completedEventWasRaised = true;

            bool loaded = oktaContext.LoadStateAsync().Result;

            loaded.Should().BeTrue();
            startedEventWasRaised.Should().BeTrue();
            completedEventWasRaised.Should().BeTrue();
        }

        [Fact]
        public void RaiseEventOnSecureStorageReadException()
        {
            TestOktaStateManager testStateManager = new TestOktaStateManager();
            OktaContext oktaContext = new OktaContext() { StateManager = testStateManager };

            bool? eventWasRaised = false;
            Exception testException = new Exception("This is a test exception");
            oktaContext.SecureStorageReadException += (sender, args) =>
            {
                args.Exception.Should().Be(testException);
                eventWasRaised = true;
            };

            testStateManager.RaiseSecureStorageReadException(new SecureStorageExceptionEventArgs { Exception = testException });
            eventWasRaised.Should().BeTrue();
        }

        [Fact]
        public void RaiseEventOnSecureStorageWriteException()
        {
            TestOktaStateManager testStateManger = new TestOktaStateManager();
            OktaContext oktaContext = new OktaContext() { StateManager = testStateManger };

            bool? eventWasRaised = false;
            Exception testException = new Exception("This is a test exception");
            oktaContext.SecureStorageWriteException += (sender, args) =>
            {
                args.Exception.Should().Be(testException);
                eventWasRaised = true;
            };

            testStateManger.RaiseSecureStorageWriteException(new SecureStorageExceptionEventArgs { Exception = testException });
            eventWasRaised.Should().BeTrue();
        }

        [Fact]
        public void NotRaiseSignInCompleteOnOAuthException()
        {
            IOidcClient mockOidcClient = Substitute.For<IOidcClient>();
            mockOidcClient.When(oidcClient => oidcClient.SignInWithBrowserAsync()).Do(mockOidcClient => throw new OAuthException());

            OktaContext oktaContext = new OktaContext();
            bool? authenticationFailedEventRaised = false;
            bool? signInCompletedEventRaised = false;
            oktaContext.AuthenticationFailed += (sender, args) => authenticationFailedEventRaised = true;
            oktaContext.SignInCompleted += (sender, args) => signInCompletedEventRaised = true;

            oktaContext.SignInAsync(mockOidcClient);

            Assert.True(authenticationFailedEventRaised);
            Assert.False(signInCompletedEventRaised);
        }

        [Fact]
        public void RaiseEventOnRequestException()
        {
            TestOidcClient client = new TestOidcClient(new OktaConfig("testoktaid", "https://dev-00000.okta.com", "com.test:/redirect", "com.test:/logout"));
            IOktaStateManager oktaStateManager = new TestOktaStateManager();
            OktaContext oktaContext = new OktaContext();
            bool? clientEventRaised = false;
            bool? stateManagerEventRaised = false;
            bool? contextEventRaised = false;
            client.RequestException += (sender, args) => clientEventRaised = true;
            oktaStateManager.RequestException += (sender, args) => stateManagerEventRaised = true;
            oktaContext.RequestException += (sender, args) => contextEventRaised = true;

            oktaStateManager.Client = client;
            oktaContext.StateManager = oktaStateManager;

            Assert.False(clientEventRaised);
            Assert.False(stateManagerEventRaised);
            Assert.False(contextEventRaised);

            client.RaiseRequestExceptionEvent(new RequestExceptionEventArgs(new Exception("Test exception"), HttpMethod.Post, "/test/path", new Dictionary<string, string>(), "default", new KeyValuePair<string, string>[] { }));

            Assert.True(clientEventRaised);
            Assert.True(stateManagerEventRaised);
            Assert.True(contextEventRaised);
        }

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

            OktaContext.Current.RevokeAsync(TokenKind.AccessToken).Wait();

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

        [Fact]
        public void RaiseInitServicesEvents()
        {
            bool? initServicesStartedRaised = false;
            bool? initServicesCompletedRaised = false;
            bool? initServicesExceptionRaised = false;
            OktaContext.Current.InitServicesStarted += (sender, args) => initServicesStartedRaised = true;
            OktaContext.Current.InitServicesCompleted += (sender, args) => initServicesCompletedRaised = true;

            TinyIoC.TinyIoCContainer container = new TinyIoC.TinyIoCContainer();
            container.Register(Substitute.For<IOidcClient>());
            container.Register(Substitute.For<SecureKeyValueStore>());
            OktaContext.Current.InitServices(container);

            initServicesStartedRaised.Should().BeTrue();
            initServicesCompletedRaised.Should().BeTrue();
            initServicesExceptionRaised.Should().BeFalse();
        }

        [Fact]
        public void RaiseInitServicesExceptionEvent()
        {
            bool? initServicesStartedRaised = false;
            bool? initServicesCompletedRaised = false;
            bool? initServicesExceptionRaised = false;
            OktaContext.Current.InitServicesStarted += (sender, args) => initServicesStartedRaised = true;
            OktaContext.Current.InitServicesCompleted += (sender, args) =>
            {
                initServicesCompletedRaised = true;
                throw new Exception("throwing exception to test that the related exception event is raised");
            };
            OktaContext.Current.InitServicesException += (sender, args) => initServicesExceptionRaised = true;

            TinyIoC.TinyIoCContainer container = new TinyIoC.TinyIoCContainer();
            container.Register(Substitute.For<IOidcClient>());
            container.Register(Substitute.For<SecureKeyValueStore>());
            OktaContext.Current.InitServices(container);

            initServicesStartedRaised.Should().BeTrue();
            initServicesCompletedRaised.Should().BeTrue();
            initServicesExceptionRaised.Should().BeTrue();
        }

        [Fact]
        public void HaveIoCContainer()
        {
            OktaContext context = new OktaContext();
            context.IoCContainer.Should().NotBeNull();
        }
    }
}
