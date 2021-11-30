// <copyright file="IonFormMemberValidationResult.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

namespace Okta.Xamarin.Oie
{
    /// <summary>
    /// Represents the result of form member validation.
    /// </summary>
    public class IonFormMemberValidationResult : IonFormValidationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IonFormMemberValidationResult"/> class.
        /// </summary>
        /// <param name="ionMember"></param>
        public IonFormMemberValidationResult(IonMember ionMember)
        {
            this.ValidatedMember = ionMember;
        }

        /// <summary>
        /// Gets the `IonMember` that is the target of validation.
        /// </summary>
        public IonMember ValidatedMember { get; private set; }

        /// <summary>
        /// Gets a value indicating whether validation succeeded.
        /// </summary>
        public override bool Success
        {
            get
            {
                return (this.IsLink || this.ValidatedMember.IsMemberNamed("form")) && this.HasRelArray && this.HasValueArray && this.HasOnlyFormFields && this.FormFieldsHaveUniqueNames;
            }
        }
    }
}
