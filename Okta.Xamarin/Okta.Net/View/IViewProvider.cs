using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.View
{
	public interface IViewProvider
	{
		IViewRenderer ViewRenderer { get; }
		IViewPresenter ViewPresenter { get; }

		PresentResult Present(object state = null);

		RenderResult Render(object state = null);
	}
}
