// <copyright file="OktaConfigValidator.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Text.RegularExpressions;

namespace Okta.Xamarin
{
	/// <summary>
	/// Validates an <see cref="IOktaConfig"/> config to ensure all required fields are present and in the correct form
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class OktaConfigValidator<T>
		where T : IOktaConfig
	{
		/// <summary>
		/// Can be overriden to validate config fields specific to a derived class
		/// </summary>
		/// <param name="config">The config object to validate</param>
		protected virtual void ValidateInternal(T config)
		{
			return;
		}

		/// <summary>
		/// Validates all fields in this config object and throws an exception if anything is wrong.
		/// </summary>
		/// <param name="config">The config object to validate</param>
		public void Validate(IOktaConfig config)
		{
			if (config == null)
			{
				throw new ArgumentNullException(nameof(config));
			}

			if (string.IsNullOrEmpty(config.OktaDomain))
			{
				throw new ArgumentNullException(
					nameof(config.OktaDomain),
					"Your Okta URL is missing. You can copy your domain from the Okta Developer Console. Follow these instructions to find it: https://bit.ly/finding-okta-domain");
			}

			if (string.IsNullOrEmpty(config.ClientId))
			{
				throw new ArgumentNullException(
					nameof(config.ClientId),
					"Your Okta ClientId is missing. You can copy your ClientId from the Okta Developer Console.");
			}

			if (string.IsNullOrEmpty(config.RedirectUri))
			{
				throw new ArgumentNullException(
					nameof(config.RedirectUri),
					"Your RedirectUri is missing. This is typically something like \"{ yourAppScheme }://callback\", and should match your scheme/intent settings for your mobile project.");
			}

			if (string.IsNullOrEmpty(config.PostLogoutRedirectUri))
			{
				throw new ArgumentNullException(
					nameof(config.PostLogoutRedirectUri),
					"Your PostLogoutRedirectUri is missing. This is typically something like \"{ yourAppScheme }://logout\", and should match your scheme/intent settings for your mobile project.");
			}


			if (!config.OktaDomain.StartsWith("https://"))
			{
				throw new ArgumentException(
					$"Your Okta URL must start with https. Current value: {config.OktaDomain}. You can copy your domain from the Okta Developer Console. Follow these instructions to find it: https://bit.ly/finding-okta-domain",
					nameof(config.OktaDomain));
			}

			if (config.OktaDomain.IndexOf("{yourOktaDomain}", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				throw new ArgumentException(
					"Replace {yourOktaDomain} with your Okta domain. You can copy your domain from the Okta Developer Console. Follow these instructions to find it: https://bit.ly/finding-okta-domain", nameof(config.OktaDomain));
			}

			if (config.OktaDomain.IndexOf("-admin.oktapreview.com", StringComparison.OrdinalIgnoreCase) >= 0 ||
				config.OktaDomain.IndexOf("-admin.okta.com", StringComparison.OrdinalIgnoreCase) >= 0 ||
				config.OktaDomain.IndexOf("-admin.okta-emea.com", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				throw new ArgumentException(
					$"Your Okta domain should not contain -admin. Current value: {config.OktaDomain}. You can copy your domain from the Okta Developer Console. Follow these instructions to find it: https://bit.ly/finding-okta-domain", nameof(config.OktaDomain));
			}

			if (config.OktaDomain.IndexOf(".com.com", StringComparison.OrdinalIgnoreCase) >= 0 || Regex.Matches(config.OktaDomain, "://").Count != 1)
			{
				throw new ArgumentException(
					$"It looks like there's a typo in your Okta domain. Current value: {config.OktaDomain}. You can copy your domain from the Okta Developer Console. Follow these instructions to find it: https://bit.ly/finding-okta-domain", nameof(config.OktaDomain));
			}

			ValidateInternal((T)config);
		}

	}
}
