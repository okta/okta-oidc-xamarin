// <copyright file="OidcClient.Android.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace Okta.Xamarin.Android
{
	[Obsolete("Use OktaPlatform.HandleCallback() instead.")]
	[Activity(Label = "OktaLogoutCallbackInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleInstance)]
	public class OktaLogoutCallbackInterceptorActivity<TMain> : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			global::Android.Net.Uri uri_android = Intent.Data;

			if (OidcClient.InterceptLogoutCallback(new Uri(uri_android.ToString())))
			{
				var intent = new Intent(this, typeof(TMain));
				intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
				StartActivity(intent);
				this.Finish();
			}

			return;
		}
	}
}