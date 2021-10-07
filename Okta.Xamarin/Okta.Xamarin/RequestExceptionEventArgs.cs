// <copyright file="RequestExceptionEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Okta.Xamarin
{
    /// <summary>
    /// Represents data relevant to request exception events.
    /// </summary>
    public class RequestExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestExceptionEventArgs"/> class.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="path">The path.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="authorizationServerId">The authorization server ID.</param>
        /// <param name="formUrlEncodedContent">The content.</param>
        /// <param name="responseBody">The body of the response if any.</param>
        public RequestExceptionEventArgs(Exception ex, HttpMethod httpMethod, string path, Dictionary<string, string> headers, string authorizationServerId, KeyValuePair<string, string>[] formUrlEncodedContent, string responseBody = null)
        {
            this.Exception = ex;
            this.HttpMethod = httpMethod;
            this.Path = path;
            this.Headers = headers;
            this.AuthorizationServerId = authorizationServerId;
            this.FormUrlEncodedContent = formUrlEncodedContent;
            this.ResponseBody = responseBody;
        }

        /// <summary>
        /// Gets the exception that occurred.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets the HTTP method.
        /// </summary>
        public HttpMethod HttpMethod { get; }

        /// <summary>
        /// Gets the path.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Gets the headers.
        /// </summary>
        public Dictionary<string, string> Headers { get; }

        /// <summary>
        /// Gets the authorization server ID.
        /// </summary>
        public string AuthorizationServerId { get; }

        /// <summary>
        /// Gets the form url encoded content.
        /// </summary>
        public KeyValuePair<string, string>[] FormUrlEncodedContent { get; }

        /// <summary>
        /// Gets the body of the response if any.
        /// </summary>
        public string ResponseBody { get; }
    }
}
