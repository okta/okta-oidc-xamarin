using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Okta.Xamarin.Test
{
	public class HttpMessageHandlerMock : HttpMessageHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var content = await request.Content.ReadAsStringAsync();
			System.Collections.Specialized.NameValueCollection data = System.Web.HttpUtility.ParseQueryString(content);
			var response = Responder(
				new Tuple<string, Dictionary<string, string>>(request.RequestUri.ToString(), data.ToDictionary()));

			return new HttpResponseMessage() { StatusCode = response.Item1, Content = new StringContent(response.Item2) };
		}

		public Func<Tuple<string, Dictionary<string, string>>, Tuple<System.Net.HttpStatusCode, string>> Responder { get; set; }
	}
}
