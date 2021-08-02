// <copyright file="OktaConfig.iOS.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>



using Foundation;
using Okta.Xamarin.Services;
using Okta.Xamarin.TinyIoC;
using UIKit;

namespace Okta.Xamarin.iOS
{
	/// <summary>
	/// Okta specific app delegate that loads an instance of the specified generic application type TApp and initializes OktaContext when finished launching.
	/// </summary>
	/// <typeparam name="TApp"></typeparam>
	public class OktaAppDelegate<TApp> : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate where TApp: global::Xamarin.Forms.Application, new()
	{
		public OktaAppDelegate()
		{
			OktaContainer = new TinyIoCContainer();
		}

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
			global::Xamarin.Forms.Forms.Init();
			LoadApplication(new TApp());

			bool result = base.FinishedLaunching(app, options);

			OktaContext.Current.SignInCompleted += HandleSignInCompleted;
			OktaContext.Current.SignOutCompleted += HandleSignOutCompleted;

			IOktaConfig oktaConfig = iOsOktaConfig.LoadFromPList("OktaConfig.plist");
			OktaContext.RegisterOktaDefaults(OktaContainer);
			OktaContainer.Register(oktaConfig);
			OktaContainer.Register<IOidcClient>(new iOsOidcClient(Window.RootViewController, oktaConfig));

			RegisterOktaServices(); // ensures that consumer provided services are registered by calling overridable RegisterOktaServices method

			OktaContext.Current.InitServices(OktaContainer);

			return result;
		}

		public override bool OpenUrl
		(
			UIApplication application,
			NSUrl url,
			string sourceApplication,
			NSObject annotation
		)
		{
			return iOsOidcClient.IsOktaCallback(application, url, sourceApplication, annotation);
		}

		#region Duplicated code
		// Due to Xamarin Forms requirement that LoadApplication MUST be called before FinishedLaunching
		// the methods below are duplicated from the OktaAppDelegate class.  If OktaAppDelegate<TApp> extends
		// OktaAppDelegate, the call to base.FinishedLaunching results in the Forms.SetFlags and Forms.Init 
		// methods being called twice; this causes an exception to be thrown.

		protected void HandleSignInCompleted(object sender, SignInEventArgs signInEventArgs)
		{
			// future Okta specific updates go here
			this.OnSignInCompleted(sender, signInEventArgs);
		}

		public virtual void OnSignInCompleted(object sender, SignInEventArgs signInEventArgs)
		{
			// extenders may override this method to execute code when sign in completes
		}

		protected void HandleSignOutCompleted(object sender, SignOutEventArgs signOutEventArgs)
		{
			// future Okta specific updates go here
			this.OnSignOutCompleted(sender, signOutEventArgs);
		}

		public virtual void OnSignOutCompleted(object sender, SignOutEventArgs signOutEventArgs)
		{
			// extenders may override this method to execute code when sign out completes.
		}

		protected TinyIoCContainer OktaContainer { get; }

		protected void RegisterOktaServices()
		{
			RegisterOktaServices(OktaContainer);
		}

		protected virtual void RegisterOktaServices(TinyIoCContainer iocContainer)
		{
			// extenders may override this method to register their services with the IoCContainer
		}
		#endregion
	}

	/// <summary>
	/// Okta specific app delegate that initializes OktaContext when finished launching.
	/// </summary>
	public class OktaAppDelegate: global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate // The use case is not clear for this class, should deprecate in a later release.  
	{        
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
			global::Xamarin.Forms.Forms.Init();			

			bool result = base.FinishedLaunching(app, options);

			OktaContext.Current.SignInCompleted += HandleSignInCompleted;
			OktaContext.Current.SignOutCompleted += HandleSignOutCompleted;

			IOktaConfig oktaConfig = iOsOktaConfig.LoadFromPList("Oktaconfig.plist");
			OktaContext.RegisterOktaDefaults(OktaContainer);
			OktaContainer.Register(oktaConfig);
			OktaContainer.Register<IOidcClient>(new iOsOidcClient(Window.RootViewController, oktaConfig));

			RegisterOktaServices(); // ensures that consumer provided services are registered by calling overridable RegisterOktaServices method
			OktaContext.Current.InitServices(OktaContainer);

			return result;
		}

		public override bool OpenUrl
		(
			UIApplication application,
			NSUrl url,
			string sourceApplication,
			NSObject annotation
		)
		{
			return iOsOidcClient.IsOktaCallback(application, url, sourceApplication, annotation);
		}

		protected void HandleSignInCompleted(object sender, SignInEventArgs signInEventArgs)
		{
			// future Okta specific updates go here
			this.OnSignInCompleted(sender, signInEventArgs);
		}

		public virtual void OnSignInCompleted(object sender, SignInEventArgs signInEventArgs)
		{
			// extenders may override this method to execute code when sign in completes
		}

		protected void HandleSignOutCompleted(object sender, SignOutEventArgs signOutEventArgs)
		{
			// future Okta specific updates go here
			this.OnSignOutCompleted(sender, signOutEventArgs);
		}

		public virtual void OnSignOutCompleted(object sender, SignOutEventArgs signOutEventArgs)
		{
			// extenders may override this method to execute code when sign out completes.
		}

		protected TinyIoCContainer OktaContainer { get; }

		protected void RegisterOktaServices()
		{
			RegisterOktaServices(OktaContainer);
		}

		protected virtual void RegisterOktaServices(TinyIoCContainer iocContainer)
		{
			// extenders may override this method to register their services with the IoCContainer
		}
	}
}