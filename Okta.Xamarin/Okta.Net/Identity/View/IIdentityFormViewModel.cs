using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Net.Identity.View
{
	public interface IIdentityFormViewModel
	{
		IIdentityIntrospection Form { get; set; }
		Exception Exception { get; set; }
		bool IsException { get; }
	}
}
