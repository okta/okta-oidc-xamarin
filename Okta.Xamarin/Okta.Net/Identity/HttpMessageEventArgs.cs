using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Okta.Net.Identity
{
	public class HttpMessageEventArgs
	{
		public HttpMessageHandler Handler { get; set; }
		public HttpRequestMessage Request { get; set; }
		public HttpResponseMessage Response { get; set; }

		public Exception Exception { get; set; }
	}
}
