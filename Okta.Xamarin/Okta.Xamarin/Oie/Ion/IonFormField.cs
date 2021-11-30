// <copyright file="IonFormField.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Okta.Xamarin.Oie
{
    /// <summary>
    /// Represents an ion form field, see https://ionspec.org/#form-fields.
    /// </summary>
    public class IonFormField : IonObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IonFormField"/> class.
        /// </summary>
        public IonFormField()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IonFormField"/> class.
        /// </summary>
        /// <param name="members">The members.</param>
        public IonFormField(List<IonMember> members)
            : base(members)
        {
            this.SetEtype();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IonFormField"/> class.
        /// </summary>
        /// <param name="members">The members.</param>
        public IonFormField(params IonMember[] members)
            : base(members)
        {
            this.SetEtype();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IonFormField"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="members">The members.</param>
        public IonFormField(string name, params IonMember[] members)
            : this(members)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get
            {
                IonMember nameMember = this["name"];
                return nameMember.Value as string;
            }

            set
            {
                this["name"].Value = value;
            }
        }

        /// <summary>
        /// All registered form field members.
        /// </summary>
        public static new HashSet<string> RegisteredMembers
        {
            get => IonFormFieldMember.RegisteredNames;
        }

        /// <summary>
        /// Returns the `desc` member value.
        /// </summary>
        /// <returns>`string`.</returns>
        public string Desc()
        {
            return this["desc"]?.Value?.ToString();
        }

        /// <summary>
        /// Sets the `desc` member value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The current `IonFormField`.</returns>
        public IonFormField Desc(string value)
        {
            this["desc"].Value = value;
            return this;
        }

        /// <summary>
        /// Returns the `eform` member value.
        /// </summary>
        /// <returns>object.</returns>
        public object EForm()
        {
            return this["eform"].Value;
        }

        /// <summary>
        /// Returns the `enabled` member value.
        /// </summary>
        /// <returns>`bool`.</returns>
        public bool Enabled()
        {
            return (bool)this["enabled"].Value?.ToString()?.IsAffirmative();
        }

        /// <summary>
        /// Sets the `enabled` member value to the specified value.
        /// </summary>
        /// <param name="enabled">The value.</param>
        /// <returns>The current `IonFormField`.</returns>
        public IonFormField Enabled(bool enabled)
        {
            this["enabled"].Value = enabled;
            return this;
        }

        protected void SetEtype()
        {
            this["etype"] = this["etype"];
        }

        /// <summary>
        /// Gets or sets the Value.
        /// </summary>
        public new IonFormFieldOption Value { get; set; }

        /// <summary>
        /// Gets or sets the member with the specified name.
        /// </summary>
        /// <param name="memberName">The member name.</param>
        /// <returns>`IonMember`.</returns>
        public override IonMember this[string memberName]
        {
            get
            {
                IonMember baseMember = base[memberName];
                if ("etype".Equals(memberName))
                {
                    if (baseMember.Value == null)
                    {
                        if (baseMember.Parent["eform"]?.Value != null)
                        {
                            baseMember.Value = "object";
                        }
                    }

                    if (!"object".Equals(baseMember.Value))
                    {
                        return null;
                    }
                }

                if ("enabled".Equals(memberName))
                {
                    if (baseMember.Value?.GetType() != typeof(bool))
                    {
                        return null;
                    }
                }

                if (IonFormFieldMember.RegisteredNames.Contains(memberName))
                {
                    if (IonFormFieldMember.RegisteredFormFieldIsValid(memberName, baseMember) != true)
                    {
                        return null;
                    }
                }

                return baseMember;
            }

            set
            {
                base[memberName] = value;
            }
        }

        /// <summary>
        /// Reads the specifie json string as an `IonFormField`.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <returns>`IonFormField`.</returns>
        public static IonFormField Read(string json)
        {
            Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            List<IonMember> members = new List<IonMember>();
            foreach (System.Collections.Generic.KeyValuePair<string, object> keyValuePair in dictionary)
            {
                members.Add(keyValuePair);
            }

            return new IonFormField(members) { SourceJson = json };
        }

        /// <summary>
        /// Returns a value indicating if the specified json is a valid form field.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <returns>`bool`.</returns>
        public static bool IsValid(string json)
        {
            return IsValid(json, out IonFormField ignore);
        }

        /// <summary>
        /// Returns a value indicating if the specified json string is a valid `IonFormField`.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <param name="formField">The parsed `IonFormField`.</param>
        /// <returns>`bool`.</returns>
        public static bool IsValid(string json, out IonFormField formField)
        {
            /**
             * 
An Ion Form Field MUST have a string member named name.

Each Ion Form Field within an Ion Form’s value array MUST have a unique name value compared to any other Form Field within the same array.
             * 
             */
            bool allFieldsAreFormFieldMembers = true;
            formField = null;
            Dictionary<string, object> keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            bool hasNameMember = keyValuePairs.ContainsKey("name");
            if (hasNameMember)
            {
                foreach (string key in keyValuePairs.Keys)
                {
                    if (!RegisteredMembers.Contains(key))
                    {
                        allFieldsAreFormFieldMembers = false;
                    }
                }
            }

            if (hasNameMember && allFieldsAreFormFieldMembers)
            {
                formField = IonFormField.Read(json);
                return true;
            }

            return false;
        }
    }
}
