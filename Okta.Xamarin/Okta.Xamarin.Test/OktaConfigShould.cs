// <copyright file="OktaConfigShould.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Okta.Xamarin.Test
{
	public class OktaConfigShould
	{
		[Fact]
		public void ConfigValidatorPassesWhenValid()
		{
			OktaConfigValidator<IOktaConfig> validator = new OktaConfigValidator<IOktaConfig>();

			validator.Validate(
				new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout"));

			validator.Validate(
							 new OktaConfig("testoktaid", "https://okta.okta.com", "appid:/redirect", "appid:/logout")
							 { ClockSkew = TimeSpan.FromSeconds(100), Scope = "test1 test2" });

			// The validator throws an exception when the config is invalid, so if we got here without an exception then the configs are valid.
		}

		[Fact]
		public void ConfigValidatorCatchesMissingProperties()
		{
			OktaConfigValidator<OktaConfig> validator = new OktaConfigValidator<OktaConfig>();

			Assert.Throws<ArgumentNullException>("config", () =>
				validator.Validate(null));

			Assert.Throws<ArgumentNullException>(() =>
				validator.Validate(new OktaConfig()));

			Assert.Throws<ArgumentNullException>("ClientId", () =>
				validator.Validate(new OktaConfig(null, "https://dev-00000.oktapreview.com", "com.test:/redirect", "com.test:/logout")));

			Assert.Throws<ArgumentNullException>("OktaDomain", () =>
				validator.Validate(new OktaConfig("testoktaid", null, "com.test:/redirect", "com.test:/logout")));

			Assert.Throws<ArgumentNullException>("RedirectUri", () =>
				validator.Validate(new OktaConfig("testoktaid", "https://dev-00000.oktapreview.com", null, "com.test:/logout")));

			Assert.Throws<ArgumentNullException>("PostLogoutRedirectUri", () =>
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
		public async Task ParseJsonFull()
		{
			var tempConfigFile = new FileInfo(Path.GetTempFileName());

			File.WriteAllText(tempConfigFile.FullName, @"{
				""ClientId"": ""testoktaid"",
				""OktaDomain"": ""https://dev-00000.oktapreview.com"",
				""RedirectUri"": ""com.test:/redirect"",
				""PostLogoutRedirectUri"": ""com.test:/logout"",
				""Scope"": ""test1 test2 test3"",
				""AuthorizationServerId"": ""test1"",
				""ClockSkew"": 90
				}");

			OktaConfig config = await OktaConfig.LoadFromJsonFileAsync(tempConfigFile.FullName);

			Assert.Equal("testoktaid", config.ClientId);
			Assert.Equal("https://dev-00000.oktapreview.com", config.OktaDomain);
			Assert.Equal("com.test:/redirect", config.RedirectUri);
			Assert.Equal("com.test:/logout", config.PostLogoutRedirectUri);
			Assert.Equal("test1 test2 test3", config.Scope);
			Assert.Equal((IEnumerable<string>)(new string[] { "test1", "test2", "test3" }), config.Scopes);
			Assert.Equal("test1", config.AuthorizationServerId);
			Assert.Equal(TimeSpan.FromSeconds(90), config.ClockSkew);

			try
			{
				tempConfigFile.Delete();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to clean up temp file used for testing OktaConfig JSON file parsing at " + tempConfigFile.FullName + Environment.NewLine + ex.ToString());
			}
		}


		[Fact]
		public async Task ParseJsonMinimal()
		{
			var tempConfigFile = new FileInfo(Path.GetTempFileName());

			File.WriteAllText(tempConfigFile.FullName, @"{
				""ClientId"": ""testoktaid"",
				""OktaDomain"": ""https://dev-00000.oktapreview.com"",
				""RedirectUri"": ""com.test:/redirect"",
				""PostLogoutRedirectUri"": ""com.test:/logout""
				}");

			OktaConfig config = await OktaConfig.LoadFromJsonFileAsync(tempConfigFile.FullName);

			Assert.Equal("testoktaid", config.ClientId);
			Assert.Equal("https://dev-00000.oktapreview.com", config.OktaDomain);
			Assert.Equal("com.test:/redirect", config.RedirectUri);
			Assert.Equal("com.test:/logout", config.PostLogoutRedirectUri);
			Assert.Equal("openid profile", config.Scope);
			Assert.Equal((IEnumerable<string>)(new string[] { "openid", "profile" }), config.Scopes);
			Assert.Equal("default", config.AuthorizationServerId);
			Assert.Equal(TimeSpan.FromSeconds(120), config.ClockSkew);

			try
			{
				tempConfigFile.Delete();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to clean up temp file used for testing OktaConfig JSON file parsing at " + tempConfigFile.FullName + Environment.NewLine + ex.ToString());
			}
		}
	}
}
