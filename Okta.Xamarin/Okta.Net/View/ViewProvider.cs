using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Net.View
{
	public class ViewProvider : IViewProvider
	{
		public IViewRenderer ViewRenderer { get; set; }

		public IViewPresenter ViewPresenter { get; set; }

		public PresentResult Present(object state = null)
		{
			throw new NotImplementedException();
		}

		public RenderResult Render(object state = null)
		{
			throw new NotImplementedException();
		}
	}
}
