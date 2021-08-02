// <copyright file="OidcClient.Android.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>


using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Okta.Xamarin.TinyIoC;

namespace Okta.Xamarin.Android
{
	[Activity(Label = "Okta.Xamarin")]
	public class OktaMainActivity<TApp> : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity where TApp : global::Xamarin.Forms.Application, new()
	{
		public OktaMainActivity()
		{
			OktaContainer = new TinyIoCContainer();
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			OktaContext.Current.SignInCompleted += HandleSignInCompleted;
			OktaContext.Current.SignOutCompleted += HandleSignOutCompleted;

			IOktaConfig oktaConfig = AndroidOktaConfig.LoadFromXmlStream(Assets.Open("OktaConfig.xml"));
			OktaContext.RegisterOktaDefaults(OktaContainer);
			OktaContainer.Register(oktaConfig);
			OktaContainer.Register<IOidcClient>(new AndroidOidcClient(this, oktaConfig));

			RegisterOktaServices(); // ensures that consumer provided services are registered by calling overridable RegisterOktaServices method

			OktaContext.Current.InitServices(OktaContainer);

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

		private void HandleSignInCompleted(object sender, SignInEventArgs signInEventArgs)
		{
			// future Okta specific updates go here
			this.OnSignInCompleted(sender, signInEventArgs);
		}

		/// <summary>
		/// Override this method if you wish to execute custom code when sign in completes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="signInEventArgs"></param>
		public virtual void OnSignInCompleted(object sender, SignInEventArgs signInEventArgs)
		{
			// extenders may override this method to execute code when sign in completes
		}

		private void HandleSignOutCompleted(object sender, SignOutEventArgs signOutEventArgs)
		{
			// future Okta specific updates go here
			this.OnSignOutCompleted(sender, signOutEventArgs);
		}

		/// <summary>
		/// Override this method if you wish to execute custom code when sign out completes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="signOutEventArgs"></param>
		public virtual void OnSignOutCompleted(object sender, SignOutEventArgs signOutEventArgs)
		{
			// extenders may override this method to execute code when sign out completes.
		}

		protected TinyIoCContainer OktaContainer { get; }

		private void RegisterOktaServices()
		{
			RegisterOktaServices(OktaContainer);
		}

		/// <summary>
		/// Override this mehod if you wish to modify the content of the inversion of control container.
		/// </summary>
		/// <param name="iocContainer"></param>
		protected virtual void RegisterOktaServices(TinyIoCContainer iocContainer)
		{
			// extenders may override this method to register their services with the IoCContainer
		}
	}
}
