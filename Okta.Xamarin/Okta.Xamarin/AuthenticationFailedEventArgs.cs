// <copyright file="AuthenticationFailedEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin
{
    public class AuthenticationFailedEventArgs : EventArgs
    {
        public AuthenticationFailedEventArgs(OAuthException oAuthException)
        {
            this.OAuthException = oAuthException;
        }

        public OAuthException OAuthException { get; set; }
    }
}
