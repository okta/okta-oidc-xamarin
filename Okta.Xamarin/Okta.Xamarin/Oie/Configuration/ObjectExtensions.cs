// <copyright file="ObjectExtensions.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Linq;
using System.Reflection;

namespace Okta.Xamarin.Oie.Configuration
{
    public static class ObjectExtensions
    {
        public static T CopyAs<T>(this object source)
            where T : new()
        {
            T result = new T();
            result.CopyProperties(source);
            return result;
        }

        public static object CopyProperties(this object destination, object source)
        {
            if (destination == null || source == null)
            {
                return destination;
            }

            ForEachProperty(destination, source, CopyProperty);

            return destination;
        }

        public static void CopyProperty(this object destination, object source, PropertyInfo destProp, PropertyInfo sourceProp)
        {
            if (sourceProp != null)
            {
                if (destProp.IsCompatibleWith(sourceProp))
                {
                    ParameterInfo[] indexParameters = sourceProp.GetIndexParameters();
                    if (indexParameters == null || indexParameters.Length == 0)
                    {
                        object value = sourceProp.GetValue(source, null);
                        destProp.SetValue(destination, value, null);
                    }
                }
            }
        }

        public static bool IsCompatibleWith(this PropertyInfo prop, PropertyInfo other)
        {
            return AreCompatibleProperties(prop, other);
        }

        public static bool AreCompatibleProperties(PropertyInfo destProp, PropertyInfo sourceProp)
        {
            return (sourceProp.PropertyType == destProp.PropertyType ||
                    sourceProp.PropertyType == Nullable.GetUnderlyingType(destProp.PropertyType) ||
                    Nullable.GetUnderlyingType(sourceProp.PropertyType) == destProp.PropertyType)
                   && destProp.CanWrite;
        }

        private static void ForEachProperty(object destination, object source, Action<object, object, PropertyInfo, PropertyInfo> action)
        {
            Type destinationType = destination.GetType();
            Type sourceType = source.GetType();

            foreach (PropertyInfo destProp in destinationType.GetProperties())
            {
                PropertyInfo sourceProp = TryGetSourcePropNamed(sourceType, destProp.Name);
                action(destination, source, destProp, sourceProp);
            }
        }

        private static PropertyInfo TryGetSourcePropNamed(Type sourceType, string propertyName)
        {
            try
            {
                return sourceType.GetProperty(propertyName);
            }
            catch (AmbiguousMatchException)
            {
                return sourceType.GetProperties().FirstOrDefault(p => p.DeclaringType == sourceType && p.Name == propertyName);
            }
        }
    }
}
