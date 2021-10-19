using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin
{
    /// <summary>
    /// Represents an exception returned by the Okta Api.
    /// </summary>
    public class OktaApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OktaApiException"/> class.
        /// </summary>
        /// <param name="response">The error response.</param>
        public OktaApiException(Response response)
            : base($"{response.Error}: {response?.ErrorDescription}")
        {
            this.Response = response;
        }

        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        public Response Response { get; set; }
    }
}
