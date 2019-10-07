// <copyright file="OidcClientShould.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Okta.Xamarin.Test
{
	public class OidcClientShould
	{
		[Fact]
		public void FailWithInvalidConfig()
		{
			Assert.Throws<ArgumentNullException>(() => new OidcClient(new OktaConfig()));
		}

		[Fact]
		public void GetCorrectAuthUrl()
		{
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect+&TEST!@url%20Encode#*^(0)", "com.test:/logout") { Scope = "test hello test_scope" });

			string url = client.GenerateAuthorizeUrlTest();
			Assert.StartsWith("https://dev-00000.oktapreview.com/oauth2/default/v1/authorize?", url);
			Assert.Contains("redirect_uri=com.test%3A%2Fredirect%2B%26TEST%21%40url%2520Encode%23%2A%5E%280%29", url);
			Assert.Contains("client_id=testoktaid", url);
			Assert.Contains("scope=test%20hello%20test_scope", url);
		}

		[Fact]
		public async void LaunchBrowserCorrectly()
		{
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

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
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

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
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

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

			client.SetHttpMock(mockHttpClient);

			client.OnLaunchBrowser = new Action<string>(url =>
			{
				Assert.True(
					OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=" + client.State_Internal)));
			});

			await client.SignInWithBrowserAsync();

			Assert.True(didRequestAccessToken);
		}

		[Fact]
		public async void SuccessfullyGetAccessToken()
		{
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

			HttpMessageHandlerMock mockHttpClient = new HttpMessageHandlerMock();
			mockHttpClient.Responder = (request) =>
			{
				string url = request.Item1;
				Dictionary<string, string> data = request.Item2;

				return new Tuple<System.Net.HttpStatusCode, string>(
					System.Net.HttpStatusCode.OK,
					@"{ ""access_token"": ""access_token_example"", ""token_type"": ""testing""}");
			};

			client.SetHttpMock(mockHttpClient);

			client.OnLaunchBrowser = new Action<string>(url =>
				OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=" + client.State_Internal)));

			StateManager state = await client.SignInWithBrowserAsync();

			Assert.Equal("access_token_example", state.AccessToken);

			Assert.True(state.IsAuthenticated);
		}

		[Fact]
		public void CorrectlySetIsAuthenticated()
		{
			StateManager stateWithNoToken = new StateManager(null, "test");
			Assert.False(stateWithNoToken.IsAuthenticated);

			StateManager stateWithTokenAndExpInPast = new StateManager("test", "test", null, null, -100);
			Assert.False(stateWithTokenAndExpInPast.IsAuthenticated);

			StateManager stateWithNoExp = new StateManager("test", "test");
			Assert.True(stateWithNoExp.IsAuthenticated);

			StateManager stateWithExpInFuture = new StateManager("test", "test", null, null, 100);
			Assert.True(stateWithExpInFuture.IsAuthenticated);
		}

		[Fact]
		public async void FailOnStateMismatchInInitialRequest()
		{
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

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
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

			client.OnLaunchBrowser = new Action<string>(url =>
			{
				Assert.True(
					OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?error=test_error&state=" + client.State_Internal)));
			});

			await Assert.ThrowsAsync<OAuthException>(() => client.SignInWithBrowserAsync());
		}

		[Fact]
		public async void FailOnErrorDataInAccessTokenRequest()
		{
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

			HttpMessageHandlerMock mockHttpClient = new HttpMessageHandlerMock();
			mockHttpClient.Responder = (request) =>
			{
				string url = request.Item1;
				Dictionary<string, string> data = request.Item2;

				return new Tuple<System.Net.HttpStatusCode, string>(
					System.Net.HttpStatusCode.OK,
					@"{ ""error"": ""test_failure"", ""token_type"": ""testing""}");
			};

			client.SetHttpMock(mockHttpClient);

			client.OnLaunchBrowser = new Action<string>(url =>
			{
				OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=" + client.State_Internal));
			});

			await Assert.ThrowsAsync<OAuthException>(() => client.SignInWithBrowserAsync());
		}

		[Fact]
		public async void FailGracefullyOnHttpErrorInAccessTokenRequest()
		{
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

			HttpMessageHandlerMock mockHttpClient = new HttpMessageHandlerMock();
			mockHttpClient.Responder = (request) =>
			{
				string url = request.Item1;
				Dictionary<string, string> data = request.Item2;

				return new Tuple<System.Net.HttpStatusCode, string>(
					System.Net.HttpStatusCode.Forbidden,
					@"{ ""error"": ""not_authorized"", ""token_type"": ""testing""}");
			};

			client.SetHttpMock(mockHttpClient);

			client.OnLaunchBrowser = new Action<string>(url =>
			{
				OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=" + client.State_Internal));
			});

			await Assert.ThrowsAsync<OAuthException>(() => client.SignInWithBrowserAsync());
		}
	}
}
