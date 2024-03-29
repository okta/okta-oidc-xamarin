﻿// <copyright file="SecureStorageEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace Okta.Xamarin
{
    /// <summary>
    /// Arguments relevant to secure storage events.
    /// </summary>
    public class SecureStorageEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the Okta state manager.
        /// </summary>
        public IOktaStateManager OktaStateManager { get; set; }
    }
}
