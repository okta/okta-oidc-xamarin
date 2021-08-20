// <copyright file="IntrospectOptions.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

namespace Okta.Xamarin
{
    /// <summary>
    /// Options for introspection.
    /// </summary>
    public class IntrospectOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntrospectOptions"/> class.
        /// </summary>
        public IntrospectOptions()
        {
            //AuthorizationServerId = "default";
        }

        /// <summary>
        /// Gets or sets the token that is the target of introspection.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the kind of the target token.
        /// </summary>
        public TokenKind TokenKind { get; set; }

        /// <summary>
        /// Gets or sets the authorization server id.
        /// </summary>
        //public string AuthorizationServerId { get; set; }
    }
}
