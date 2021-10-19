using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Net
{
	public interface IHasServiceProvider
	{
		IServiceProvider ServiceProvider { get; }
	}
}
