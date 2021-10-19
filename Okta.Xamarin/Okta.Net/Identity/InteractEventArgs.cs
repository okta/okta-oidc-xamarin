using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Identity
{
	public class InteractEventArgs : EventArgs
	{
		public string ClientId { get; set; }
		public List<string> Scopes { get; set; }
		public string CodeChallengeMethod { get; set; }
		public string RedirectUri { get; set; }
		public string State { get; set; }

		public Exception Exception { get; set; }
	}
}
