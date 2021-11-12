// <copyright file="IIonLink.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Newtonsoft.Json;

namespace Okta.Xamarin.Widget
{
    /// <summary>
    /// An Ion Link.
    /// </summary>
    public interface IIonLink
    {
        /// <summary>
        /// Gets or sets the href value.
        /// </summary>
        [JsonProperty("href")]
        Iri Href { get; set; }
    }
}
