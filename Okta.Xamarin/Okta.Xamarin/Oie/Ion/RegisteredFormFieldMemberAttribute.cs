// <copyright file="RegisteredFormFieldMemberAttribute.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace Okta.Xamarin.Oie
{
    /// <summary>
    /// An attribute used to register custom form field members.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisteredFormFieldMemberAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredFormFieldMemberAttribute"/> class.
        /// </summary>
        /// <param name="memberName"></param>
        public RegisteredFormFieldMemberAttribute(string memberName)
        {
            this.MemberName = memberName;
        }

        /// <summary>
        /// Gets or sets the member name.
        /// </summary>
        public string MemberName { get; set; }
    }
}
