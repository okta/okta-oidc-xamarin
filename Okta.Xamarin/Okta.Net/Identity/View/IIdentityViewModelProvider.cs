using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Identity.View
{
	public interface IIdentityViewModelProvider
	{
		event EventHandler<IdentityViewModelProviderEventArgs> GetViewModelStarted;
		event EventHandler<IdentityViewModelProviderEventArgs> GetViewModelCompleted;
		event EventHandler<IdentityViewModelProviderEventArgs> GetViewModelExceptionThrown;

		Task<IIdentityFormViewModel> GetViewModelAsync(IIdentityIntrospection identityForm);
	}
}
