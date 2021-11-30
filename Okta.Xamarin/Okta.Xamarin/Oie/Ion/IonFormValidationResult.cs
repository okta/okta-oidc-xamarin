// <copyright file="IonFormValidationResult.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;

namespace Okta.Xamarin.Oie
{
    /// <summary>
    /// Represents the result of Ion form validation.
    /// </summary>
    public class IonFormValidationResult
    {
        /// <summary>
        /// Gets or sets the source json.
        /// </summary>
        public object SourceJson { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the form is a link.
        /// </summary>
        public bool IsLink { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a rel array is present.
        /// </summary>
        public bool HasRelArray { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a value array is present.
        /// </summary>
        public bool HasValueArray { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether only form fields are present.
        /// </summary>
        public bool HasOnlyFormFields { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether form fields have unique names.
        /// </summary>
        public bool FormFieldsHaveUniqueNames { get; set; }

        /// <summary>
        /// Gets or sets a dictionary containing form fields with duplicate names.
        /// </summary>
        public Dictionary<string, List<IonFormField>> FormFieldsWithDuplicateNames { get; set; }

        /// <summary>
        /// Gets a value indicating whether validation succeeded.
        /// </summary>
        public virtual bool Success
        {
            get { return this.IsLink && this.HasRelArray && this.HasValueArray && this.HasOnlyFormFields && this.FormFieldsHaveUniqueNames; }
        }
    }
}
