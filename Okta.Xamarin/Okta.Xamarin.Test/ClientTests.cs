using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit;



namespace Okta.Xamarin.Test
{
	public class ClientTests
	{


		[Fact]
		public async void FailsWithInvalidConfig()
		{
			Assert.Throws<ArgumentNullException>(() => new OidcClient(new OktaConfig()));
		}

		[Fact]
		public async void GetsCorrectAuthUrl()
		{
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect+&TEST!@url%20Encode#*^(0)", "com.test:/logout") { Scope = "test hello test_scope" });

			string url = client.GenerateAuthorizeUrlTest();
			Assert.StartsWith("https://dev-00000.oktapreview.com/oauth2/default/v1/authorize?", url);
			Assert.Contains("redirect_uri=com.test%3A%2Fredirect%2B%26TEST%21%40url%2520Encode%23%2A%5E%280%29", url);
			Assert.Contains("client_id=testoktaid", url);
			Assert.Contains("scope=test%20hello%20test_scope", url);
		}

		[Fact]
		public async void LaunchesBrowserCorrectly()
		{
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

			bool didLaunchBrowser = false;

			client.onLaunchBrowser = new Action<string>(url =>
			{
				Assert.StartsWith("https://dev-00000.oktapreview.com/oauth2/default/v1/authorize?", url);
				didLaunchBrowser = true;
			});

			await Task.WhenAny(client.SignInWithBrowserAsync(), Task.Delay(1000));

			Assert.True(didLaunchBrowser);
		}

		[Fact]
		public async void ClosesBrowserCorrectly()
		{
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

			bool didCloseBrowser = false;

			client.onCloseBrowser = () =>
			{
				didCloseBrowser = true;
			};

			client.onLaunchBrowser = new Action<string>(url =>
			{
				OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=" + client.State_Internal));
			});

			await Task.WhenAny(client.SignInWithBrowserAsync(), Task.Delay(1000));

			Assert.True(didCloseBrowser);
		}

		[Fact]
		public async void RequestsAccessToken()
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

			client.onLaunchBrowser = new Action<string>(url =>
			{
				Assert.True(
					OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=" + client.State_Internal)));
			});

			await client.SignInWithBrowserAsync();

			Assert.True(didRequestAccessToken);
		}

		[Fact]
		public async void SuccessfullyGetsAccessToken()
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

			client.onLaunchBrowser = new Action<string>(url =>
				OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=" + client.State_Internal)));

			var state = await client.SignInWithBrowserAsync();

			Assert.Equal("access_token_example", state.AccessToken);
		}



		[Fact]
		public async void FailsOnStateMismatchInInitialRequest()
		{
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

			client.onLaunchBrowser = new Action<string>(url =>
			{
				Assert.False(
					OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=fake_state")));
			});

			await Task.WhenAny(client.SignInWithBrowserAsync(), Task.Delay(1000));
		}

		[Fact]
		public async void FailsOnErrorInInitialRequest()
		{
			OidcClient client = new OidcClient(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

			client.onLaunchBrowser = new Action<string>(url =>
			{
				Assert.True(
					OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?error=test_error&state=" + client.State_Internal)));
			});

			await Assert.ThrowsAsync<OAuthException>(() => client.SignInWithBrowserAsync());
		}

		[Fact]
		public async void FailsOnErrorDataInAccessTokenRequest()
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

			client.onLaunchBrowser = new Action<string>(url =>
			{
				OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=" + client.State_Internal));
			});

			await Assert.ThrowsAsync<OAuthException>(() => client.SignInWithBrowserAsync());
		}

		[Fact]
		public async void FailsGracefullyOnHttpErrorInAccessTokenRequest()
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

			client.onLaunchBrowser = new Action<string>(url =>
			{
				OidcClient.CaptureRedirectUrl(new Uri(client.Config.RedirectUri + "?code=12345&state=" + client.State_Internal));
			});

			await Assert.ThrowsAsync<OAuthException>(() => client.SignInWithBrowserAsync());
		}
	}

}
