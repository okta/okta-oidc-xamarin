// <copyright file="IdentityIntrospection.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Net.Http;

namespace Okta.Xamarin.Widget.Pipeline.Identity
{
    /// <summary>
    /// Represents the authentication form data model.
    /// </summary>
    public class IdentityIntrospection : IdentityResponse, IIdentityIntrospection
    {
        public IdentityIntrospection() { }

        internal IdentityIntrospection(HttpResponseMessage responseMessage)
            : base(responseMessage)
        {
        }

        public IonObject IonObject => IonObject.ReadObject(Raw);
    }
}
