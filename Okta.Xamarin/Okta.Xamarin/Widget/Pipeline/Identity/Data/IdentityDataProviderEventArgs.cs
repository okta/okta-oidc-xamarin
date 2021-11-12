// <copyright file="IdentityDataProviderEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace Okta.Xamarin.Widget.Pipeline.Identity.Data
{
    public class IdentityDataProviderEventArgs : EventArgs
    {
        public IIdentityDataProvider DataProvider { get; set; }

        public IIdentityIntrospection Form { get; set; }

        public Exception Exception { get; set; }
    }
}
