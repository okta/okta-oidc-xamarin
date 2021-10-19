using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Net.Identity
{
	public class IdentityApiException : Exception
	{
		public IdentityApiException(IdentityResponse response): base($"{response.ApiError}: {response.ApiErrorDescription}")
		{
			this.IdentityResponse = response;
		}

		public IdentityResponse IdentityResponse { get; set; }
	}
}
