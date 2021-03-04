// <copyright file="RevokeTokenEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace Okta.Xamarin
{
    /// <summary>
    /// Represents data relevant to revoke token events.
    /// </summary>
    public class RevokeEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the kind of token.
        /// </summary>
        public TokenKind TokenKind { get; set; }

        /// <summary>
        /// Gets or sets the state manager.
        /// </summary>
        public IOktaStateManager StateManager { get; set; }
    }
}
