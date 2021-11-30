// <copyright file="IonExtensions.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Okta.Xamarin.Oie
{
    /// <summary>
    /// Provides extensions relevant to the Ion specification.
    /// </summary>
    public static class IonExtensions
    {
        /// <summary>
        /// Returns an instance of generic type `T` with properties set from the specified members.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="ionMembers">The members.</param>
        /// <returns>Instance of `T`.</returns>
        public static T ToInstance<T>(this IEnumerable<IonMember> ionMembers)
        {
            ConstructorInfo ctor = typeof(T).GetConstructor(Type.EmptyTypes);
            if (ctor == null)
            {
                throw new InvalidOperationException($"The specified type ({typeof(T).AssemblyQualifiedName}) does not have a parameterless constructor.");
            }

            T instance = (T)ctor.Invoke(null);
            foreach (IonMember ionMember in ionMembers)
            {
                ionMember.SetProperty(instance);
            }

            return instance;
        }

        /// <summary>
        /// Returns a value indicating if the specified json string represents an array.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <returns>bool.</returns>
        public static bool IsJsonArray(this string json)
        {
            return IsJsonArray(json, out _);
        }

        /// <summary>
        /// Returns a value indicating if the specified json string represents an array.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <param name="jArray">The `JArray` instance if the specified json represents an array.</param>
        /// <returns>bool.</returns>
        public static bool IsJsonArray(this string json, out JArray jArray)
        {
            return IsJsonArray(json, out jArray, out _);
        }

        /// <summary>
        /// Returns a value indicating if the specified json string represents an array.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <param name="jArray">The `JArray` instance if the specified json represents an array.</param>
        /// <param name="exception">The exception that caused failure if the specified json does not represent an array.</param>
        /// <returns>bool.</returns>
        public static bool IsJsonArray(this string json, out JArray jArray, out Exception exception)
        {
            exception = null;
            try
            {
                jArray = JArray.Parse(json);
                return true;
            }
            catch (Exception ex)
            {
                exception = ex;
                jArray = null;
                return false;
            }
        }

        /// <summary>
        /// Returns a value indicating if the specified json string is valid.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <returns>bool.</returns>
        public static bool IsJson(this string json)
        {
            return IsJson(json, out _);
        }

        /// <summary>
        /// Returns a value indicating if the specified json string is valid.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <param name="jObject">The parsed json.</param>
        /// <returns>bool.</returns>
        public static bool IsJson(this string json, out JObject jObject)
        {
            return IsJson(json, out jObject, out _);
        }

        /// <summary>
        /// Returns a value indicating if the specified json string is valid.
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <param name="jObject">The parsed json.</param>
        /// <param name="exception">The exception that caused failure if the specified string is not valid json.</param>
        /// <returns>bool.</returns>
        public static bool IsJson(this string json, out JObject jObject, out Exception exception)
        {
            exception = null;
            try
            {
                jObject = JObject.Parse(json);
                return true;
            }
            catch (JsonReaderException ex)
            {
                exception = ex;
                jObject = null;
                return false;
            }
        }
    }
}
