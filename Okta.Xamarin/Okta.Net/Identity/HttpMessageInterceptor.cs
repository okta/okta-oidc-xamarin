using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Okta.Net.Identity
{
	/// <summary>
	/// Provides a means to intercept client calls if necessary.
	/// </summary>
	public class HttpMessageInterceptor : HttpClientHandler
	{
		public event EventHandler<HttpMessageEventArgs> SendStarted;
		public event EventHandler<HttpMessageEventArgs> SendCompleted;
		public event EventHandler<HttpMessageEventArgs> SendExceptionThrown;

		protected virtual Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return base.SendAsync(request, cancellationToken);
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			HttpResponseMessage response = null;
			try
			{
				SendStarted?.Invoke(this, new HttpMessageEventArgs
				{
					Handler = this,
					Request = request
				});
				response = await SendRequestAsync(request, cancellationToken);
				SendCompleted?.Invoke(this, new HttpMessageEventArgs
				{
					Handler = this,
					Request = request,
					Response = response
				});

				return response;
			}
			catch (Exception ex)
			{
				SendExceptionThrown?.Invoke(this, new HttpMessageEventArgs
				{
					Request = request,
					Exception = ex
				});
				return response;
			}
		}
	}
}
