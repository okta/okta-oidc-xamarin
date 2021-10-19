using Okta.Net.Identity.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Identity.View
{
	public class IdentityViewModelProvider : IIdentityViewModelProvider
	{
		public event EventHandler<IdentityViewModelProviderEventArgs> GetViewModelStarted;
		public event EventHandler<IdentityViewModelProviderEventArgs> GetViewModelCompleted;
		public event EventHandler<IdentityViewModelProviderEventArgs> GetViewModelExceptionThrown;

		public IIdentityDataProvider DataProvider { get; set; }

		public async Task<IIdentityFormViewModel> GetViewModelAsync(IIdentityIntrospection identityForm)
		{
			try
			{
				GetViewModelStarted?.Invoke(this, new IdentityViewModelProviderEventArgs
				{
					ViewModelProvider = this
				});

				IdentityFormViewModel result = new IdentityFormViewModel(identityForm);

				GetViewModelCompleted?.Invoke(this, new IdentityViewModelProviderEventArgs
				{
					ViewModelProvider = this,
					ViewModel = result
				});
				return result;
			}
			catch (Exception ex)
			{
				GetViewModelExceptionThrown?.Invoke(this, new IdentityViewModelProviderEventArgs
				{
					ViewModelProvider = this,
					Exception = ex
				});
				return new IdentityFormViewModel { Exception = ex };
			}
		}
	}
}
