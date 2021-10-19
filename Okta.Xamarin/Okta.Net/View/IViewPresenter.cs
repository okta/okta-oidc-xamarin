using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.View
{
	public interface IViewPresenter
	{
		PresentResult Present(object state = null);
	}
}
