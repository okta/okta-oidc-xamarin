// <copyright file="IEnumerableExtensions.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;

namespace Okta.Xamarin.Widget
{
    /// <summary>
    /// Provides extensions to the `IEnumerable` class.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Iterate over the specified IEnumerable passing each element to the specified action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="action"></param>
        public static void Each<T>(this IEnumerable<T> arr, Action<T> action)
        {
            foreach (T item in arr)
            {
                action(item);
            }
        }
    }
}
