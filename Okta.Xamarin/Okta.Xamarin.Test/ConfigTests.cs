using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit;



namespace Okta.Xamarin.Test
{
	public class ConfigTests
	{
		[Fact]
		public void ConfigValidatorPassesWhenValid()
		{
			OktaConfigValidator<OktaConfig> validator = new OktaConfigValidator<OktaConfig>();

			validator.Validate(
				new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

			validator.Validate(
							new OktaConfig("testoktaid", "https://okta.okta.com", "appid:/redirect", "appid:/logout")
							{ ClockSkew = TimeSpan.FromSeconds(100), Scope = "test1 test2", GetClaimsFromUserInfoEndpoint = true });

			// The validator throws an exception when the config is invalid, so if we got here without an exception then the configs are valid.
		}

		[Fact]
		public void ConfigValidatorCatchesMissingProperties()
		{
			OktaConfigValidator<OktaConfig> validator = new OktaConfigValidator<OktaConfig>();

			Assert.Throws<ArgumentNullException>(() =>
				validator.Validate(new OktaConfig()));

			Assert.Throws<ArgumentNullException>(() =>
				 validator.Validate(new OktaConfig(null, "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout")));

			Assert.Throws<ArgumentNullException>(() =>
				 validator.Validate(new OktaConfig("testoktaid", null, "com.test:/redirect", "com.test:/logout")));

			Assert.Throws<ArgumentNullException>(() =>
				 validator.Validate(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", null, "com.test:/logout")));

			Assert.Throws<ArgumentNullException>(() =>
				 validator.Validate(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", null)));
		}


		[Fact]
		public void ConfigValidatorCatchesInvalidDomain()
		{
			OktaConfigValidator<OktaConfig> validator = new OktaConfigValidator<OktaConfig>();


			Assert.Throws<ArgumentException>(() =>
				 validator.Validate(new OktaConfig("testoktaid", "http://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout")));

			Assert.Throws<ArgumentException>(() =>
				 validator.Validate(new OktaConfig("testoktaid", "https://dev-00000-admin.oktapreview.com", "com.test:/redirect", "com.test:/logout")));

			Assert.Throws<ArgumentException>(() =>
				 validator.Validate(new OktaConfig("testoktaid", "https://{yourOktaDomain}", "com.test:/redirect", "com.test:/logout")));

			Assert.Throws<ArgumentException>(() =>
				 validator.Validate(new OktaConfig("testoktaid", "dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout")));
		}

		[Fact]
		public async Task ConfigParsesJsonFull()
		{
			var f = new FileInfo(Path.GetTempFileName());

			File.WriteAllText(f.FullName, @"{
				  ""ClientId"": ""testoktaid"",
				  ""OktaDomain"": ""https://dev-00000.oktapreview.com"",
				  ""RedirectUri"": ""com.test:/redirect"",
				  ""PostLogoutRedirectUri"": ""com.test:/logout"",
				  ""CallbackPath"": ""/test/callback"",
				  ""Scope"": ""test1 test2 test3"",
				  ""GetClaimsFromUserInfoEndpoint"": true,
				  ""AuthorizationServerId"": ""test1"",
				  ""ClockSkew"": 90
				}");

			OktaConfig config = await OktaConfig.LoadfromJsonFileAsync(f.FullName);

			Assert.Equal("testoktaid", config.ClientId);
			Assert.Equal("https://dev-00000.oktapreview.com", config.OktaDomain);
			Assert.Equal("com.test:/redirect", config.RedirectUri);
			Assert.Equal("com.test:/logout", config.PostLogoutRedirectUri);
			Assert.Equal("/test/callback", config.CallbackPath);
			Assert.Equal("test1 test2 test3", config.Scope);
			Assert.Equal((IEnumerable<string>)(new string[] { "test1", "test2", "test3" }), config.Scopes);
			Assert.True(config.GetClaimsFromUserInfoEndpoint);
			Assert.Equal("test1", config.AuthorizationServerId);
			Assert.Equal(TimeSpan.FromSeconds(90), config.ClockSkew);

			try
			{
				f.Delete();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to clean up temp file used for testing OktaConfig JSON file parsing at " + f.FullName + Environment.NewLine + ex.ToString());
			}
		}


		[Fact]
		public async Task ConfigParsesJsonMinimal()
		{
			var f = new FileInfo(Path.GetTempFileName());

			File.WriteAllText(f.FullName, @"{
				  ""ClientId"": ""testoktaid"",
				  ""OktaDomain"": ""https://dev-00000.oktapreview.com"",
				  ""RedirectUri"": ""com.test:/redirect"",
				  ""PostLogoutRedirectUri"": ""com.test:/logout""
				}");

			OktaConfig config = await OktaConfig.LoadfromJsonFileAsync(f.FullName);

			Assert.Equal("testoktaid", config.ClientId);
			Assert.Equal("https://dev-00000.oktapreview.com", config.OktaDomain);
			Assert.Equal("com.test:/redirect", config.RedirectUri);
			Assert.Equal("com.test:/logout", config.PostLogoutRedirectUri);
			Assert.Equal("/authorization-code/callback", config.CallbackPath);
			Assert.Equal("openid profile", config.Scope);
			Assert.Equal((IEnumerable<string>)(new string[] { "openid", "profile" }), config.Scopes);
			Assert.False(config.GetClaimsFromUserInfoEndpoint);
			Assert.Equal("default", config.AuthorizationServerId);
			Assert.Equal(TimeSpan.FromSeconds(120), config.ClockSkew);

			try
			{
				f.Delete();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to clean up temp file used for testing OktaConfig JSON file parsing at " + f.FullName + Environment.NewLine + ex.ToString());
			}
		}
	}
}
