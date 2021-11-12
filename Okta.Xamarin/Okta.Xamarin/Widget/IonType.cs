// <copyright file="IonType.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using Newtonsoft.Json;

namespace Okta.Xamarin.Widget
{
    /// <summary>
    /// A base class for Ion types.
    /// </summary>
    public abstract class IonType
    {
        private Type type;

        /// <summary>
        /// Initializes a new instance of the <see cref="IonType"/> class.
        /// </summary>
        public IonType()
        {
            this.TypeContextKind = TypeContextKind.TypeName;
        }

        /// <summary>
        /// Gets or sets the kind of the type context.
        /// </summary>
        [JsonIgnore]
        public TypeContextKind TypeContextKind
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Type context.  Setting this value to a non null value causes IncludeTypeContext to return
        /// true regardless if the IncludeTypeContext value is explicitly set to false.
        /// </summary>
        [JsonIgnore]
        public Type Type
        {
            get => this.type;
            set
            {
                this.type = value;
                this.SetTypeContext();
            }
        }

        /// <summary>
        /// Returns a json string representation of the current `IonObject`.
        /// </summary>
        /// <param name="pretty">A value indicating whether to use indentation.</param>
        /// <param name="nullValueHandling">Specifies null handling options for the JsonSerializer.</param>
        /// <returns>json string.</returns>
        public virtual string ToJson(bool pretty = false, NullValueHandling nullValueHandling = NullValueHandling.Ignore)
        {
            return JsonExtensions.ToJson(this, pretty, nullValueHandling);
        }

        protected abstract void SetTypeContext();
    }
}
