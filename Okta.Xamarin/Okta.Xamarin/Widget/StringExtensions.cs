// <copyright file="StringExtensions.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Web;

namespace Okta.Xamarin.Widget
{
    internal static class StringExtensions
    {
        /// <summary>
        /// Returns a camel cased string from the specified string using the specified 
        /// separators.  For example, the input "The quick brown fox jumps over the lazy
        /// dog" with the separators of "new string[]{" "}" should return the string 
        /// "theQuickBrownFoxJumpsOverTheLazyDog".
        /// </summary>
        /// <param name="stringToCamelize">The string to camelize.</param>
        /// <param name="preserveInnerUppers">if set to <c>true</c> [preserve inner uppers].</param>
        /// <param name="separators">The separators.</param>
        /// <returns></returns>
        public static string CamelCase(this string stringToCamelize, bool preserveInnerUppers = true, params string[] separators)
        {
            if (stringToCamelize.Length > 0)
            {
                string pascalCase = stringToCamelize.PascalCase(preserveInnerUppers, separators);
                string camelCase = string.Format("{0}{1}", pascalCase[0].ToString().ToLowerInvariant(), pascalCase.Remove(0, 1));
                return camelCase;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns a pascal cased string from the specified string using the specified
        /// separators.  For example, the input "The quick brown fox jumps over the lazy
        /// dog" with the separators of "new string[]{" "}" should return the string
        /// "TheQuickBrownFoxJumpsOverTheLazyDog".
        /// </summary>
        /// <param name="stringToPascalize"></param>
        /// <param name="preserveInnerUppers">If true uppercase letters that appear in.
        /// the middle of a word remain uppercase if false they are converted to lowercase.</param>
        /// <param name="separators"></param>
        /// <returns></returns>
        public static string PascalCase(this string stringToPascalize, bool preserveInnerUppers = true, params string[] separators)
        {
            string[] splitString = stringToPascalize.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string retVal = string.Empty;
            foreach (string part in splitString)
            {
                string firstChar = part[0].ToString().ToUpper();
                retVal += firstChar;
                for (int i = 1; i < part.Length; i++)
                {
                    if (!preserveInnerUppers)
                    {
                        retVal += part[i].ToString().ToLowerInvariant();
                    }
                    else
                    {
                        retVal += part[i].ToString();
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// Returns true if the string equals "true", "t", "yes", "y" or "1" using a case insensitive comparison.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>bool.</returns>
        public static bool IsAffirmative(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            return value.Equals("true", StringComparison.InvariantCultureIgnoreCase) ||
                   value.Equals("yes", StringComparison.InvariantCultureIgnoreCase) ||
                   value.Equals("t", StringComparison.InvariantCultureIgnoreCase) ||
                   value.Equals("y", StringComparison.InvariantCultureIgnoreCase) ||
                   value.Equals("1");
        }

        public static byte[] FromBase64UrlEncoded(this string data)
        {
            return Base64UrlDecode(data);
        }

        public static string Base64UrlEncode(this string data)
        {
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(data);
            string base64 = Convert.ToBase64String(inputBytes);
            return UrlEncode(base64);
        }

        public static byte[] Base64UrlDecode(this string data)
        {
            string base64 = UrlDecode(data);
            return Convert.FromBase64String(base64);
        }

        public static string UrlEncode(this string data)
        {
            return HttpUtility.UrlEncode(data);
        }

        public static string UrlDecode(this string data)
        {
            return HttpUtility.UrlDecode(data);
        }
    }
}
