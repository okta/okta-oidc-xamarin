using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Okta.Xamarin.Oie.Configuration;
using Okta.Xamarin.Oie.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Okta.Xamarin.Droid.Identity
{
	public class AndroidIdentityClientConfigurationProvider : IIdentityClientConfigurationProvider
	{
		public IdentityClientConfiguration GetConfiguration()
		{
			throw new NotImplementedException();
		}
	}
}