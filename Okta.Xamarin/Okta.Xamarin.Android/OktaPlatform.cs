using Okta.Xamarin.TinyIoC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Android.Content;

namespace Okta.Xamarin.Android
{
    public class OktaPlatform : OktaPlatformBase
    {

		public static async Task<OktaContext> InitAsync(Context context, IOktaConfig config)
		{
			return await InitAsync(new AndroidOidcClient(context, config));
		}
    }
}
