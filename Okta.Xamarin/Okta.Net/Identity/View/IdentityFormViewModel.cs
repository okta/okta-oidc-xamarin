using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Net.Identity.View
{
	public class IdentityFormViewModel : IIdentityFormViewModel
	{
		public IdentityFormViewModel() { }

		public IdentityFormViewModel(IIdentityIntrospection form)
		{
			this.Form = form;
		}

		public Exception Exception { get; set; }

		public bool IsException => Exception != null;

		public IIdentityIntrospection Form { get; set; }
	}
}
