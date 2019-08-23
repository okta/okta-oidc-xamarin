// <copyright file="OidcClientShould.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Okta.Xamarin.Test
{
	[TestClass]
	public class OidcClientShould
	{
		[TestMethod]
		public void FailWithInvalidConfig()
		{
			Assert.ThrowsException<ArgumentNullException>(() => new OidcClient(new OktaConfig()));
		}

		[TestMethod]
		public void GetCorrectAuthUrl()
		{
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect+&TEST!@url%20Encode#*^(0)", "com.test:/logout") { Scope = "test hello test_scope" });

			string url = client.GenerateAuthorizeUrlTest();
			Assert.IsTrue(url.StartsWith("https://dev-00000.oktapreview.com/oauth2/default/v1/authorize?"));
			Assert.IsTrue(url.Contains("redirect_uri=com.test%3A%2Fredirect%2B%26TEST%21%40url%2520Encode%23%2A%5E%280%29"));
			Assert.IsTrue(url.Contains("client_id=testoktaid"));
			Assert.IsTrue(url.Contains("scope=test%20hello%20test_scope"));
		}

		[TestMethod]
		public void LaunchBrowserCorrectly()
		{
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

			bool didLaunchBrowser = false;

			client.OnLaunchBrowser = new Action<string>(url =>
			{
				Assert.IsTrue(url.StartsWith("https://dev-00000.oktapreview.com/oauth2/default/v1/authorize?"));
				didLaunchBrowser = true;
			});

			var res = Task.WhenAny(client.SignInWithBrowserAsync(), Task.Delay(1000)).Result;

			Assert.IsTrue(didLaunchBrowser);
		}

		[TestMethod]
		public void CloseBrowserCorrectly()
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

			var res = Task.WhenAny(client.SignInWithBrowserAsync(), Task.Delay(1000)).Result;

			Assert.IsTrue(didCloseBrowser);
		}

		[TestMethod]
		public  void RequestAccessToken()
		{
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

			bool didRequestAccessToken = false;

			HttpMessageHandlerMock mockHttpClient = new HttpMessageHandlerMock();
			mockHttpClient.Responder = (request) =>
			{
				string url = request.Item1;
				Dictionary<string, string> data = request.Item2;

				Assert.IsTrue(url.StartsWith("https://dev-00000.oktapreview.com/oauth2/default/v1/token"));
				Assert.AreEqual("12345", data["code"]);

				didRequestAccessToken = true;

				return new Tuple<System.Net.HttpStatusCode, string>(
					System.Net.HttpStatusCode.OK,
					@"{ ""access_token"": ""access_token_example"", ""token_type"": ""testing""}");
			};

			client.SetHttpMock(mockHttpClient);

			client.OnLaunchBrowser = new Action<string>(url =>
			{
				Assert.IsTrue(
					OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=" + client.State_Internal)));
			});

			var res = client.SignInWithBrowserAsync().Result;

			Assert.IsTrue(didRequestAccessToken);
		}

		[TestMethod]
		public  void SuccessfullyGetAccessToken()
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

			StateManager state = client.SignInWithBrowserAsync().Result;

			Assert.AreEqual("access_token_example", state.AccessToken);
		}

		[TestMethod]
		public void FailOnStateMismatchInInitialRequest()
		{
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

			client.OnLaunchBrowser = new Action<string>(url =>
			{
				Assert.IsFalse(
					OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=fake_state")));
			});

			var res = Task.WhenAny(client.SignInWithBrowserAsync(), Task.Delay(1000)).Result;
		}

		[TestMethod]
		public  void FailOnErrorInInitialRequest()
		{
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

			client.OnLaunchBrowser = new Action<string>(url =>
			{
				Assert.IsTrue(
					OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?error=test_error&state=" + client.State_Internal)));
			});

			var res = Assert.ThrowsExceptionAsync<OAuthException>(() => client.SignInWithBrowserAsync()).Result;
		}

		[TestMethod]
		public void FailOnErrorDataInAccessTokenRequest()
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

			var res = Assert.ThrowsExceptionAsync<OAuthException>(() => client.SignInWithBrowserAsync()).Result;
		}

		[TestMethod]
		public void FailGracefullyOnHttpErrorInAccessTokenRequest()
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

			var res = Assert.ThrowsExceptionAsync<OAuthException>(() => client.SignInWithBrowserAsync()).Result;
		}
	}
}
