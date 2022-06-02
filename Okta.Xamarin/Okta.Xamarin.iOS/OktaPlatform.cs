using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace Okta.Xamarin.iOS
{
	public class OktaPlatform : OktaPlatformBase
	{
		public static async Task<OktaContext> InitAsync(UIKit.UIViewController iOSViewController, IOktaConfig config)
		{
			return await InitAsync(new iOsOidcClient(iOSViewController, config));
		}
	}
}