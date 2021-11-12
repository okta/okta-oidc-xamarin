// <copyright file="HttpMessageEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Net.Http;

namespace Okta.Xamarin.Widget.Pipeline.Identity
{
    public class HttpMessageEventArgs
    {
        public HttpMessageHandler Handler { get; set; }

        public HttpRequestMessage Request { get; set; }

        public HttpResponseMessage Response { get; set; }

        public Exception Exception { get; set; }
    }
}
