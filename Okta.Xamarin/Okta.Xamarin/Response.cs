// <copyright file="Response.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Newtonsoft.Json;
using Okta.Xamarin.Models;

namespace Okta.Xamarin
{
    /// <summary>
    /// A base class for Okta API responses.
    /// </summary>
    public abstract class Response : Serializable
    {
        /// <summary>
        /// Gets or sets the error if any.
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the error description if any.
        /// </summary>
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Gets a value indicating whether there is an error.
        /// </summary>
        public bool IsException => !string.IsNullOrEmpty(this.Error);

        /// <summary>
        /// Throws an OktaApiException exception if there is an error.
        /// </summary>
        public void EnsureSuccess()
        {
            if (this.IsException)
            {
                throw new OktaApiException(this);
            }
        }
    }
}
