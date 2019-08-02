using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Content.PM;
using Xamarin.Essentials;

namespace Okta.Xamarin.Android.Example
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	public class MainActivity : AppCompatActivity
	{

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			Platform.Init(this, savedInstanceState);
			SetContentView(Resource.Layout.activity_main);

			Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);

			FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
			fab.Click += FabOnClick;
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.menu_main, menu);
			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			int id = item.ItemId;
			if (id == Resource.Id.action_settings)
			{
				return true;
			}

			return base.OnOptionsItemSelected(item);
		}

		private async void FabOnClick(object sender, EventArgs eventArgs)
		{
			View view = (View) sender;

			OidcClient client = new OidcClient(this, await OktaConfig.LoadFromXmlStreamAsync(Assets.Open("OktaConfig.xml")));
			var res = await client.SignInWithBrowserAsync();
			res.AccessToken.Clone();
		}
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}

