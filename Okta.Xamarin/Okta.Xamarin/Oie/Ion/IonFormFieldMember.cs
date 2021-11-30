// <copyright file="IonFormFieldMember.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Okta.Xamarin.Oie
{
    /// <summary>
    /// Represents a registered form field member.
    /// </summary>
    public abstract class IonFormFieldMember : IonMember
    {
        private static readonly object RegisteredFormFieldMembersLock = new object();

        private static readonly object RegisteredFormFieldMemberTypesLock = new object();

        private static readonly Dictionary<string, Func<IonMember, IonFormField>> RegisteredFormFieldMemberReaders = new Dictionary<string, Func<IonMember, IonFormField>>()
        {
            {
                "eform", (member) =>
                {
                    if (RegisteredFormFieldMemberTypes.ContainsKey(member.Name))
                    {
                        if (member.Value == null)
                        {
                            return null;
                        }

                        IonFormFieldMember formField = ContructFormFieldMember(member);
                        if (!formField.IsValid())
                        {
                            return null;
                        }
                    }

                    IonFormField result = new IonFormField(member.Name, IonMember.ListFromObject(member.Value).ToArray());
                    return result;
                }
            },
        };

        private static Dictionary<string, Type> registeredFormFieldMemberTypes;

        private static HashSet<string> registeredFormFieldMembers;

        /// <summary>
        /// Gets the registered form field member names.
        /// </summary>
        public static HashSet<string> RegisteredNames
        {
            get
            {
                if (registeredFormFieldMembers == null)
                {
                    lock (RegisteredFormFieldMembersLock)
                    {
                        if (registeredFormFieldMembers == null)
                        {
                            registeredFormFieldMembers = new HashSet<string>(new[]
                            {
                                "desc",
                                "eform",
                                "enabled",
                                "etype",
                                "form",
                                "label",
                                "max",
                                "maxLength",
                                "maxsize",
                                "min",
                                "minLength",
                                "minsize",
                                "mutable",
                                "name",
                                "options",
                                "pattern",
                                "placeHolder",
                                "required",
                                "secret",
                                "type",
                                "value",
                                "visible",
                            });
                        }
                    }
                }

                return registeredFormFieldMembers;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is an optional member.
        /// </summary>
        [JsonIgnore]
        public bool Optional { get; protected set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        [JsonIgnore]
        public string FullName { get; protected set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [JsonIgnore]
        public string Description { get; protected set; }

        /// <summary>
        /// Returns a value indicating if the parent field is valid.  This method should be overridden if parent field validation is necessary, the default value is `true`.
        /// </summary>
        /// <param name="ionParentObject">The parent.</param>
        /// <returns>`bool`.</returns>
        public virtual bool ParentFieldIsValid(IonObject ionParentObject)
        {
            return true;
        }

        /// <summary>
        /// Returns a value indicating if this member is valid.  This method should be overridden if form field member validation is necessary, the default value is `true`.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid()
        {
            return true;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public override object Value 
        {
            get => this.ObjectValue;
            set
            {
                this.ObjectValue = value;
                Type objectType = this.ObjectValue.GetType();
                string stringValue = this.ObjectValue as string;
                if (!IonTypes.All.Contains(objectType) || !string.IsNullOrEmpty(stringValue))
                {
                    if (!string.IsNullOrEmpty(stringValue) && stringValue.IsJson(out JObject jo))
                    {
                        this.SourceJson = stringValue;
                        this.JObjectValue = jo;
                        return;
                    }

                    JObject jObject = this.ObjectValue as JObject;
                    if (jObject != null)
                    {
                        this.SourceJson = jObject.ToString();
                        this.JObjectValue = jObject;
                        return;
                    }

                    string objJson = this.ObjectValue?.ToJson(true) ?? "null";
                    this.SourceJson = objJson;
                    this.JObjectValue = JsonConvert.DeserializeObject<JObject>(objJson);
                }
            }
        }

        private static IonFormFieldMember ContructFormFieldMember(IonMember val)
        {
            return RegisteredFormFieldMemberTypes[val.Name].Construct<IonFormFieldMember>(val.Value);
        }

        /// <summary>
        /// Gets the registered form field member types.
        /// </summary>
        public static Dictionary<string, Type> RegisteredFormFieldMemberTypes
        {
            get
            {
                if (registeredFormFieldMemberTypes == null)
                {
                    lock (RegisteredFormFieldMemberTypesLock)
                    {
                        if (registeredFormFieldMemberTypes == null)
                        {
                            Dictionary<string, Type> temp = new Dictionary<string, Type>();
                            Assembly.GetExecutingAssembly()
                               .GetTypes()
                               .Where(type => type.HasCustomAttributeOfType<RegisteredFormFieldMemberAttribute>())
                               .Select(type => new { Type = type, Attribute = type.GetCustomAttribute<RegisteredFormFieldMemberAttribute>() })
                               .Each(val => temp.Add(val.Attribute.MemberName, val.Type));

                            registeredFormFieldMemberTypes = temp;
                        }
                    }
                }

                return registeredFormFieldMemberTypes;
            }
        }

        /// <summary>
        /// Returns a value indicating if the member is a valid registered form field.
        /// </summary>
        /// <param name="registeredMemberName">The member name.</param>
        /// <param name="member">The member.</param>
        /// <returns>`bool`.</returns>
        public static bool RegisteredFormFieldIsValid(string registeredMemberName, IonMember member)
        {
            return RegisteredFormFieldIsValid(registeredMemberName, member, out _);
        }

        /// <summary>
        /// Returns a value indicating if the member is a valid registered form field.
        /// </summary>
        /// <param name="registeredMemberName">The member name.</param>
        /// <param name="member">The member.</param>
        /// <param name="ionFormField">The parsed form field.</param>
        /// <returns>`bool`.</returns>
        public static bool RegisteredFormFieldIsValid(string registeredMemberName, IonMember member, out IonFormField ionFormField)
        {
            ionFormField = ReadRegisteredFormFieldMember(registeredMemberName, member);
            return ionFormField != null;
        }

        /// <summary>
        /// Returns a registered form field.
        /// </summary>
        /// <param name="registeredMemberName">The name of the registered member.</param>
        /// <param name="member">The parsed member.</param>
        /// <returns>`IonFormField`.</returns>
        public static IonFormField ReadRegisteredFormFieldMember(string registeredMemberName, IonMember member)
        {
            if (RegisteredFormFieldMemberReaders.ContainsKey(registeredMemberName))
            {
                return RegisteredFormFieldMemberReaders[registeredMemberName](member);
            }

            if (member?.Value is IJsonable jsonable)
            {
                return IonFormField.Read(jsonable.ToJson());
            }

            if (member?.Value is string stringValue)
            {
                if (stringValue.IsJson())
                {
                    return IonFormField.Read(stringValue);
                }
            }

            stringValue = member?.ToString();
            if (stringValue.IsJson())
            {
                return IonFormField.Read(stringValue);
            }

            return new IonFormField(new IonMember(registeredMemberName, member));
        }

        /// <summary>
        /// Gets or sets the object the Value property was set to.
        /// </summary>
        protected object ObjectValue { get; set; }

        /// <summary>
        /// The resulting JObject from reading the current form field from json input.
        /// </summary>
        protected JObject JObjectValue { get; set; }

        /// <summary>
        /// Gets either the original json parsed into ReadValue or, SetValue serialized if this instance 
        /// was not originally read from json.
        /// </summary>
        protected string SourceJson { get; private set; }

        /// <summary>
        /// Returns a value indicating if the specified member is present.
        /// </summary>
        /// <param name="memberName">The member name.</param>
        /// <returns>`bool`.</returns>
        protected bool JObjectHasMember(string memberName)
        {
            return this.JObjectHasMember(memberName, out JToken ignore);
        }

        /// <summary>
        /// Returns a value indicating if the specified member is present.
        /// </summary>
        /// <param name="memberName">The member name.</param>
        /// <param name="jToken">The member as a JToken.</param>
        /// <returns>`bool`.</returns>
        protected bool JObjectHasMember(string memberName, out JToken jToken)
        {
            bool hasMember = this.JObjectValue.ContainsKey(memberName);
            jToken = this.JObjectValue[memberName];
            return hasMember;
        }
    }
}
