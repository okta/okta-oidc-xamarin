﻿// <copyright file="IntrospectEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace Okta.Xamarin
{
    /// <summary>
    /// Represents data relevant to introspection events.
    /// </summary>
    public class IntrospectEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the token kind.
        /// </summary>
        public TokenKind TokenKind { get; set; }

        /// <summary>
        /// Gets or sets the state manager.
        /// </summary>
        public IOktaStateManager StateManager { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        public object Response { get; set; }
    }
}
