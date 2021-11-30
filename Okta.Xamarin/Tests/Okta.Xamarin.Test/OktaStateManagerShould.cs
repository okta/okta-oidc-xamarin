// <copyright file="OktaStateManagerShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Okta.Xamarin.Services;
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

            string retrievedAccessToken = testOktaStateManager.GetToken(TokenKind.AccessToken);
            string retrievedRefreshToken = testOktaStateManager.GetToken(TokenKind.RefreshToken);

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

        [Fact]
        public void SerializeAndDeserialize()
        {
            string testAccessToken = "test access token";
            string testTokenType = "test token type";
            string testRefreshToken = "test refresh token";
            string testIdToken = "test id token";
            string testScope = "test scope";
            OktaStateManager testOktaStateManager = new OktaStateManager(testAccessToken, testTokenType, testIdToken, testRefreshToken, 300 /* seconds */, testScope);

            string json = testOktaStateManager.ToJson();
            OktaStateManager deserializedStateManager = JsonConvert.DeserializeObject<OktaStateManager>(json);

            deserializedStateManager.AccessToken.Should().Be(testAccessToken);
            deserializedStateManager.IdToken.Should().Be(testIdToken);
            deserializedStateManager.RefreshToken.Should().Be(testRefreshToken);
            deserializedStateManager.Scope.Should().Be(testScope);
            deserializedStateManager.Expires.Should().NotBeNull();
        }

        [Fact]
        public void WriteToSecureStorage()
        {
            string testAccessToken = "test access token";
            string testTokenType = "test token type";
            string testRefreshToken = "test refresh token";
            string testIdToken = "test id token";
            string testScope = "test scope";
            OktaStateManager testOktaStateManager = new OktaStateManager(testAccessToken, testTokenType, testIdToken, testRefreshToken, 300 /* seconds */, testScope);

            SecureKeyValueStore mockSecureKeyValueStore = Substitute.For<SecureKeyValueStore>();
            OktaContext.RegisterServiceImplementation<SecureKeyValueStore>(mockSecureKeyValueStore);

            testOktaStateManager.WriteToSecureStorageAsync().Wait();

            string json = testOktaStateManager.ToJson();
            mockSecureKeyValueStore.Received().SetAsync(OktaStateManager.StoreKey, json);
        }

        [Fact]
        public void ReadFromSecureStorage()
        {
            string testAccessToken = "test access token";
            string testTokenType = "test token type";
            string testRefreshToken = "test refresh token";
            string testIdToken = "test id token";
            string testScope = "test scope";
            OktaStateManager testOktaStateManagerInSecureStorage = new OktaStateManager(testAccessToken, testTokenType, testIdToken, testRefreshToken, 300 /* seconds */, testScope);

            SecureKeyValueStore mockSecureKeyValueStore = Substitute.For<SecureKeyValueStore>();
            mockSecureKeyValueStore.GetAsync<OktaStateManager>(OktaStateManager.StoreKey).Returns(testOktaStateManagerInSecureStorage);
            IOktaConfig testConfig = Substitute.For<IOktaConfig>();
            IOidcClient testClient = Substitute.For<IOidcClient>();
            OktaContext.RegisterServiceImplementation<SecureKeyValueStore>(mockSecureKeyValueStore);
            OktaContext.RegisterServiceImplementation<IOktaConfig>(testConfig);
            OktaContext.RegisterServiceImplementation<IOidcClient>(testClient);

            OktaStateManager reader = new OktaStateManager();
            OktaStateManager result = reader.ReadFromSecureStorageAsync().Result;
            result.Config.Should().Be(testConfig);
            result.Client.Should().Be(testClient);
            result.AccessToken.Should().BeEquivalentTo(testAccessToken);
            result.TokenType.Should().BeEquivalentTo(testTokenType);
            result.RefreshToken.Should().BeEquivalentTo(testRefreshToken);
            result.IdToken.Should().BeEquivalentTo(testIdToken);
            result.Scope.Should().BeEquivalentTo(testScope);
        }

        [Fact]
        public void CallClientRevokeAccessToken()
        {
            string testAccessToken = "test access token";
            IOidcClient mockClient = Substitute.For<IOidcClient>();
            OktaStateManager oktaStateManager = new OktaStateManager(testAccessToken, "test token type") { Client = mockClient };

            oktaStateManager.RevokeAsync(TokenKind.AccessToken).Wait();

            mockClient.Received().RevokeAccessTokenAsync(testAccessToken);
        }

        [Fact]
        public void CallClientRevokeRefreshToken()
        {
            string testRefreshToken = "test refresh token";
            IOidcClient mockClient = Substitute.For<IOidcClient>();
            OktaStateManager oktaStateManager = new OktaStateManager("test access token", "test token type", "test id token", testRefreshToken) { Client = mockClient };

            oktaStateManager.RevokeAsync(TokenKind.RefreshToken).Wait();

            mockClient.Received().RevokeRefreshTokenAsync(testRefreshToken);
        }

        [Fact]
        public void GetAccessToken()
        {
            string testAccessToken = "this is a test access token";
            OktaStateManager oktaStateManager = new OktaStateManager { AccessToken = testAccessToken };

            string retrieved = oktaStateManager.GetAccessToken();

            retrieved.Should().BeEquivalentTo(testAccessToken);
        }

        [Fact]
        public void GetRefreshToken()
        {
            string testRefreshToken = "this is a test refresh token";
            OktaStateManager oktaStateManager = new OktaStateManager { RefreshToken = testRefreshToken };

            string retrieved = oktaStateManager.GetRefreshToken();

            retrieved.Should().BeEquivalentTo(testRefreshToken);
        }

        [Fact]
        public void GetIdToken()
        {
            string testIdToken = "this is a test id token";
            OktaStateManager oktaStateManager = new OktaStateManager { RefreshToken = testIdToken };

            string retrieved = oktaStateManager.GetRefreshToken();

            retrieved.Should().BeEquivalentTo(testIdToken);
        }
    }
}
