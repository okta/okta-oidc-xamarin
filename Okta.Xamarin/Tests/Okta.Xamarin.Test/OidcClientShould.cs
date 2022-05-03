// <copyright file="OidcClientShould.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Okta.Xamarin.Test
{
    public class OidcClientShould
    {
        [Fact]
        public void FailWithInvalidConfig()
        {
            Assert.Throws<ArgumentNullException>(() => new TestOidcClient(new OktaConfig()));
        }

        [Fact]
        public void GetCorrectAuthUrl()
        {
            TestOidcClient client = new TestOidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect+&TEST!@url%20Encode#*^(0)", "com.test:/logout") { Scope = "test hello test_scope" });

            string url = client.GenerateAuthorizeUrlTest();
            Assert.StartsWith("https://dev-00000.oktapreview.com/oauth2/default/v1/authorize?", url);
            Assert.Contains("redirect_uri=com.test%3A%2Fredirect%2B%26TEST%21%40url%2520Encode%23%2A%5E%280%29", url);
            Assert.Contains("client_id=testoktaid", url);
            Assert.Contains("scope=test%20hello%20test_scope", url);
        }

        [Fact]
        public async void LaunchBrowserCorrectly()
        {
            TestOidcClient client = new TestOidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

            bool didLaunchBrowser = false;

            client.OnLaunchBrowser = new Action<string>(url =>
            {
                Assert.StartsWith("https://dev-00000.oktapreview.com/oauth2/default/v1/authorize?", url);
                didLaunchBrowser = true;
            });

            await Task.WhenAny(client.SignInWithBrowserAsync(), Task.Delay(1000));

            Assert.True(didLaunchBrowser);
        }

        [Fact]
        public async void CloseBrowserCorrectly()
        {
            TestOidcClient client = new TestOidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

            bool didCloseBrowser = false;

            client.OnCloseBrowser = () =>
            {
                didCloseBrowser = true;
            };

            client.OnLaunchBrowser = new Action<string>(url =>
            {
                OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=" + client.State_Internal));
            });

            await Task.WhenAny(client.SignInWithBrowserAsync(), Task.Delay(1000));

            Assert.True(didCloseBrowser);
        }

        [Fact]
        public async void RequestAccessToken()
        {
            TestOidcClient client = new TestOidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

            bool didRequestAccessToken = false;

            HttpMessageHandlerMock mockHttpClient = new HttpMessageHandlerMock();
            mockHttpClient.Responder = (request) =>
            {
                string url = request.Item1;
                Dictionary<string, string> data = request.Item2;

                Assert.StartsWith("https://dev-00000.oktapreview.com/oauth2/default/v1/token", url);
                Assert.Equal("12345", data["code"]);

                didRequestAccessToken = true;

                return new Tuple<System.Net.HttpStatusCode, string>(
                    System.Net.HttpStatusCode.OK,
                    @"{ ""access_token"": ""access_token_example"", ""token_type"": ""testing""}");
            };

            client.SetMockHttpMessageHandler(mockHttpClient);

            client.OnLaunchBrowser = new Action<string>(url =>
            {
                Assert.True(
                    OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=" + client.State_Internal)));
            });

            await client.SignInWithBrowserAsync();

            Assert.True(didRequestAccessToken);
        }

        [Fact]
        public void SuccessfullyGetAccessToken()
        {
            TestOidcClient client = new TestOidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

            HttpMessageHandlerMock mockHttpClient = new HttpMessageHandlerMock();
            mockHttpClient.Responder = (request) =>
            {
                string url = request.Item1;
                Dictionary<string, string> data = request.Item2;

                return new Tuple<System.Net.HttpStatusCode, string>(
                    System.Net.HttpStatusCode.OK,
                    @"{ ""access_token"": ""access_token_example"", ""token_type"": ""testing""}");
            };

            client.SetMockHttpMessageHandler(mockHttpClient);

            client.OnLaunchBrowser = new Action<string>(url =>
                OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=" + client.State_Internal)));

            IOktaStateManager state = client.SignInWithBrowserAsync().Result;

            Assert.Equal("access_token_example", state.AccessToken);

            Assert.True(state.IsAuthenticated);
        }

        [Fact]
        public void CorrectlySetIsAuthenticated()
        {
            OktaStateManager stateWithNoToken = new OktaStateManager(null, "test");
            Assert.False(stateWithNoToken.IsAuthenticated);

            OktaStateManager stateWithNoExp = new OktaStateManager("test", "test");
            Assert.True(stateWithNoExp.IsAuthenticated);
        }

        [Fact]
        public void CorrectlySetIsAccessTokenExpired()
        {
            OktaStateManager stateWithTokenAndExpInPast = new OktaStateManager("test", "test", null, null, -100);
            Assert.True(stateWithTokenAndExpInPast.IsAccessTokenExpired);

            OktaStateManager stateWithExpInFuture = new OktaStateManager("test", "test", null, null, 100);
            Assert.False(stateWithExpInFuture.IsAccessTokenExpired);
        }

        [Fact]
        public async void FailOnStateMismatchInInitialRequest()
        {
            TestOidcClient client = new TestOidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

            client.OnLaunchBrowser = new Action<string>(url =>
            {
                Assert.False(
                    OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=fake_state")));
            });

            await Task.WhenAny(client.SignInWithBrowserAsync(), Task.Delay(1000));
        }

        [Fact]
        public async void FailOnErrorInInitialRequest()
        {
            TestOidcClient client = new TestOidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

            client.OnLaunchBrowser = new Action<string>(url =>
            {
                Assert.True(
                    OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?error=test_error&state=" + client.State_Internal)));
            });

            await Assert.ThrowsAsync<OAuthException>(() => client.SignInWithBrowserAsync());
        }

        [Fact]
        public async void RaiseAuthenticationFailedEvent()
        {
            TestOidcClient client = new TestOidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

            client.OnLaunchBrowser = new Action<string>(url =>
            {
                Assert.True(
                    OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?error=test_error&state=" + client.State_Internal)));
            });

            bool? authenticationFailedEventWasRaised = false;
            OAuthException oAuthException = null;
            OktaContext.Current.AuthenticationFailed += (sender, authenticationFailedEventArgs) =>
            {
                authenticationFailedEventWasRaised = true;
                oAuthException = authenticationFailedEventArgs.OAuthException;
            };

            await OktaContext.Current.SignInAsync(client);

            Assert.True(authenticationFailedEventWasRaised.Value, "AuthenticationFailed event was not raised as expected.");
            Assert.NotNull(oAuthException);
            Assert.Equal(OktaContext.Current.OAuthException, oAuthException);
        }

        [Fact]
        public async void FailOnErrorDataInAccessTokenRequest()
        {
            TestOidcClient client = new TestOidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

            HttpMessageHandlerMock mockHttpClient = new HttpMessageHandlerMock();
            mockHttpClient.Responder = (request) =>
            {
                string url = request.Item1;
                Dictionary<string, string> data = request.Item2;

                return new Tuple<System.Net.HttpStatusCode, string>(
                    System.Net.HttpStatusCode.OK,
                    @"{ ""error"": ""test_failure"", ""token_type"": ""testing""}");
            };

            client.SetMockHttpMessageHandler(mockHttpClient);

            client.OnLaunchBrowser = new Action<string>(url =>
            {
                OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=" + client.State_Internal));
            });

            await Assert.ThrowsAsync<OAuthException>(() => client.SignInWithBrowserAsync());
        }

        [Fact]
        public async void FailGracefullyOnHttpErrorInAccessTokenRequest()
        {
            TestOidcClient client = new TestOidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

            HttpMessageHandlerMock mockHttpClient = new HttpMessageHandlerMock();
            mockHttpClient.Responder = (request) =>
            {
                string url = request.Item1;
                Dictionary<string, string> data = request.Item2;

                return new Tuple<System.Net.HttpStatusCode, string>(
                    System.Net.HttpStatusCode.Forbidden,
                    @"{ ""error"": ""not_authorized"", ""token_type"": ""testing""}");
            };

            client.SetMockHttpMessageHandler(mockHttpClient);

            client.OnLaunchBrowser = new Action<string>(url =>
            {
                OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=" + client.State_Internal));
            });

            await Assert.ThrowsAsync<OAuthException>(() => client.SignInWithBrowserAsync());
        }

        [Fact]
        public void SetStateCodeVerifierAndChallengeOnSignOut()
        {
            TestOidcClient client = new TestOidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));
            Assert.True(string.IsNullOrEmpty(client.State_Internal));
            Assert.True(string.IsNullOrEmpty(client.CodeChallenge_Internal));
            Assert.True(string.IsNullOrEmpty(client.CodeVerifier_Internal));

            client.SignOutOfOktaAsync(new OktaStateManager("testAccessToken", "testTokenType"));

            Assert.False(string.IsNullOrEmpty(client.State_Internal));
            Assert.False(string.IsNullOrEmpty(client.CodeChallenge_Internal));
            Assert.False(string.IsNullOrEmpty(client.CodeVerifier_Internal));
        }

        [Fact]
        public async void LaunchBrowserIfAuthenticatedOnSignOut()
        {
            bool? browserLaunched = false;
            TestOidcClient client = new TestOidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));
            client.OnLaunchBrowser = (url) => browserLaunched = true;
            client.SignOutOfOktaAsync(new OktaStateManager("testAccessToken", "testTokenType"));
            Assert.True(browserLaunched);
        }

        [Fact]
        public void NotLaunchBrowserIfNotAuthenticatedOnSignOut()
        {
            bool? browserLaunched = false;
            TestOidcClient client = new TestOidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));
            client.OnLaunchBrowser = (url) => browserLaunched = true;
            client.SignOutOfOktaAsync(new OktaStateManager(string.Empty, string.Empty));
            Assert.False(browserLaunched);
        }

        [Fact]
        public void UseAuthorizationServerIdFromConfig()
        {
            string testClientId = "test client id";
            string testAuthorizationServerId = "test authorization server id";
            OktaConfig testConfig = new OktaConfig
            {
                OktaDomain = "https://fake.cxm/",
                RedirectUri = "https://fake.cxm/redirect",
                PostLogoutRedirectUri = "https://fake.cxm/logoutRedirect",
                ClientId = testClientId,
                AuthorizationServerId = testAuthorizationServerId,
            };
            bool? requestReceived = false;
            string testResponse = "test response";
            HttpMessageHandlerMock mockHttpMessageHandler = new HttpMessageHandlerMock();
            mockHttpMessageHandler.Responder = (request) =>
            {
                string url = request.Item1;
                Dictionary<string, string> data = request.Item2;

                Assert.Equal($"https://fake.cxm/oauth2/{testAuthorizationServerId}/v1/testPath", url);

                requestReceived = true;

                return new Tuple<System.Net.HttpStatusCode, string>(
                    System.Net.HttpStatusCode.OK, testResponse);
            };
            TestOidcClient testOidcClient = new TestOidcClient(testConfig);
            testOidcClient.SetMockHttpMessageHandler(mockHttpMessageHandler);

            testOidcClient.CallPerformAuthorizationServerRequestAsync(HttpMethod.Post, "/testPath", new Dictionary<string, string>());

            Assert.True(requestReceived);
        }

        [Fact]
        public void UseAuthorizationServerIdFromConfigOnRevokeAccessToken()
        {
            string testClientId = "test client id";
            string testAccessToken = "test access token";
            string testAuthorizationServerId = "test authorization server id";
            OktaConfig testConfig = new OktaConfig
            {
                OktaDomain = "https://fake.cxm/",
                RedirectUri = "https://fake.cxm/redirect",
                PostLogoutRedirectUri = "https://fake.cxm/logoutRedirect",
                ClientId = testClientId,
                AuthorizationServerId = testAuthorizationServerId,
            };
            bool? requestReceived = false;
            string testResponse = "test response";
            HttpMessageHandlerMock mockHttpMessageHandler = new HttpMessageHandlerMock();
            mockHttpMessageHandler.Responder = (request) =>
            {
                string url = request.Item1;
                Dictionary<string, string> data = request.Item2;

                Assert.Equal($"https://fake.cxm/oauth2/{testAuthorizationServerId}/v1/revoke", url);
                Assert.Equal(testAccessToken, data["token"]);
                Assert.Equal("access_token", data["token_type_hint"]);
                Assert.Equal(testClientId, data["client_id"]);
                requestReceived = true;

                return new Tuple<System.Net.HttpStatusCode, string>(
                    System.Net.HttpStatusCode.OK, testResponse);
            };
            TestOidcClient testOidcClient = new TestOidcClient(testConfig);
            testOidcClient.SetMockHttpMessageHandler(mockHttpMessageHandler);

            testOidcClient.RevokeAccessTokenAsync(testAccessToken).Wait();

            Assert.True(requestReceived);
        }

        [Fact]
        public void UseAuthorizationServerIdFromConfigOnRevokeRefreshToken()
        {
            string testClientId = "test client id";
            string testRefreshToken = "test refresh token";
            string testAuthorizationServerId = "test authorization server id";
            OktaConfig testConfig = new OktaConfig
            {
                OktaDomain = "https://fake.cxm/",
                RedirectUri = "https://fake.cxm/redirect",
                PostLogoutRedirectUri = "https://fake.cxm/logoutRedirect",
                ClientId = testClientId,
                AuthorizationServerId = testAuthorizationServerId,
            };
            bool? requestReceived = false;
            string testResponse = "test response";
            HttpMessageHandlerMock mockHttpMessageHandler = new HttpMessageHandlerMock();
            mockHttpMessageHandler.Responder = (request) =>
            {
                string url = request.Item1;
                Dictionary<string, string> data = request.Item2;

                Assert.Equal($"https://fake.cxm/oauth2/{testAuthorizationServerId}/v1/revoke", url);
                Assert.Equal(testRefreshToken, data["token"]);
                Assert.Equal("refresh_token", data["token_type_hint"]);
                Assert.Equal(testClientId, data["client_id"]);
                requestReceived = true;

                return new Tuple<System.Net.HttpStatusCode, string>(
                    System.Net.HttpStatusCode.OK, testResponse);
            };
            TestOidcClient testOidcClient = new TestOidcClient(testConfig);
            testOidcClient.SetMockHttpMessageHandler(mockHttpMessageHandler);

            testOidcClient.RevokeRefreshTokenAsync(testRefreshToken).Wait();

            Assert.True(requestReceived);
        }

        [Fact]
        public void UseDefaultAuthorizationServerIfNullInConfigOnRevokeAccessToken()
        {
            string testAccessToken = "test access token";
            string testClientId = "test client id";
            OktaConfig testConfig = new OktaConfig
            {
                OktaDomain = "https://fake.cxm/",
                RedirectUri = "https://fake.cxm/redirect",
                PostLogoutRedirectUri = "https://fake.cxm/logoutRedirect",
                ClientId = testClientId,
                AuthorizationServerId = null,
            };
            bool? requestReceived = false;
            string testResponse = "test response";
            HttpMessageHandlerMock mockHttpMessageHandler = new HttpMessageHandlerMock();
            mockHttpMessageHandler.Responder = (request) =>
            {
                string url = request.Item1;
                Dictionary<string, string> data = request.Item2;

                Assert.Equal($"https://fake.cxm/oauth2/default/v1/revoke", url);
                Assert.Equal(testAccessToken, data["token"]);
                Assert.Equal("access_token", data["token_type_hint"]);
                Assert.Equal(testClientId, data["client_id"]);
                requestReceived = true;

                return new Tuple<System.Net.HttpStatusCode, string>(
                    System.Net.HttpStatusCode.OK, testResponse);
            };
            TestOidcClient testOidcClient = new TestOidcClient(testConfig);
            testOidcClient.SetMockHttpMessageHandler(mockHttpMessageHandler);

            testOidcClient.RevokeAccessTokenAsync(testAccessToken).Wait();

            Assert.True(requestReceived);
        }

        [Fact]
        public void UseDefaultAuthorizationServerIfNullInConfigOnRevokeRefreshToken()
        {
            string testRefreshToken = "test access token";
            string testClientId = "test client id";
            OktaConfig testConfig = new OktaConfig
            {
                OktaDomain = "https://fake.cxm/",
                RedirectUri = "https://fake.cxm/redirect",
                PostLogoutRedirectUri = "https://fake.cxm/logoutRedirect",
                ClientId = testClientId,
                AuthorizationServerId = null,
            };
            bool? requestReceived = false;
            string testResponse = "test response";
            HttpMessageHandlerMock mockHttpMessageHandler = new HttpMessageHandlerMock();
            mockHttpMessageHandler.Responder = (request) =>
            {
                string url = request.Item1;
                Dictionary<string, string> data = request.Item2;

                Assert.Equal($"https://fake.cxm/oauth2/default/v1/revoke", url);
                Assert.Equal(testRefreshToken, data["token"]);
                Assert.Equal("refresh_token", data["token_type_hint"]);
                Assert.Equal(testClientId, data["client_id"]);
                requestReceived = true;

                return new Tuple<System.Net.HttpStatusCode, string>(
                    System.Net.HttpStatusCode.OK, testResponse);
            };
            TestOidcClient testOidcClient = new TestOidcClient(testConfig);
            testOidcClient.SetMockHttpMessageHandler(mockHttpMessageHandler);

            testOidcClient.RevokeRefreshTokenAsync(testRefreshToken).Wait();

            Assert.True(requestReceived);
        }

        [Fact]
        public void RaiseAuthCodeTokenExchangeExceptionThrownEvent()
        {
            string testClientId = "test client id";
            OktaConfig testConfig = new OktaConfig
            {
                OktaDomain = "https://fake.cxm/",
                RedirectUri = "https://fake.cxm/redirect",
                PostLogoutRedirectUri = "https://fake.cxm/logoutRedirect",
                ClientId = testClientId,
                AuthorizationServerId = null,
            };

            bool? requestReceived = false;
            HttpMessageHandlerMock mockHttpMessageHandler = new HttpMessageHandlerMock();
            mockHttpMessageHandler.GetTestResponse = (request, cancellationToken) =>
            {
                requestReceived = true;
                throw new Exception("test exception to simulate request failure");
            };
            TestOidcClient testOidcClient = new TestOidcClient(testConfig);
            testOidcClient.SetMockHttpMessageHandler(mockHttpMessageHandler);

            bool? eventWasRaised = false;
            testOidcClient.AuthCodeTokenExchangeExceptionThrown += (sender, args) =>
            {
                eventWasRaised = true;
            };

            testOidcClient.CallExchangeCodeForTokenAsync("test code").Wait();
            Assert.True(requestReceived);
            Assert.True(eventWasRaised);
        }

        [Fact]
        public void UseScopeFromConfigOnRenew()
        {
            string testClientId = "test client id";
            string guid = System.Guid.NewGuid().ToString();
            string testScope = $"test scope {guid}";
            RenewResponse testRenewResponse = new RenewResponse { Scope = testScope };

            OktaConfig testConfig = new OktaConfig
            {
                OktaDomain = "https://fake.cxm/",
                RedirectUri = "https://fake.cxm/redirect",
                PostLogoutRedirectUri = "https://fake.cxm/logoutRedirect",
                ClientId = testClientId,
                Scope = testScope,
            };

            bool? requestReceived = false;
            HttpMessageHandlerMock mockHttpMessageHandler = new HttpMessageHandlerMock();
            mockHttpMessageHandler.GetTestResponse = (request, cancellationToken) =>
            {
                string content = request.Content.ReadAsStringAsync().Result;
                Assert.True(content.Equals($"grant_type=refresh_token&redirect_uri=https%3A%2F%2Ffake.cxm%2Fredirect&scope=test+scope+{guid}&refresh_token=test+refresh+token"));
                requestReceived = true;
                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent(JsonConvert.SerializeObject(testRenewResponse)) };
            };
            TestOidcClient testOidcClient = new TestOidcClient(testConfig);
            testOidcClient.SetMockHttpMessageHandler(mockHttpMessageHandler);
            testOidcClient.RenewAsync<RenewResponse>("test refresh token").Wait();
            Assert.True(requestReceived);
        }
    }
}
