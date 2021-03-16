// <copyright file="OidcClient.Android.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>


using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

namespace Okta.Xamarin.Android
{
	[Activity(Label = "Okta.Xamarin")]
	public class OktaMainActivity<TApp> : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity where TApp : global::Xamarin.Forms.Application, new()
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			OktaContext.Init(new AndroidOidcClient(this, AndroidOktaConfig.LoadFromXmlStream(Assets.Open("OktaConfig.xml"))));
			OktaContext.Current.SignInCompleted += OnSignInCompleted;
			OktaContext.Current.SignOutCompleted += OnSignOutCompleted;

			base.OnCreate(savedInstanceState);

			global::Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
			LoadApplication(new TApp());
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			global::Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		public virtual void OnSignInCompleted(object sender, SignInEventArgs signInEventArgs)
		{

		}

		public virtual void OnSignOutCompleted(object sender, SignOutEventArgs signOutEventArgs)
		{

		}
	}
}
