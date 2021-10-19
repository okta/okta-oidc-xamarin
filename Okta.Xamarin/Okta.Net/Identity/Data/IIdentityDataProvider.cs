using Okta.Net.Identity;
using Okta.Net.Identity.View;
using Okta.Net.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Identity.Data
{
	public interface IIdentityDataProvider : IHasServiceProvider
	{
		event EventHandler<IdentityDataProviderEventArgs> SessionStarting;
		event EventHandler<IdentityDataProviderEventArgs> SessionStarted;
		event EventHandler<IdentityDataProviderEventArgs> SessionStartExceptionThrown;

		SecureSessionProvider SecureSessionProvider { get; set; }
		IIdentityViewModelProvider ViewModelProvider { get; set; }

		Task<IIdentityInteraction> StartSessionAsync();
		Task<IIdentityIntrospection> GetFormDataAsync(string interactionHandle);
		Task<IIdentityFormViewModel> GetViewModelAsync(IIdentityIntrospection identityForm);
	}
}
