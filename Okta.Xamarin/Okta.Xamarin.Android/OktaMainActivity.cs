using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Xamarin.Forms;

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
			// for demo purposes go to the profile page
			Shell.Current.GoToAsync("//ProfilePage", true);
		}

		public virtual void OnSignOutCompleted(object sender, SignOutEventArgs signOutEventArgs)
		{
			// for demo purposes go to the profile page
			Shell.Current.GoToAsync("//ProfilePage", true);
		}
    }
}
