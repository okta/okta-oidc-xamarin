// <copyright file="ByteArrayExtensions.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace Okta.Xamarin.Oie
{
    /// <summary>
    /// Provides extension methods to the `byte[]` class.
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Get the base 64 encoded value of the specified `byte[]`.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>base 64 encoded string.</returns>
        public static string ToBase64UrlEncoded(this byte[] data)
        {
            string base64 = Convert.ToBase64String(data);
            return base64.UrlEncode();
        }
    }
}
