// <copyright file="IJsonable.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

namespace Okta.Xamarin.Oie
{
    /// <summary>
    /// A class that may be represented as Json.
    /// </summary>
    public interface IJsonable
    {
        string ToJson();
    }
}
