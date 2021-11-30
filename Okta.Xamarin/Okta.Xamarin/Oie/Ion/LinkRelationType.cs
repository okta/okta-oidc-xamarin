// <copyright file="LinkRelationType.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

namespace Okta.Xamarin.Oie
{
    /// <summary>
    /// Represents a link relation type.
    /// </summary>
    public class LinkRelationType
    {
        public static implicit operator string(LinkRelationType relationType)
        {
            return relationType.ToString();
        }

        public static implicit operator LinkRelationType(string value)
        {
            return new LinkRelationType(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkRelationType"/> class.
        /// </summary>
        public LinkRelationType() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkRelationType"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public LinkRelationType(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Returns the Value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Value;
        }
    }
}
