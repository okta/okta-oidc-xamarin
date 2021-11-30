// <copyright file="IonFormFieldValidationResult.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Okta.Xamarin.Oie
{
    /// <summary>
    /// Represents result of form field validation.
    /// </summary>
    public class IonFormFieldValidationResult
    {
        /// <summary>
        /// Gets a value indicating whether the value is an array.
        /// </summary>
        public bool ValueIsArray { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the value has only form field members.
        /// </summary>
        public bool ValueHasOnlyFormFields { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the value has form field members with unique names.
        /// </summary>
        public bool ValueHasFormFieldsWithUniqueNames { get; private set; }

        /// <summary>
        /// Gets a dictionary containing form fields with duplicate names.
        /// </summary>
        public Dictionary<string, List<IonFormField>> FormFieldsWithDuplicateNames { get; private set; }

        /// <summary>
        /// Returns an IonFormFieldValidationResult which represents the result of validating the specified member.
        /// </summary>
        /// <param name="ionMember">The member.</param>
        /// <returns>`IonFormFieldValidationResult`.</returns>
        public static IonFormFieldValidationResult ValidateFormFields(IonMember ionMember)
        {
            if (ionMember == null)
            {
                throw new ArgumentNullException(nameof(ionMember));
            }

            if (ionMember.Value is null)
            {
                return new IonFormFieldValidationResult();
            }

            if (ionMember.Value is JArray jArrayValue)
            {
                return ValidateFormFields(jArrayValue);
            }

            if (ionMember.Value is string stringValue)
            {
                if (stringValue.IsJsonArray(out JArray jArray))
                {
                    return ValidateFormFields(jArray);
                }
            }

            string valueJson = ionMember.Value.ToJson();
            if (valueJson.IsJsonArray(out JArray jsonArray))
            {
                return ValidateFormFields(jsonArray);
            }

            return new IonFormFieldValidationResult { ValueIsArray = false };
        }

        /// <summary>
        /// Returns an IonFormFieldValidationResult which represents the result of validating the specified member.
        /// </summary>
        /// <param name="jArrayValue">The form fields</param>
        /// <returns>`IonFormFieldValidationResult`.</returns>
        public static IonFormFieldValidationResult ValidateFormFields(JArray jArrayValue)
        {
            bool valueHasOnlyFormFields = true;
            bool valueHasFormFieldsWithUniqueNames = true;
            Dictionary<string, List<IonFormField>> formFieldsWithDuplicateNames;
            HashSet<IonFormField> formFields = new HashSet<IonFormField>();
            formFieldsWithDuplicateNames = new Dictionary<string, List<IonFormField>>();
            HashSet<string> duplicateNames = new HashSet<string>();
            foreach (JToken jToken in jArrayValue)
            {
                if (!IonFormField.IsValid(jToken?.ToString(), out IonFormField formField))
                {
                    valueHasOnlyFormFields = false;
                }

                List<IonFormField> existing = formFields.Where(ff => ff.Name.Equals(formField.Name)).ToList();
                if (existing.Any())
                {
                    duplicateNames.Add(formField.Name);

                    valueHasFormFieldsWithUniqueNames = false;
                }

                formFields.Add(formField);
            }

            if (duplicateNames.Count > 0)
            {
                foreach (string duplicateName in duplicateNames)
                {
                    if (!formFieldsWithDuplicateNames.ContainsKey(duplicateName))
                    {
                        formFieldsWithDuplicateNames.Add(duplicateName, new List<IonFormField>());
                    }

                    formFieldsWithDuplicateNames[duplicateName].AddRange(formFields.Where(ff => ff.Name.Equals(duplicateName)));
                }
            }

            return new IonFormFieldValidationResult
            {
                ValueIsArray = true,
                ValueHasOnlyFormFields = valueHasOnlyFormFields,
                ValueHasFormFieldsWithUniqueNames = valueHasFormFieldsWithUniqueNames,
                FormFieldsWithDuplicateNames = formFieldsWithDuplicateNames,
            };
        }
    }
}
