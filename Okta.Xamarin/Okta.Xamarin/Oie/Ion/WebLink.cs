// <copyright file="WebLink.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;

namespace Okta.Xamarin.Oie
{
    /// <summary>
    /// Represents a web link.
    /// </summary>
    public class WebLink
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebLink"/> class.
        /// </summary>
        public WebLink()
        {
            this.TargetAttributes = new List<string>();
        }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        public Iri Context { get; set; }

        /// <summary>
        /// Gets or sets the relation type.
        /// </summary>
        public LinkRelationType RelationType { get; set; }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        public Iri Target { get; set; }

        /// <summary>
        /// Gets or sets the target attributes.
        /// </summary>
        public List<string> TargetAttributes { get; set; }

        /// <summary>
        /// Returns a string describing this web link.
        /// </summary>
        /// <returns></returns>
        public string Describe()
        {
            string attributeDescription = this.TargetAttributes?.Count > 0 ? $", which has {string.Join(", ", this.TargetAttributes)}" : string.Empty;
            return $"{this.Context?.ToString()} has a {this.RelationType?.ToString()} resource at {this.Target?.ToString()}{attributeDescription}";
        }
    }
}
