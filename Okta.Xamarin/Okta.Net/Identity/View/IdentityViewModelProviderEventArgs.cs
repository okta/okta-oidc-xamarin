using Okta.Net.Identity.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Net.Identity.View
{
	public class IdentityViewModelProviderEventArgs : IdentityDataProviderEventArgs
	{
		public IIdentityViewModelProvider ViewModelProvider { get; set; }
		public IIdentityFormViewModel ViewModel { get; set; }
	}
}
