// <copyright file="OktaContextShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
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
        public async void LoadState()
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

            await OktaContext.LoadStateAsync();

            completedEventWasRaised.Should().BeTrue();
            OktaContext.AccessToken.Should().BeEquivalentTo(testAccessToken);
        }

        [Fact]
        public async void NotLoadEmptyStateOverAuthenticatedState()
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

            await OktaContext.LoadStateAsync();
            Thread.Sleep(300);

            completedEventWasRaised.Should().BeTrue();
            OktaContext.AccessToken.Should().BeEquivalentTo(testAccessToken); // Loading state should not overwrite existing access token with empty string
        }

        [Fact]
        public async void RaiseLoadStateExceptionEvent()
        {
            IOktaStateManager stateManager = Substitute.For<IOktaStateManager>();
            stateManager.ReadFromSecureStorageAsync().Returns(new OktaStateManager());
            OktaContext oktaContext = new OktaContext() { StateManager = stateManager };
            OktaContext.Current = oktaContext;

            bool? exceptionEventWasRaised = false;
            OktaContext.AddLoadStateStartedListener((sender, args) => throw new Exception("This is a test exception to test that the exception event is raised"));
            OktaContext.AddLoadStateExceptionListener((sender, args) => exceptionEventWasRaised = true);

            await OktaContext.LoadStateAsync();
            Thread.Sleep(100);

            exceptionEventWasRaised.Should().BeTrue();
        }

        [Fact]
        public async void RaiseLoadStateEvents()
        {
            IOktaStateManager stateManager = Substitute.For<IOktaStateManager>();
            stateManager.ReadFromSecureStorageAsync().Returns(new OktaStateManager());
            OktaContext oktaContext = new OktaContext() { StateManager = stateManager };
            OktaContext.Current = oktaContext;

            bool? startedEventWasRaised = false;
            bool? completedEventWasRaised = false;
            OktaContext.AddLoadStateStartedListener((sender, args) => startedEventWasRaised = true);
            OktaContext.AddLoadStateCompletedListener((sender, args) => completedEventWasRaised = true);

            await OktaContext.LoadStateAsync();
            Thread.Sleep(100);

            startedEventWasRaised.Should().BeTrue();
            completedEventWasRaised.Should().BeTrue();
        }

        [Fact]
        public async void RaiseSecureStorageWriteEventsOnSaveStateAsync()
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

            await OktaContext.SaveStateAsync();

            startedEventWasRaised.Should().BeTrue();
            completedEventWasRaised.Should().BeTrue();
        }

        [Fact]
        public async void RaiseSecureStorageReadEventsOnLoadStateAsync()
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

            bool loaded = await OktaContext.LoadStateAsync();
            Thread.Sleep(300);

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
        public async void NotRaiseSignInCompleteOnOAuthException()
        {
            IOidcClient mockOidcClient = Substitute.For<IOidcClient>();
            mockOidcClient.When(oidcClient => oidcClient.SignInWithBrowserAsync()).Do(mockOidcClient => throw new OAuthException());

            OktaContext oktaContext = new OktaContext();
            bool? authenticationFailedEventRaised = false;
            bool? signInCompletedEventRaised = false;
            oktaContext.AuthenticationFailed += (sender, args) => authenticationFailedEventRaised = true;
            oktaContext.SignInCompleted += (sender, args) => signInCompletedEventRaised = true;

            await oktaContext.SignInAsync(mockOidcClient);

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
        public async void RaiseSignInEventsOnSignIn()
        {
            IOidcClient client = Substitute.For<IOidcClient>();
            client.SignInWithBrowserAsync().Returns(new OktaStateManager("testAccessToken", "testTokenType", "testIdToken", "testRefreshToken"));
            client.SignOutOfOktaAsync(Arg.Any<OktaStateManager>()).Returns(new OktaStateManager(string.Empty, string.Empty));

            OktaContext.Init(client);
            bool? signInStartedEventRaised = false;
            bool? signInCompletedEventRaised = false;
            OktaContext.AddSignInStartedListener((sender, eventArgs) => signInStartedEventRaised = true);
            OktaContext.AddSignInCompletedListener((sender, eventArgs) => signInCompletedEventRaised = true);

            await OktaContext.Current.SignInAsync();

            Assert.True(signInStartedEventRaised);
            Assert.True(signInCompletedEventRaised);
        }

        [Fact]
        public async void RaiseSignOutEventsOnSignOut()
        {
            IOidcClient client = Substitute.For<IOidcClient>();
            client.SignInWithBrowserAsync().Returns(new OktaStateManager("testAccessToken", "testTokenType", "testIdToken", "testRefreshToken"));
            client.SignOutOfOktaAsync(Arg.Any<OktaStateManager>()).Returns(new OktaStateManager(string.Empty, string.Empty));

            OktaContext.Init(client);
            bool? signOutStartedEventRaised = false;
            bool? signOutCompletedEventRaised = false;
            OktaContext.AddSignOutStartedListener((sender, eventArgs) => signOutStartedEventRaised = true);
            OktaContext.AddSignOutCompletedListener((sender, eventArgs) => signOutCompletedEventRaised = true);

            await OktaContext.Current.SignInAsync();
            await OktaContext.Current.SignOutAsync();
            Thread.Sleep(300);

            Assert.True(signOutStartedEventRaised);
            Assert.True(signOutCompletedEventRaised);
        }

        [Fact]
        public async void RaiseRevokeTokenEvents()
        {
            string testAccessToken = "test access token";
            string testRefreshToken = "test refresh token";
            bool? revokingTokenRaised = false;
            bool? revokedTokenRaised = false;
            OktaContext.Current.StateManager = new TestOktaStateManager(testAccessToken, testRefreshToken);
            OktaContext.Current.RevokeStarted += (sender, args) => revokingTokenRaised = true;
            OktaContext.Current.RevokeCompleted += (sender, args) => revokedTokenRaised = true;

            await OktaContext.Current.RevokeAsync(TokenKind.AccessToken);
            Thread.Sleep(100);

            revokedTokenRaised.Should().BeTrue();
            revokingTokenRaised.Should().BeTrue();
        }

        [Fact]
        public async void RaiseGetUserEvents()
        {
            IOktaStateManager testStateManager = Substitute.For<IOktaStateManager>();
            Task<ClaimsPrincipal> testClaimsPrincipal = Task.FromResult(new ClaimsPrincipal());
            testStateManager.GetClaimsPrincipalAsync().Returns(testClaimsPrincipal);
            OktaContext.Current.StateManager = testStateManager;
            bool? getUserStartedRaised = false;
            bool? getUserCompletedRaised = false;
            OktaContext.Current.GetUserStarted += (sender, args) => getUserStartedRaised = true;
            OktaContext.Current.GetUserCompleted += (sender, args) => getUserCompletedRaised = true;

            await OktaContext.Current.GetClaimsPrincipalAsync();
            Thread.Sleep(100);

            getUserStartedRaised.Should().BeTrue();
            getUserCompletedRaised.Should().BeTrue();
        }

        [Fact]
        public async void RaiseIntrospectionEvents()
        {
            IOktaStateManager testStateManager = Substitute.For<IOktaStateManager>();
            Task<Dictionary<string, object>> testIntrospectResponse = Task.FromResult(new Dictionary<string, object>());
            testStateManager.IntrospectAsync(Arg.Any<TokenKind>()).Returns(testIntrospectResponse);
            OktaContext.Current.StateManager = testStateManager;

            bool? introspectStartedRaised = false;
            bool? introspectCompletedRaised = false;
            OktaContext.Current.IntrospectStarted += (sender, args) => introspectStartedRaised = true;
            OktaContext.Current.IntrospectCompleted += (sender, args) => introspectCompletedRaised = true;

            await OktaContext.Current.IntrospectAsync(TokenKind.AccessToken);
            Thread.Sleep(100);

            introspectStartedRaised.Should().BeTrue();
            introspectCompletedRaised.Should().BeTrue();
        }

        [Fact]
        public async void RaiseRenewEvents()
        {
            IOktaStateManager testStateManager = Substitute.For<IOktaStateManager>();
            testStateManager.RefreshToken.Returns("test refresh token");
            Task<RenewResponse> testRenewResponse = Task.FromResult(new RenewResponse());
            testStateManager.RenewAsync(Arg.Any<string>(), Arg.Any<bool>()).Returns(testRenewResponse);
            OktaContext.Current.StateManager = testStateManager;

            bool? renewStartedRaised = false;
            bool? renewCompletedRaised = false;
            OktaContext.Current.RenewStarted += (sender, args) => renewStartedRaised = true;
            OktaContext.Current.RenewCompleted += (sender, args) => renewCompletedRaised = true;

            await OktaContext.Current.RenewAsync();
            Thread.Sleep(100);

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

            Ioc.TinyIoCContainer container = new Ioc.TinyIoCContainer();
            container.Register(Substitute.For<IOidcClient>());
            container.Register(Substitute.For<SecureKeyValueStore>());
            OktaContext.Current.InitServices(container);
            Thread.Sleep(100);
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

            Ioc.TinyIoCContainer container = new Ioc.TinyIoCContainer();
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
        public async void RaiseRenewExceptionEvent()
        {
            bool? renewExceptionEventRaised = false;
            Exception testException = new Exception("This is a test exception");
            IOktaStateManager mockStateManager = Substitute.For<IOktaStateManager>();
            mockStateManager.RefreshToken.Returns("test refresh token");
            mockStateManager.RenewAsync(Arg.Any<string>(), Arg.Any<bool>()).Returns(new RenewResponse());
            OktaContext.Current.StateManager = mockStateManager;
            OktaContext.AddRenewExceptionListener((sender, renewExceptionEventArgs) =>
            {
                renewExceptionEventArgs.Exception.Should().Be(testException);
                renewExceptionEventRaised = true;
            });

            OktaContext.AddRenewCompletedListener((sender, renewEventArgs) => throw testException);

            await OktaContext.Current.RenewAsync();

            renewExceptionEventRaised.Should().BeTrue();
        }

        [Fact]
        public async void RaiseRevokeExceptionEvent()
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
            await OktaContext.Current.RevokeAsync();

            revokeExceptionEventRaised.Should().BeTrue();
        }

        [Fact]
        public async void RevokeSpecifiedAccessToken()
        {
            string goodAccessToken = "this is the one";
            string badAccessToken = "this is the wrong one";
            IOktaStateManager mockOktaStateManager = Substitute.For<IOktaStateManager>();
            mockOktaStateManager.AccessToken.Returns(badAccessToken);

            OktaContext oktaContext = new OktaContext() { StateManager = mockOktaStateManager };
            OktaContext.Current = oktaContext;
            await OktaContext.RevokeAccessTokenAsync(goodAccessToken);
            Thread.Sleep(100);

            mockOktaStateManager.Received().RevokeAccessTokenAsync(goodAccessToken);
        }
    }
}
