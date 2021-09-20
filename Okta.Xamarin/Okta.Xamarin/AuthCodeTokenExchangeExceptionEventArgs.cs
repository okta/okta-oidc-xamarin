// <copyright file="AuthCodeTokenExchangeExceptionEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace Okta.Xamarin
{
    /// <summary>
    /// Data relevant to AuthCodeTokenExchangeException events.
    /// </summary>
    public class AuthCodeTokenExchangeExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the OidcClient.
        /// </summary>
        public IOidcClient OidcClient { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get; set; }
    }
}
