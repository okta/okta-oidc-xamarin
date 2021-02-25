// <copyright file="GetUserEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Security.Claims;

namespace Okta.Xamarin
{
    /// <summary>
    /// Represents data relevant to get user events.
    /// </summary>
    public class GetUserEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the state manager.
        /// </summary>
        public IOktaStateManager StateManager { get; set; }

        /// <summary>
        /// Gets or sets the claims principal.
        /// </summary>
        public object UserInfo { get; set; }
    }
}
