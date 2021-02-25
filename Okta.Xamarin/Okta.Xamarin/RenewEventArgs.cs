// <copyright file="RenewEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace Okta.Xamarin
{
    /// <summary>
    /// Represents data relevant to renew events.
    /// </summary>
    public class RenewEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the state manager.
        /// </summary>
        public IOktaStateManager StateManager{ get; set; }

        /// <summary>
        /// Gets or sets a value indication whether the id token is refreshed.
        /// </summary>
        public bool RefreshIdToken { get; set; }

        /// <summary>
        /// Gets or sets the authorization server id.
        /// </summary>
        public string AuthorizationServerId { get; set; }

        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        public RenewResponse Response { get; set; }
    }
}
