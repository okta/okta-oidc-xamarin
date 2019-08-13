// <copyright file="OAuthException.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;

namespace Okta.Xamarin
{
	/// <summary>
	/// Represents an error that occurred during OAuth login
	/// </summary>
	public class OAuthException : Exception
	{
		/// <summary>
		/// The title of error that occurred
		/// </summary>
		public string ErrorTitle { get; set; }

		/// <summary>
		/// A more detailed description of the error, if provided
		/// </summary>
		public string ErrorDescription { get; set; }

		/// <summary>
		/// If this error was the result of an HTTP response, then this is the HTTP status code of that response
		/// </summary>
		public int? HTTPStatusCode { get; set; }

		/// <summary>
		/// If this error was the result of an HTTP request, then this is the URL of that request
		/// </summary>
		public string RequestUrl { get; set; }

		/// <summary>
		/// Additional relevant data to the exception, such as querystring or header data
		/// </summary>
		public IEnumerable<KeyValuePair<string, string>> ExtraData { get; set; }

		public override string ToString()
		{
			string errorText = base.ToString() + Environment.NewLine +
				"ErrorTitle: " + ErrorTitle ?? "<none>" + Environment.NewLine +
				"ErrorDescription: " + ErrorDescription ?? "<none>" + Environment.NewLine +
				"HTTPStatusCode: " + (HTTPStatusCode?.ToString() ?? "<none>") + Environment.NewLine +
				"RequestUrl: " + RequestUrl ?? "<none>";

			if (ExtraData != null)
				foreach (var kv in ExtraData)
				{
					errorText += Environment.NewLine + kv.Key + ": " + kv.Value;
				}

			return errorText;
		}
	}
}
