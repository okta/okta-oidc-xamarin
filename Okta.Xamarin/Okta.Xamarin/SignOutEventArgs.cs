// <copyright file="SignOutEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace Okta.Xamarin
{
    /// <summary>
    /// Represents arguments relevant to a sign out event.
    /// </summary>
    public class SignOutEventArgs: EventArgs
    {
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public OktaState StateManager { get; set; }
    }
}
