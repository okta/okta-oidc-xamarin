// <copyright file="OktaConfig.Android.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Threading.Tasks;
using Android.Content;
using Android.App;

namespace Okta.Xamarin.Android
{
	public class OktaPlatform : OktaPlatformBase
    {
		public static void HandleCallback(Activity callingActivity, Type newActivityType)
		{
			Uri uri = new Uri(callingActivity.Intent.Data.ToString());
			if (OidcClient.InterceptLoginCallback(uri) || OidcClient.InterceptLogoutCallback(uri))
			{
				Intent intent = new Intent(callingActivity, newActivityType);
				intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
				callingActivity.StartActivity(intent);
				callingActivity.Finish();
				return;
			}
		}

		public static async Task<OktaContext> InitAsync(Context context)
		{
			return await InitAsync(context, AndroidOktaConfig.LoadFromXmlStream(context.Assets.Open("OktaConfig.xml")));
		}

		public static async Task<OktaContext> InitAsync(Context context, IOktaConfig config)
		{
			return await InitAsync(new AndroidOidcClient(context, config));
		}
    }
}
