using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Okta.Xamarin
{
	public partial class OidcClient
	{
		public Action<string> onLaunchBrowser { get; set; }
		public Action onCloseBrowser { get; set; }


		public void LaunchBrowser(string url)
		{
			onLaunchBrowser?.Invoke(url);
		}

		public OidcClient(IOktaConfig config)
		{
			this.Config = config;
			validator.Validate(Config);
		}

		private void CloseBrowser()
		{
			onCloseBrowser?.Invoke();
		}

		public string State_Internal
		{
			get
			{
				return this.State;
			}
		}

		public void SetHttpMock(HttpMessageHandler handler)
		{
			if (handler == null)
				client = new HttpClient();
			else
				client = new HttpClient(handler);
		}

		public string GenerateAuthorizeUrlTest()
		{
			return GenerateAuthorizeUrl();
		}
	}
}
