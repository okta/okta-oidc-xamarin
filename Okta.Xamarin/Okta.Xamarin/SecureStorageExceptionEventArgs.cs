// <copyright file="SecureStorageExceptionEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin
{
    /// <summary>
    /// Arguments relevant to to secure storage exceptions.
    /// </summary>
    public class SecureStorageExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the Okta config.
        /// </summary>
        public IOktaConfig OktaConfig { get; set; }

        /// <summary>
        /// Gets or sets the OIDC client.
        /// </summary>
        public IOidcClient OidcClient{ get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get; set; }
    }
}
