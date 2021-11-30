using System;
using System.Reflection;

namespace Okta.Xamarin.Oie
{
    /// <summary>
    /// Provides extension methods to read custom attributes.
    /// </summary>
    public static class CustomAttributeExtensions
    {

        public static bool HasCustomAttributeOfType<T>(this MemberInfo member) where T : Attribute
        {
            return HasCustomAttributeOfType<T>(member, true);
        }

        public static bool HasCustomAttributeOfType<T>(this MemberInfo member, bool inherit) where T : Attribute
        {
            T outT = null;
            return HasCustomAttributeOfType<T>(member, inherit, out outT);
        }

        public static bool HasCustomAttributeOfType<T>(this MemberInfo memberInfo, bool inherit, out T attribute) where T : Attribute
        {
            return HasCustomAttributeOfType<T>(memberInfo, inherit, out attribute, false);
        }

        /// <summary>
        /// Determine if the MemberInfo is addorned with the specified generic attribute type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberInfo"></param>
        /// <param name="inherit"></param>
        /// <param name="attribute"></param>
        /// <param name="concreteAttribute">If true, must be the attribute specified and not an attribute that extends the specified attribute</param>
        /// <returns></returns>
        public static bool HasCustomAttributeOfType<T>(this MemberInfo memberInfo, bool inherit, out T attribute, bool concreteAttribute) where T : Attribute
        {
            if (memberInfo == null)
            {
                attribute = null;
                return false;
            }
            object[] customAttributes = memberInfo.GetCustomAttributes(typeof(T), inherit);

            return ContainsCustomAttributeOfType(customAttributes, out attribute, concreteAttribute);
        }

        private static bool ContainsCustomAttributeOfType<T>(object[] customAttributes, out T attribute,
            bool concreteAttribute)
            where T : Attribute
        {
            attribute = null;
            if (concreteAttribute)
            {
                foreach (object foundAttribute in customAttributes)
                {
                    if (foundAttribute.GetType() == typeof(T))
                    {
                        attribute = (T)foundAttribute;
                        break;
                    }
                }

                if (attribute == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (customAttributes.Length > 0)
                {
                    attribute = (T)customAttributes[0];
                }

                return customAttributes.Length > 0;
            }
        }
    }
}
