// <copyright file="IonFormFieldOption.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;

namespace Okta.Xamarin.Oie
{
    /// <summary>
    /// Represents an ion form field option, see // https://ionspec.org/#form-field-option-members.
    /// </summary>
    public class IonFormFieldOption : IonObject
    {
        private static readonly object FormFieldOptionMembersLock = new object();
        private static HashSet<string> formFieldOptionMembers;

        /// <summary>
        /// Gets the valid form field option members.
        /// </summary>
        public static HashSet<string> FormFieldOptionMembers
        {
            get
            {
                if (formFieldOptionMembers == null)
                {
                    lock (FormFieldOptionMembersLock)
                    {
                        if (formFieldOptionMembers == null)
                        {
                            formFieldOptionMembers = new HashSet<string>(new[]
                            {
                                "enabled",
                                "label",
                                "value",
                            });
                        }
                    }
                }

                return formFieldOptionMembers;
            }
        }
    }
}
