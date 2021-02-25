// <copyright file="RenewOptions.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

namespace Okta.Xamarin
{
    /// <summary>
    /// Options for token renewal.
    /// </summary>
    public class RenewOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenewOptions"/> class.
        /// </summary>
        public RenewOptions()
        {
            this.AuthorizationServerId = "default";
        }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the id token is refreshed.
        /// </summary>
        public bool RefreshIdToken { get; set; }

        /// <summary>
        /// Gets or sets the authorization server ID.
        /// </summary>
        public string AuthorizationServerId { get; set; }
    }
}
