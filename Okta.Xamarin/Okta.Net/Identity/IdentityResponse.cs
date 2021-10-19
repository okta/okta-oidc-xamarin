using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Okta.Net.Identity
{
	public class IdentityResponse : IIdentityResponse
	{
		public IdentityResponse() { }

		internal IdentityResponse(HttpResponseMessage responseMessage)
		{
			try
			{
				HttpResponseMessage = responseMessage.EnsureSuccessStatusCode();
				Raw = HttpResponseMessage?.Content?.ReadAsStringAsync().Result;
			}
			catch (Exception ex)
			{
				Exception = ex;
			}
		}

		[JsonProperty("error")]
		public string ApiError { get; set; }

		[JsonProperty("error_description")]
		public string ApiErrorDescription { get; set; }

		[JsonProperty("interaction_handle")]
		public string InteractionHandle { get; set; }

		internal HttpResponseMessage HttpResponseMessage { get; }

		private Exception _exception;
		internal Exception Exception
		{
			get
			{
				if (_exception == null && !string.IsNullOrEmpty(ApiError))
				{
					_exception = new IdentityApiException(this);
				}
				return _exception;
			}
			set => _exception = value;
		}

		[YamlIgnore]
		[JsonIgnore]
		public string Raw { get; set; }

		[YamlIgnore]
		[JsonIgnore]
		public bool HasException => Exception != null;

		public void EnsureSuccess()
		{
			if (HasException)
			{
				throw Exception;
			}
		}

		public T As<T>()
		{
			return JsonConvert.DeserializeObject<T>(Raw);
		}
	}
}
