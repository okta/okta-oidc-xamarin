// <copyright file="OktaMiddlewareExtensions.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;

namespace Okta.Xamarin
{
	public class OAuthException : Exception
	{
		public string ErrorTitle { get; set; }
		public string ErrorDescription { get; set; }
		public int? HTTPStatusCode { get; set; }
		public string RequestUrl { get; set; }
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
