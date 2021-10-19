using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Identity
{
	public class IntrospectEventArgs
	{
		public string InteractionHandle { get; set; }
		public IdentityResponse IdxState { get; set; }
		public Exception Exception { get; set; }
	}
}
