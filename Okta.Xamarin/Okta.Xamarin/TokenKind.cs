// <copyright file="TokenType.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

namespace Okta.Xamarin
{
    /// <summary>
    /// Kinds of tokens.
    /// </summary>
    public enum TokenKind
    {
        /// <summary>
        /// Invalid token.
        /// </summary>
        Invalid,

        /// <summary>
        /// ID token.
        /// </summary>
        IdToken,

        /// <summary>
        /// Access token.
        /// </summary>
        AccessToken,

        /// <summary>
        /// Refresh token.
        /// </summary>
        RefreshToken,
    }
}
