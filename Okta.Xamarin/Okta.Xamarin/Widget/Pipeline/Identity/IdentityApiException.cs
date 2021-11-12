// <copyright file="IdentityApiException.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace Okta.Xamarin.Widget.Pipeline.Identity
{
    public class IdentityApiException : Exception
    {
        public IdentityApiException(IdentityResponse response)
            : base($"{response.ApiError}: {response.ApiErrorDescription}")
        {
            this.IdentityResponse = response;
        }

        public IdentityResponse IdentityResponse { get; set; }
    }
}
