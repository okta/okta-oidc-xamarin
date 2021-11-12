// <copyright file="IdentityFormViewModel.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace Okta.Xamarin.Widget.Pipeline.Identity.View
{
    public class IdentityFormViewModel : IIdentityFormViewModel
    {
        public IdentityFormViewModel() { }

        public IdentityFormViewModel(IIdentityIntrospection form)
        {
            this.Form = form;
        }

        public Exception Exception { get; set; }

        public bool IsException => Exception != null;

        public IIdentityIntrospection Form { get; set; }
    }
}
