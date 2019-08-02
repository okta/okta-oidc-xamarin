using System;

namespace Okta.Xamarin
{
	public class OAuthException : Exception
	{
		public string ErrorTitle { get; set; }
		public string ErrorDescription { get; set; }
		public int? HTTPStatusCode { get; set; }
		public string RequestUrl { get; set; }

		public override string ToString()
		{
			return base.ToString() + Environment.NewLine +
				"ErrorTitle: " + ErrorTitle ?? "<none>" + Environment.NewLine +
				"ErrorDescription: " + ErrorDescription ?? "<none>" + Environment.NewLine +
				"HTTPStatusCode: " + (HTTPStatusCode?.ToString() ?? "<none>") + Environment.NewLine +
				"RequestUrl: " + RequestUrl ?? "<none>";
		}
	}
}
