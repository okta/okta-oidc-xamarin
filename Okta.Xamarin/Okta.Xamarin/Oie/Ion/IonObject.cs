// <copyright file="IonObject.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Okta.Xamarin.Oie
{
    /// <summary>
    /// Represents an IonObject whose value property is of the specified generic type TValue.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class IonObject<TValue> : IonObject
    {
        private TValue value;

        public static implicit operator IonObject<TValue>(TValue value)
        {
            return new IonObject<TValue> { Value = value };
        }

        public static implicit operator string(IonObject<TValue> value)
        {
            return value.ToJson();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IonObject{TValue}"/> class.
        /// </summary>
        public IonObject() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IonObject{TValue}"/> class.
        /// </summary>
        /// <param name="members">The members.</param>
        public IonObject(List<IonMember> members) : base(members)
        {
            this.Value = this.ToInstance();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IonObject{TValue}"/> class.
        /// </summary>
        /// <param name="json">A json string representing the members.</param>
        public IonObject(string json)
            : this(IonMember.ListFromJson(json).ToList())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IonObject{TValue}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public IonObject(TValue value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public new TValue Value
        {
            get => this.value;
            set
            {
                this.value = value;
                base.Value = value;
                if ((this.Members == null || this.Members?.Count == 0) &&
                    this.value != null)
                {
                    Type typeOfValue = this.value.GetType();
                    if (!IonTypes.All.Contains(typeOfValue))
                    {
                        this.Members = IonMember.ListFromJson(this.value?.ToJson()).ToList();
                    }
                }
            }
        }

        /// <summary>
        /// Returns a json string representing the current `IonObject`.
        /// </summary>
        /// <param name="pretty">A value indicating whether to use indentation.</param>
        /// <param name="nullValueHandling">Specifies null handling options for the JsonSerializer.</param>
        /// <returns></returns>
        public override string ToJson(bool pretty = false, NullValueHandling nullValueHandling = NullValueHandling.Ignore)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            if (this.Value != null)
            {
                data.Add("value", this.Value);
            }

            foreach (IonMember member in this.Members)
            {
                data.Add(member.Name, member.Value);
            }

            foreach (string key in this.SupportingMembers?.Keys)
            {
                data.Add(key, this.SupportingMembers[key]);
            }

            return data.ToJson(pretty, nullValueHandling);
        }

        /// <summary>
        /// Returns the current `IonObject` as an instance of the specified generic type `TValue`.
        /// </summary>
        /// <returns>`TValue`.</returns>
        public TValue ToInstance()
        {
            ConstructorInfo ctor = typeof(TValue).GetConstructor(Type.EmptyTypes);
            if (ctor == null)
            {
                throw new InvalidOperationException($"The specified type ({typeof(TValue).AssemblyQualifiedName}) does not have a parameterless constructor.");
            }

            TValue instance = (TValue)ctor.Invoke(null);
            foreach (IonMember ionMember in this)
            {
                ionMember.SetProperty(instance);
            }

            return instance;
        }

        /// <summary>
        /// Returns a value uniquely identifying this instance at runtime.
        /// </summary>
        /// <returns>`int`.</returns>
        public override int GetHashCode()
        {
            return this.ToJson(false).GetHashCode();
        }

        /// <summary>
        /// Returns a value indicating if the current instance is equivalent to the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>`bool`.</returns>
        public override bool Equals(object value)
        {
            if (value == null && this.Value == null)
            {
                return true;
            }

            if (value != null && this.Value == null)
            {
                return false;
            }

            if (value != null && this.Value != null)
            {
                if (value is string json && json.TryFromJson<TValue>(out TValue otherValue))
                {
                    return this.Value.ToJson().Equals(otherValue.ToJson());
                }
                else if (value is IonObject<TValue> otherIonObject)
                {
                    return this.Value.Equals(otherIonObject.Value);
                }

                return this.Value.Equals(value);
            }

            return false;
        }
    }

    /// <summary>
    /// Represents an `IonObject`.
    /// </summary>
    public class IonObject : IonType, IJsonable, IIonJsonable, IEnumerable<IonMember>
    {
        private static readonly object RegisteredMemberLock = new object();
        private static HashSet<string> registeredMembers;
        private List<IonMember> memberList;
        private Dictionary<string, IonMember> memberDictionary;
        private object value;

        public static implicit operator IonObject(string value)
        {
            return new IonObject { Value = value };
        }

        public static implicit operator string(IonObject value)
        {
            return value.ToJson();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IonObject"/> class.
        /// </summary>
        public IonObject()
        {
            this.memberList = new List<IonMember>();
            this.memberDictionary = new Dictionary<string, IonMember>();
            this.SupportingMembers = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IonObject"/> class.
        /// </summary>
        /// <param name="members">The members.</param>
        public IonObject(List<IonMember> members)
        {
            this.memberList = members;
            this.memberDictionary = new Dictionary<string, IonMember>();
            this.SupportingMembers = new Dictionary<string, object>();
            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IonObject"/> class.
        /// </summary>
        /// <param name="members">The members.</param>
        public IonObject(params IonMember[] members)
            : this(members.ToList())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IonObject"/> class.
        /// </summary>
        /// <param name="members">The members.</param>
        /// <param name="supportingMembers">The supporting members.</param>
        public IonObject(List<IonMember> ionMembers, Dictionary<string, object> supportingMembers)
            : this()
        {
            this.memberList = ionMembers;
            this.memberDictionary = new Dictionary<string, IonMember>();
            this.SupportingMembers = supportingMembers;
            this.Initialize();
        }

        /// <summary>
        /// Gets registered members as defined by the specification.
        /// </summary>
        public static HashSet<string> RegisteredMembers
        {
            get
            {
                if (registeredMembers == null)
                {
                    lock (RegisteredMemberLock)
                    {
                        if (registeredMembers == null)
                        {
                            registeredMembers = new HashSet<string>(new[]
                            {
                                "eform",
                                "etype",
                                "form",
                                "href",
                                "method",
                                "accepts",
                                "produces",
                                "rel",
                                "type",
                                "value",
                            });
                        }
                    }
                }

                return registeredMembers;
            }
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        [JsonIgnore]
        public string SourceJson { get; internal set; }

        /// <summary>
        /// Gets the members.
        /// </summary>
        public List<IonMember> Members
        {
            get
            {
                return this.memberList;
            }

            protected set
            {
                this.memberList = value;
                this.Initialize();
            }
        }

        /// <summary>
        /// Gets or sets the supporting members.
        /// </summary>
        public Dictionary<string, object> SupportingMembers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a member.
        /// </summary>
        /// <param name="name">The name of the member</param>
        /// <returns>`IonMember`.</returns>
        public virtual IonMember this[string name]
        {
            get
            {
                string pascalCase = name.PascalCase();
                if (this.memberDictionary.ContainsKey(pascalCase))
                {
                    return this.memberDictionary[pascalCase];
                }

                string camelCase = name.CamelCase();
                if (this.memberDictionary.ContainsKey(camelCase))
                {
                    return this.memberDictionary[camelCase];
                }

                IonMember result = new IonMember { Name = name, Parent = this };
                this.memberDictionary.Add(camelCase, result);
                this.memberDictionary.Add(pascalCase, result);
                return result;
            }

            set
            {
                if (this.memberDictionary.ContainsKey(name))
                {
                    this.memberDictionary[name] = value;
                }
                else
                {
                    this.memberDictionary.Add(name, value);
                }
            }
        }

        /// <summary>
        /// Returns a representation of the current `IonObject` as a dictionary.
        /// </summary>
        /// <returns>`Dictionary{string, object}`.</returns>
        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (IonMember ionMember in this.memberList)
            {
                dictionary.Add(ionMember.Name, ionMember.Value);
            }

            return dictionary;
        }

        /// <summary>
        /// Returns the current `IonObject` as an instance of generic type `T`.
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <returns>{T}.</returns>
        public T ToInstance<T>()
        {
            return IonExtensions.ToInstance<T>(this);
        }

        /// <summary>
        /// Returns a value indicating if the current instance is equivalent to the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>`bool`.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null && this.Value == null)
            {
                return true;
            }

            if (obj != null && this.Value == null)
            {
                return false;
            }

            if (obj != null && this.Value != null)
            {
                if (obj is IonObject otherIonObject)
                {
                    return this.Value.Equals(otherIonObject.Value);
                }

                return this.Value.Equals(obj);
            }

            return false;
        }

        /// <summary>
        /// Returns a value uniquely identifying this instance at runtime.
        /// </summary>
        /// <returns>`int`.</returns>
        public override int GetHashCode()
        {
            if (this.Value == null)
            {
                return base.GetHashCode();
            }

            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Returns the value of the specified member.
        /// </summary>
        /// <param name="memberName">The member name.</param>
        /// <returns>`IonObject`.</returns>
        public IonObject ValueOf(string memberName)
        {
            return new IonObject(IonMember.ListFromObject(this[memberName]?.Value).ToArray());
        }

        /// <summary>
        /// Returns the member with the specified name.
        /// </summary>
        /// <param name="memberName">The member name.</param>
        /// <returns>`IonMember`.</returns>
        public IonMember MemberOf(string memberName)
        {
            return this[memberName];
        }

        /// <summary>
        /// Add a member.
        /// </summary>
        /// <param name="name">The name of the member to add.</param>
        /// <param name="value">The value of the member to add.</param>
        /// <returns>The current `IonObject`.</returns>
        public IonObject AddMember(string name, object value)
        {
            return this.AddMember(new IonMember(name, value));
        }

        /// <summary>
        /// Add the specified member.
        /// </summary>
        /// <param name="ionMember">The member.</param>
        /// <returns>The current `IonObject`.</returns>
        public IonObject AddMember(IonMember ionMember)
        {
            ionMember.Parent = this;
            this.memberList.Add(ionMember);
            this.Initialize();
            return this;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object Value
        {
            get => this.value;
            set
            {
                this.value = value;
                if ((this.Members == null || this.Members?.Count == 0) &&
                   this.value != null)
                {
                    Type typeOfValue = this.value.GetType();
                    if (!IonTypes.All.Contains(typeOfValue))
                    {
                        this.Members = IonMember.ListFromJson(this.value?.ToJson()).ToList();
                    }
                    else if (typeOfValue == typeof(string) && ((string)this.value).TryFromJson(out Dictionary<string, object> result))
                    {
                        this.Members = IonMember.ListFromDictionary(result).ToList();
                    }
                }
            }
        }

        /// <summary>
        /// Set the value for the specified supporting member.
        /// </summary>
        /// <param name="name">The name of the supporting member.</param>
        /// <param name="value">The value of the supporting member.</param>
        /// <returns>The current `IonObject`.</returns>
        public IonObject SetSupportingMember(string name, object value)
        {
            if (this.SupportingMembers == null)
            {
                this.SupportingMembers = new Dictionary<string, object>();
            }

            if (this.SupportingMembers.ContainsKey(name))
            {
                this.SupportingMembers[name] = value;
            }
            else
            {
                this.SupportingMembers.Add(name, value);
            }

            return this;
        }

        /// <summary>
        /// Adds supporting members.
        /// </summary>
        /// <param name="keyValuePairs">The members to add.</param>
        /// <returns>The current `IonObject`.</returns>
        public IonObject AddSupportingMembers(List<System.Collections.Generic.KeyValuePair<string, object>> keyValuePairs)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, object> kvp in keyValuePairs)
            {
                this.AddSupportingMember(kvp.Key, kvp.Value);
            }

            return this;
        }

        /// <summary>
        /// Adds the specified supporting member if a supporting member of the same name does
        /// not already exist.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public IonObject AddSupportingMember(string name, object data = null)
        {
            if (!this.SupportingMembers.ContainsKey(name))
            {
                this.SupportingMembers.Add(name, data);
            }

            return this;
        }

        /// <summary>
        /// Sets the `type` member to the name of the specified generic type `T`.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The current `IonObject`.</returns>
        public IonObject SetTypeContext<T>()
        {
            return this.SetTypeContext(typeof(T));
        }

        /// <summary>
        /// Sets the `type` member to the name of the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The current `IonObject`.</returns>
        public IonObject SetTypeContext(Type type)
        {
            return this.SetSupportingMember("type", type.Name);
        }

        /// <summary>
        /// Sets the `fullName` member to the full name of the specified generic type `T`.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The current `IonObject`.</returns>
        public IonObject SetTypeFullNameContext<T>()
        {
            return this.SetTypeFullNameContext(typeof(T));
        }

        /// <summary>
        /// Sets the `fullName` member to the full name of the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The current `IonObject`.</returns>
        public IonObject SetTypeFullNameContext(Type type)
        {
            return this.SetSupportingMember("fullName", type.FullName);
        }

        /// <summary>
        /// Sets the `assemblyQaulifiedName` member to the assembly qualified name of the specfied generic type `T`.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The current `IonObject`.</returns>
        public IonObject SetTypeAssemblyQualifiedNameContext<T>()
        {
            return this.SetTypeAssemblyQualifiedNameContext(typeof(T));
        }

        /// <summary>
        /// Sets the `assemblyQaulifiedName` member to the assembly qualified name of the specfied type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The current `IonObject`.</returns>
        public IonObject SetTypeAssemblyQualifiedNameContext(Type type)
        {
            return this.SetSupportingMember("assemblyQualifiedName", type.AssemblyQualifiedName);
        }

        /// <summary>
        /// Reads the specified json as an `IonObject`.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static IonObject ReadObject(string json)
        {
            Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            List<IonMember> members = new List<IonMember>();
            foreach (KeyValuePair<string, object> keyValuePair in dictionary)
            {
                members.Add(keyValuePair);
            }

            return new IonObject(members) { SourceJson = json };
        }

        /// <summary>
        /// Reads the specified json as an IonObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static IonObject<T> ReadObject<T>(string json)
        {
            Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Dictionary<string, PropertyInfo> properties = IonMember.GetPropertyDictionary(typeof(T));
            List<IonMember> members = new List<IonMember>();
            List<KeyValuePair<string, object>> supportingMembers = new List<KeyValuePair<string, object>>();

            foreach (System.Collections.Generic.KeyValuePair<string, object> keyValuePair in dictionary)
            {
                if (properties.ContainsKey(keyValuePair.Key))
                {
                    members.Add(keyValuePair);
                }
                else
                {
                    supportingMembers.Add(keyValuePair);
                }
            }

            IonObject<T> result = new IonObject<T>(members) { SourceJson = json };
            result.Initialize();
            result.Value = result.ToInstance();
            result.AddSupportingMembers(supportingMembers);
            return result;
        }

        /// <summary>
        /// Sets the type context.
        /// </summary>
        protected override void SetTypeContext()
        {
            switch (this.TypeContextKind)
            {
                case TypeContextKind.Invalid:
                case TypeContextKind.TypeName:
                    this.SetTypeContext(this.Type);
                    break;
                case TypeContextKind.FullName:
                    this.SetTypeFullNameContext(this.Type);
                    break;
                case TypeContextKind.AssemblyQualifiedName:
                    this.SetTypeAssemblyQualifiedNameContext(this.Type);
                    break;
            }
        }

        private void Initialize()
        {
            Dictionary<string, IonMember> temp = new Dictionary<string, IonMember>();
            foreach (IonMember ionMember in this.memberList)
            {
                ionMember.Parent = this;
                string camelCase = ionMember.Name.CamelCase();
                string pascalCase = ionMember.Name.PascalCase();
                temp.Add(camelCase, ionMember);
                temp.Add(pascalCase, ionMember);
            }

            this.memberDictionary = temp;
        }

        public IEnumerator<IonMember> GetEnumerator()
        {
            return this.memberList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.memberList.GetEnumerator();
        }

        /// <summary>
        /// Returns a json string representation of the current `IonObject`.
        /// </summary>
        /// <returns>json string.</returns>
        public virtual string ToJson()
        {
            return this.ToJson(false);
        }

        /// <summary>
        /// Returns a json string representation of the current `IonObject`.
        /// </summary>
        /// <param name="pretty">A value indicating whether to use indentation.</param>
        /// <param name="nullValueHandling">Specifies null handling options for the JsonSerializer.</param>
        /// <returns>json string.</returns>
        public override string ToJson(bool pretty = false, NullValueHandling nullValueHandling = NullValueHandling.Ignore)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            if (this.Value != null)
            {
                if (this.Value is string stringValue)
                {
                    if (stringValue.IsJson(out JObject jObject))
                    {
                        data = jObject.ToObject<Dictionary<string, object>>();
                    }
                    else
                    {
                        data = new Dictionary<string, object>()
                        {
                            { "value", stringValue },
                        };
                    }
                }
                else
                {
                    data = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(this.Value));
                }
            }

            foreach (IonMember member in this.memberList)
            {
                data.Add(member.Name, member.Value);
            }

            foreach (string key in this.SupportingMembers?.Keys)
            {
                data.Add(key, this.SupportingMembers[key]);
            }

            return data.ToJson(pretty, nullValueHandling);
        }

        /// <summary>
        /// Returns an Ion json string representation of the current `IonObject`.
        /// </summary>
        /// <returns>Ion json string.</returns>
        public virtual string ToIonJson()
        {
            return this.ToIonJson(false);
        }

        /// <summary>
        /// Returns an Ion json string representation of the current `IonObject`.
        /// </summary>
        /// <param name="pretty">A value indicating whether to use indentation.</param>
        /// <param name="nullValueHandling">Specifies null handling options for the JsonSerializer.</param>
        /// <returns>An Ion json string.</returns>
        public virtual string ToIonJson(bool pretty = false, NullValueHandling nullValueHandling = NullValueHandling.Ignore)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            if (this.Value != null)
            {
                if (this.Value is IIonJsonable ionJsonable)
                {
                    data.Add("value", ionJsonable.ToIonJson(pretty, nullValueHandling));
                }
                else
                {
                    data.Add("value", this.Value);
                }
            }

            foreach (IonMember member in this.memberList)
            {
                data.Add(member.Name, member.Value);
            }

            foreach (string key in this.SupportingMembers?.Keys)
            {
                data.Add(key, this.SupportingMembers[key]);
            }

            return data.ToJson(pretty, nullValueHandling);
        }
    }
}
