using Okta.Net.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Okta.Net.Identity
{
	public interface IIdentityClient
	{
		IdentityClientConfiguration Configuration { get; set; }

		Task<IIdentityInteraction> InteractAsync(IIdentityInteraction state = null);
		Task<IIdentityIntrospection> IntrospectAsync(IIdentityInteraction state);
		Task<IIdentityIntrospection> IntrospectAsync(string interactionHandle);

		Task<TokenResponse> RedeemInteractionCodeAsync(IIdentityInteraction identitySession, string interactionCode);
	}
}
