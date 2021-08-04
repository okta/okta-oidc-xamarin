// <copyright file="InitServicesEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using Okta.Xamarin.TinyIoC;

namespace Okta.Xamarin
{
    /// <summary>
    /// Arguments relevant to events that occur during service initialization.
    /// </summary>
    public class InitServicesEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the inversion of control container.
        /// </summary>
        public TinyIoCContainer TinyIoCContainer { get; set; }

        /// <summary>
        /// Gets or sets the exception if any, may be null.
        /// </summary>
        public Exception Exception { get; set; }
    }
}
