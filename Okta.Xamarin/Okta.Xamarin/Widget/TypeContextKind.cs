// <copyright file="TypeContextKind.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

namespace Okta.Xamarin.Widget
{
    /// <summary>
    /// Enumeration of kinds of types.  Describes the source of the `type` member.
    /// </summary>
    public enum TypeContextKind
    {
        Invalid,

        /// <summary>
        /// The name of a type.
        /// </summary>
        TypeName,

        /// <summary>
        /// The full name of a type.
        /// </summary>
        FullName,

        /// <summary>
        /// The assembly qualified name of a type.
        /// </summary>
        AssemblyQualifiedName,
    }
}