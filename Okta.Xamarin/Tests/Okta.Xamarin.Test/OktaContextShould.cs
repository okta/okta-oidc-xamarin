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
        public void LoadState()
        {
            string testAccessToken = "load state test:  access token";
            OktaContext oktaContext = new OktaContext() { StateManager = new OktaStateManager { AccessToken = null } };
            OktaContext.Current = oktaContext;

            IOktaConfig testConfig = Substitute.For<IOktaConfig>();
            IOidcClient testClient = Substitute.For<IOidcClient>();
            SecureKeyValueStore testSecureKeyValueStore = Substitute.For<SecureKeyValueStore>();
            testSecureKeyValueStore.GetAsync<OktaStateManager>(OktaStateManager.StoreKey).Returns(new OktaStateManager { AccessToken = testAccessToken }); // represents what is in secure storage
            OktaContext.RegisterServiceImplementation<SecureKeyValueStore>(testSecureKeyValueStore);
            OktaContext.RegisterServiceImplementation<IOktaConfig>(testConfig);
            OktaContext.RegisterServiceImplementation<IOidcClient>(testClient);

            bool? completedEventWasRaised = false;
            OktaContext.AddLoadStateCompletedListener((sender, args) =>
            {
                args.OktaStateManager.AccessToken.Should().Be(testAccessToken);
                completedEventWasRaised = true;
            });

            OktaContext.AccessToken.Should().BeNull();

            OktaContext.LoadStateAsync().Wait();

            completedEventWasRaised.Should().BeTrue();
            OktaContext.AccessToken.Should().BeEquivalentTo(testAccessToken);
        }

        [Fact]
        public void NotLoadEmptyStateOverAuthenticatedState()
        {
            string testAccessToken = "test access token";
            OktaContext oktaContext = new OktaContext() { StateManager = new OktaStateManager { AccessToken = testAccessToken } };
            OktaContext.Current = oktaContext;

            IOktaConfig testConfig = Substitute.For<IOktaConfig>();
            IOidcClient testClient = Substitute.For<IOidcClient>();
            SecureKeyValueStore testSecureKeyValueStore = Substitute.For<SecureKeyValueStore>();
            testSecureKeyValueStore.GetAsync<OktaStateManager>(OktaStateManager.StoreKey).Returns(new OktaStateManager { AccessToken = null }); // represents what is in secure storage
            OktaContext.RegisterServiceImplementation<SecureKeyValueStore>(testSecureKeyValueStore);
            OktaContext.RegisterServiceImplementation<IOktaConfig>(testConfig);
            OktaContext.RegisterServiceImplementation<IOidcClient>(testClient);

            bool? completedEventWasRaised = false;
            OktaContext.AddLoadStateCompletedListener((sender, args) =>
            {
                args.OktaStateManager.AccessToken.Should().Be(testAccessToken);
                completedEventWasRaised = true;
            });

            OktaContext.AccessToken.Should().BeEquivalentTo(testAccessToken);

            OktaContext.LoadStateAsync().Wait();

            completedEventWasRaised.Should().BeTrue();
            OktaContext.AccessToken.Should().BeEquivalentTo(testAccessToken); // Loading state should not overwrite existing access token with empty string
        }

        [Fact]
        public void RaiseLoadStateExceptionEvent()
        {
            IOktaStateManager stateManager = Substitute.For<IOktaStateManager>();
            stateManager.ReadFromSecureStorageAsync().Returns(new OktaStateManager());
            OktaContext oktaContext = new OktaContext() { StateManager = stateManager };
            OktaContext.Current = oktaContext;

            bool? exceptionEventWasRaised = false;
            OktaContext.AddLoadStateStartedListener((sender, args) => throw new Exception("This is a test exception to test that the exception event is raised"));
            OktaContext.AddLoadStateExceptionListener((sender, args) => exceptionEventWasRaised = true);

            OktaContext.LoadStateAsync().Wait();

            exceptionEventWasRaised.Should().BeTrue();
        }

        [Fact]
        public void RaiseLoadStateEvents()
        {
            IOktaStateManager stateManager = Substitute.For<IOktaStateManager>();
            stateManager.ReadFromSecureStorageAsync().Returns(new OktaStateManager());
            OktaContext oktaContext = new OktaContext() { StateManager = stateManager };
            OktaContext.Current = oktaContext;

            bool? startedEventWasRaised = false;
            bool? completedEventWasRaised = false;
            OktaContext.AddLoadStateStartedListener((sender, args) => startedEventWasRaised = true);
            OktaContext.AddLoadStateCompletedListener((sender, args) => completedEventWasRaised = true);

            OktaContext.LoadStateAsync().Wait();

            startedEventWasRaised.Should().BeTrue();
            completedEventWasRaised.Should().BeTrue();
        }

        [Fact]
        public void RaiseSecureStorageWriteEventsOnSaveStateAsync()
        {
            TestOktaStateManager testOktaStateManager = new TestOktaStateManager();
            OktaContext oktaContext = new OktaContext() { StateManager = testOktaStateManager };
            OktaContext.Current = oktaContext;

            IOktaConfig testConfig = Substitute.For<IOktaConfig>();
            IOidcClient testClient = Substitute.For<IOidcClient>();
            SecureKeyValueStore testSecureKeyValueStore = Substitute.For<SecureKeyValueStore>();
            testSecureKeyValueStore.GetAsync<OktaStateManager>(OktaStateManager.StoreKey).Returns(testOktaStateManager);
            OktaContext.RegisterServiceImplementation<SecureKeyValueStore>(testSecureKeyValueStore);
            OktaContext.RegisterServiceImplementation<IOktaConfig>(testConfig);
            OktaContext.RegisterServiceImplementation<IOidcClient>(testClient);

            bool? startedEventWasRaised = false;
            bool? completedEventWasRaised = false;
            OktaContext.AddSecureStorageWriteStartedListener((sender, args) => startedEventWasRaised = true);
            OktaContext.AddSecureStorageWriteCompletedListener((sender, args) => completedEventWasRaised = true);

            OktaContext.SaveStateAsync().Wait();

            startedEventWasRaised.Should().BeTrue();
            completedEventWasRaised.Should().BeTrue();
        }

        [Fact]
        public void RaiseSecureStorageReadEventsOnLoadStateAsync()
        {
            TestOktaStateManager testOktaStateManager = new TestOktaStateManager();
            OktaContext oktaContext = new OktaContext() { StateManager = testOktaStateManager };
            OktaContext.Current = oktaContext;

            IOktaConfig testConfig = Substitute.For<IOktaConfig>();
            IOidcClient testClient = Substitute.For<IOidcClient>();
            SecureKeyValueStore testSecureKeyValueStore = Substitute.For<SecureKeyValueStore>();
            testSecureKeyValueStore.GetAsync<OktaStateManager>(OktaStateManager.StoreKey).Returns(testOktaStateManager);
            OktaContext.RegisterServiceImplementation<SecureKeyValueStore>(testSecureKeyValueStore);
            OktaContext.RegisterServiceImplementation<IOktaConfig>(testConfig);
            OktaContext.RegisterServiceImplementation<IOidcClient>(testClient);

            bool? startedEventWasRaised = false;
            bool? completedEventWasRaised = false;
            OktaContext.AddSecureStorageReadStartedListener((sender, args) => startedEventWasRaised = true);
            OktaContext.AddSecureStorageReadCompletedListener((sender, args) => completedEventWasRaised = true);

            bool loaded = OktaContext.LoadStateAsync().Result;

            loaded.Should().BeTrue();
            startedEventWasRaised.Should().BeTrue();
            completedEventWasRaised.Should().BeTrue();
        }

        [Fact]
        public void RaiseEventOnStateManagerSecureStorageReadException()
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
        public void RaiseEventOnStateManagerSecureStorageWriteException()
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

        [Fact]
        public void RaiseRenewExceptionEvent()
        {
            bool? renewExceptionEventRaised = false;
            Exception testException = new Exception("This is a test exception");
            IOidcClient mockClient = Substitute.For<IOidcClient>();
            mockClient.RenewAsync<RenewResponse>(Arg.Any<string>()).Returns(new RenewResponse());
            OktaContext.Current.StateManager.Client = mockClient;
            OktaContext.AddRenewExceptionListener((sender, renewExceptionEventArgs) =>
            {
                renewExceptionEventArgs.Exception.Should().Be(testException);
                renewExceptionEventRaised = true;
            });

            OktaContext.AddRenewCompletedListener((sender, renewEventArgs) => throw testException);

            OktaContext.Current.RenewAsync().Wait();

            renewExceptionEventRaised.Should().BeTrue();
        }

        [Fact]
        public void RaiseRevokeExceptionEvent()
        {
            bool? revokeExceptionEventRaised = false;
            Exception testException = new Exception("This is a test exception");
            IOktaStateManager mockStateManager = Substitute.For<IOktaStateManager>();

            OktaContext.AddRevokeExceptionListener((sender, revokeExceptionEventArgs) =>
            {
                revokeExceptionEventArgs.Exception.Should().Be(testException);
                revokeExceptionEventRaised = true;
            });

            OktaContext.AddRevokeCompletedListener((sender, renewEventArgs) => throw testException);

            OktaContext.Current.StateManager = mockStateManager;
            OktaContext.Current.RevokeAsync().Wait();

            revokeExceptionEventRaised.Should().BeTrue();
        }

        [Fact]
        public void RevokeSpecifiedAccessToken()
        {
            string goodAccessToken = "this is the one";
            string badAccessToken = "this is the wrong one";
            IOktaStateManager mockOktaStateManager = Substitute.For<IOktaStateManager>();
            mockOktaStateManager.AccessToken.Returns(badAccessToken);

            OktaContext oktaContext = new OktaContext() { StateManager = mockOktaStateManager };
            OktaContext.Current = oktaContext;
            OktaContext.RevokeAccessTokenAsync(goodAccessToken).Wait();

            mockOktaStateManager.Received().RevokeAccessTokenAsync(goodAccessToken);
        }
    }
}
