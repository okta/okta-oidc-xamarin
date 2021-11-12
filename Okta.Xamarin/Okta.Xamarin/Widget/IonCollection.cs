// <copyright file="IonCollection.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Okta.Xamarin.Widget
{
    /// <summary>
    /// A collection of Ion objects.
    /// </summary>
    public class IonCollection : IonObject, IJsonable, IIonJsonable, IEnumerable, IEnumerable<IonObject>
    {
        private readonly List<JToken> jTokens;
        private readonly Dictionary<string, object> metaDataElements;
        private List<IonObject> ionValueObjectList;

        /// <summary>
        /// Initializes a new instance of the <see cref="IonCollection"/> class.
        /// </summary>
        public IonCollection()
        {
            this.jTokens = new List<JToken>();
            this.ionValueObjectList = new List<IonObject>();
            this.metaDataElements = new Dictionary<string, object>();
            this.Value = this.ionValueObjectList;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IonCollection"/> class.
        /// </summary>
        /// <param name="ionValues">The values to populate the collection with.</param>
        public IonCollection(List<IonObject> ionValues) 
        {
            this.jTokens = new List<JToken>();
            this.ionValueObjectList = ionValues;
            this.metaDataElements = new Dictionary<string, object>();
            this.Value = this.ionValueObjectList;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IonCollection"/> class.
        /// </summary>
        /// <param name="jTokens">The values to populate the collection with.</param>
        public IonCollection(List<JToken> jTokens)
        {
            this.jTokens = jTokens;
            this.ionValueObjectList = jTokens.Select(jt => new IonObject { Value = jt }).ToList();
            this.metaDataElements = new Dictionary<string, object>();
            this.Value = this.ionValueObjectList;
        }

        /// <summary>
        /// Gets the meta data elements.
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, object> MetaDataElements
        {
            get => this.metaDataElements;
        }

        /// <summary>
        /// Gets or sets the values in this collection.
        /// </summary>
        public new List<IonObject> Value
        {
            get => this.ionValueObjectList;
            set
            {
                this.ionValueObjectList = value;
            }
        }

        /// <summary>
        /// Gets the count of objects in this collection.
        /// </summary>
        [JsonIgnore]
        public int Count => this.ionValueObjectList.Count;

        /// <summary>
        /// Returns an enumerator that iterates the values.
        /// </summary>
        /// <returns>IEnumerator.</returns>
        IEnumerator<IonObject> IEnumerable<IonObject>.GetEnumerator()
        {
            return this.ionValueObjectList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates the values.
        /// </summary>
        /// <returns>IEnumerator.</returns>
        public new virtual IEnumerator GetEnumerator()
        {
            return this.ionValueObjectList.GetEnumerator();
        }

        /// <summary>
        /// Adds the specified value to the collection.
        /// </summary>
        /// <param name="ionValueObject">The value to add.</param>
        public virtual void Add(IonObject ionValueObject)
        {
            this.ionValueObjectList.Add(ionValueObject);
        }

        /// <summary>
        /// Adds the specified value to the collection.
        /// </summary>
        /// <typeparam name="T">The type of the specified value.</typeparam>
        /// <param name="json">The json string representation of the value to add.</param>
        public virtual void Add<T>(string json)
        {
            this.Add<T>(new IonObject<T>(json));
        }

        /// <summary>
        /// Adds the specified value to the collection.
        /// </summary>
        /// <typeparam name="T">The type of the specified value.</typeparam>
        /// <param name="ionValueObject">The Ion object to add.</param>
        public virtual void Add<T>(IonObject<T> ionValueObject)
        {
            this.ionValueObjectList.Add(ionValueObject);
        }

        /// <summary>
        /// Determines if the collection contains the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>bool.</returns>
        public virtual bool Contains(object value)
        {
            return this.ionValueObjectList.Contains(value);
        }

        /// <summary>
        /// Gets the value at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [JsonIgnore]
        public IonObject this[int index]
        {
            get
            {
                return ionValueObjectList[index];
            }
        }

        /// <summary>
        /// Returns the json string representation of the collection.
        /// </summary>
        /// <returns></returns>
        public override string ToJson()
        {
            return this.ToJson(false);
        }

        /// <summary>
        /// Returns the json string representation of the collection.
        /// </summary>
        /// <param name="pretty">If true, use indentation.</param>
        /// <param name="nullValueHandling">Specifies null value handling for the underlying JsonSerializer.</param>
        /// <returns>Json string.</returns>
        public override string ToJson(bool pretty = false, NullValueHandling nullValueHandling = NullValueHandling.Ignore)
        {
            return base.ToJson(pretty, nullValueHandling);
        }

        /// <summary>
        /// Returns the Ion json string representation of the collection.
        /// </summary>
        /// <returns>Ion Json string.</returns>
        public override string ToIonJson()
        {
            return ToIonJson(false);
        }

        /// <summary>
        /// Returns the Ion json string representation of the collection.
        /// </summary>
        /// <param name="pretty">If true, use indentation.</param>
        /// <param name="nullValueHandling">Specifies null value handling for the underlying JsonSerializer.</param>
        /// <returns>Json string.</returns>
        public override string ToIonJson(bool pretty, NullValueHandling nullValueHandling = NullValueHandling.Ignore)
        {
            List<object> value = new List<object>();
            value.AddRange(this.ionValueObjectList.Select(iv => iv.ToDictionary()));
            Dictionary<string, object> toBeSerialized = new Dictionary<string, object>();
            foreach (string key in this.metaDataElements.Keys)
            {
                toBeSerialized.Add(key, this.metaDataElements[key]);
            }

            toBeSerialized.Add("value", value);

            return toBeSerialized.ToJson(pretty, nullValueHandling);
        }

        /// <summary>
        /// Ads the specified meta data.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>The current collection.</returns>
        public IonCollection AddElementMetaData(string name, object value)
        {
            this.metaDataElements.Add(name, value);
            return this;
        }

        /// <summary>
        /// Reads the specified json string as an IonCollection.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <returns>A new IonCollection.</returns>
        public static IonCollection Read(string json)
        {
            Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            List<JToken> jTokens = new List<JToken>();
            if (dictionary.ContainsKey("value"))
            {
                JArray arrayValue = dictionary["value"] as JArray;
                foreach (JToken token in arrayValue)
                {
                    jTokens.Add(token);
                }
            }

            IonCollection ionCollection = new IonCollection(jTokens);

            foreach (string key in dictionary.Keys)
            {
                if (!"value".Equals(key))
                {
                    ionCollection.AddElementMetaData(key, dictionary[key]);
                }
            }

            ionCollection.SourceJson = json;
            return ionCollection;
        }

        /// <summary>
        /// Removes the specified object from the collection.
        /// </summary>
        /// <param name="ionObject">The ion object.</param>
        protected void RemoveObject(IonObject ionObject)
        {
            if (this.ionValueObjectList.Contains(ionObject))
            {
                this.ionValueObjectList.Remove(ionObject);
            }
        }
    }
}
