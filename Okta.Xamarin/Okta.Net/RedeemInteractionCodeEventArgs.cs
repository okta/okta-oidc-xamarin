using Okta.Net.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Net
{
	public class RedeemInteractionCodeEventArgs : EventArgs
	{
		public IIdentityClient IdentityClient { get; set; }
		public IIdentityInteraction IdentitySession { get; set; }
		public TokenResponse TokenResponse { get; set; }
		public string InteractionCode { get; set; }
	}
}
