// <copyright file="HttpMessageHandlerMock.cs" company="Okta, Inc">
// Copyright (c) 2019-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Okta.Xamarin;

namespace Okta.Xamarin.Test
{
    // TODO: refactor this to eliminate Responder in favor of GetTestResponse for simplicity.

    /// <summary>
    /// Mock message handler for testing.
    /// </summary>
    public class HttpMessageHandlerMock : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            System.Collections.Specialized.NameValueCollection data = new System.Collections.Specialized.NameValueCollection();
            if (request.Content != null)
            {
                string content = await request.Content.ReadAsStringAsync();
                data = System.Web.HttpUtility.ParseQueryString(content);
            }

            if (this.Responder != null)
            {
                var response = this.Responder(
                    new Tuple<string, Dictionary<string, string>>(request.RequestUri.ToString(), data.ToDictionary()));

                return new HttpResponseMessage() { StatusCode = response.Item1, Content = new StringContent(response.Item2) };
            }
            else if (this.GetTestResponse != null)
            {
                return this.GetTestResponse(request, cancellationToken);
            }

            return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };
        }

        /// <summary>
        /// Gets or sets the responder.
        /// </summary>
        public Func<Tuple<string, Dictionary<string, string>>, Tuple<System.Net.HttpStatusCode, string>> Responder { get; set; }

        /// <summary>
        /// Gets or sets the GetTestResponse function.
        /// </summary>
        public Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> GetTestResponse { get; set; }
    }
}
