// <copyright file="IonTypes.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;

namespace Okta.Xamarin.Widget
{
    public static class IonTypes
    {
        static IonTypes()
        {
            All = new HashSet<Type>
            {
                String,
                Int,
                Long,
                ULong,
                DateTime,
            };
        }

        public static HashSet<Type> All { get; private set; }

        public static Type String => typeof(string);

        public static Type Int => typeof(int);

        public static Type Long => typeof(long);

        public static Type ULong => typeof(ulong);

        public static Type DateTime => typeof(DateTime);
    }
}
