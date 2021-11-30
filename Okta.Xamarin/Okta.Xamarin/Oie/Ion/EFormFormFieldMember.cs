// <copyright file="EFormFormFieldMember.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Newtonsoft.Json.Linq;

namespace Okta.Xamarin.Oie
{
    /// <summary>
    /// Represents an `IonFormFieldMember` with the member name of `eform`.
    /// </summary>
    [RegisteredFormFieldMember("eform")]
    public class EFormFormFieldMember : IonFormFieldMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EFormFormFieldMember"/> class.
        /// </summary>
        public EFormFormFieldMember()
        {
            this.Name = "eform";
            this.Optional = true;
            this.Description = @"The eform member value is either a Form object or a Link to a Form object that reflects the required object structure of each element in the field’s value array. The name ""eform"" is short for ""element form"".";
            this.FullName = "element form";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EFormFormFieldMember"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public EFormFormFieldMember(object value)
            : this()
        {
            this.Value = value;
        }

        /// <summary>
        /// Get an `EFormFormFieldMember` with the specified value.  Performs validation of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>EFormFormFieldMember.</returns>
        public static EFormFormFieldMember FromValue(string value)
        {
            EFormFormFieldMember result = null;
            if (value.IsJson(out JObject jObject))
            {
                result = new EFormFormFieldMember(jObject);
                if (!result.IsValid())
                {
                    return null;
                }
            }

            return result;
        }

        /// <summary>
        /// Determines if the `Value` property is valid.
        /// </summary>
        /// <returns>`true` if the value is valid.</returns>
        public override bool IsValid()
        {
            if (this.Value == null)
            {
                return false;
            }

            bool hasRequiredMembers = false;
            if (this.Value is JObject jObject)
            {
                hasRequiredMembers = this.HasRequiredMembers(jObject);
            }
            else if (this.JObjectValue != null)
            {
                hasRequiredMembers = this.HasRequiredMembers(this.JObjectValue);
            }

            return hasRequiredMembers && this.IsForm();
        }

        /// <summary>
        /// Determines if the specified `JObject` has required members `type` equal to "array" or "set" and `etype` equal to "object".
        /// </summary>
        /// <param name="jObject">The JObject</param>
        /// <returns>`true` if required members exist.</returns>
        protected bool HasRequiredMembers(JObject jObject)
        {
            string typeValue = (string)jObject["type"];
            if (string.IsNullOrEmpty(typeValue))
            {
                return false;
            }

            if (!"array".Equals(typeValue) && !"set".Equals(typeValue))
            {
                return false;
            }

            string etypeValue = (string)jObject["etype"];
            if (etypeValue != null && !"object".Equals(etypeValue))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines if the Value is a form.
        /// </summary>
        /// <returns>`true` if the Value is a form.</returns>
        protected bool IsForm()
        {
            bool isForm;
            if (this.Value is IIonJsonable ionJsonable)
            {
                string ionJson = ionJsonable.ToIonJson();
                isForm = IonForm.Validate(ionJson).Success;
            }
            else if (this.Value is IJsonable jsonable)
            {
                string json = jsonable.ToJson();
                isForm = IonForm.Validate(json).Success;
            }
            else
            {
                isForm = IonForm.Validate((IonMember)this).Success;
            }

            return isForm;
        }
    }
}
