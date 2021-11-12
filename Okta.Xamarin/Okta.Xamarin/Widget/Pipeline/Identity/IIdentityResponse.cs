// <copyright file="IIdentityResponse.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

namespace Okta.Xamarin.Widget.Pipeline.Identity
{
    /// <summary>
    /// Interface defining base IdentityResponse structure.
    /// </summary>
    public interface IIdentityResponse
    {
        /// <summary>
        /// Gets or sets the API error.
        /// </summary>
        string ApiError { get; set; }

        /// <summary>
        /// Gets or sets the API error description.
        /// </summary>
        string ApiErrorDescription { get; set; }

        /// <summary>
        /// Gets a value indicating whether this is an exception response.
        /// </summary>
        bool HasException { get; }

        /// <summary>
        /// Gets or sets the interaction handle.
        /// </summary>
        string InteractionHandle { get; set; }

        /// <summary>
        /// Gets or sets the raw response.
        /// </summary>
        string Raw { get; set; }

        /// <summary>
        /// Cast the raw response to the specified generic type T.
        /// </summary>
        /// <typeparam name="T">The type to convert this response to.</typeparam>
        /// <returns>{T}.</returns>
        T As<T>();

        /// <summary>
        /// If HasException is true, throws an exception.
        /// </summary>
        void EnsureSuccess();
    }
}