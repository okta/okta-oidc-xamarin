// <copyright file="IonLinkedForm.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Newtonsoft.Json;

namespace Okta.Xamarin.Oie
{
    /// <summary>
    /// Represents a linked form.
    /// </summary>
    public class IonLinkedForm : IonForm, IIonLink
    {
        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        [JsonProperty("href")]
        public Iri Href { get; set; }
    }
}
