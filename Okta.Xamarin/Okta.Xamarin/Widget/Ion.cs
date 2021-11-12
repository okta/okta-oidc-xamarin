// <copyright file="Ion.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Okta.Xamarin.Widget
{
    /// <summary>
    /// A convenience entry point to Ion functionality.
    /// </summary>
    public abstract class Ion
    {
        /// <summary>
        /// Infer the Ion base type of the specified json string.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <returns>IonObjectTypes value.</returns>
        public static IonObjectTypes InferBaseType(string json)
        {
            Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (dictionary.ContainsKey("value"))
            {
                object value = dictionary["value"];
                if (value is JObject)
                {
                    return IonObjectTypes.Object;
                }

                if (value is JArray)
                {
                    return IonObjectTypes.Collection;
                }
            }
            return IonObjectTypes.Value;
        }

        /// <summary>
        /// Determines if the specified json string is an Ion Link.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <returns>`true` if the specified json string is an Ion link.</returns>
        public static bool IsLink(string json)
        {
            return IsLink(json, out IonLink ignore);
        }

        /// <summary>
        /// Determines if the specified json string is an Ion Link.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <param name="ionLink">The parsed IonLink.</param>
        /// <returns>`true` if the specified json string is an Ion link.</returns>
        public static bool IsLink(string json, out IonLink ionLink)
        {
            return IonLink.IsValid(json, out ionLink);
        }

        /// <summary>
        /// Determines if the specified json string is an Ion form field.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <returns>`true` if the specified json string is an Ion form field.</returns>
        public static bool IsFormField(string json)
        {
            return IsFormField(json, out IonFormField ignore);
        }

        /// <summary>
        /// Determines if the specified json string is an Ion form field.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <param name="ionFormField">The parsed Ion for field.</param>
        /// <returns>`true` if the specified json string is an Ion form field.</returns>
        public static bool IsFormField(string json, out IonFormField ionFormField)
        {
            return IonFormField.IsValid(json, out ionFormField);
        }

        /// <summary>
        /// Determines if the specified json string is an Ion form.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <returns>`true` if the specified json string is an Ion form.</returns>
        public static bool IsForm(string json)
        {
            return IsForm(json, out IonFormValidationResult ignore);
        }

        /// <summary>
        /// Determines if the specified json string is an Ion form.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <param name="ionForm">The parsed Ion form.</param>
        /// <returns>`true` if the specified json string is an Ion form.</returns>
        public static bool IsForm(string json, out IonFormValidationResult ionForm)
        {
            ionForm = IonForm.Validate(json);
            return ionForm.Success;
        }
    }
}
