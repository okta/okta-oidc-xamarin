// <copyright file="StorageEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace Okta.Xamarin.Widget.Pipeline.Data
{
	public class StorageEventArgs : EventArgs
    {
        public string Key { get; set; }

        public object Value { get; set; }

        public IStorageProvider StorageProvider { get; set; }

        public Exception Exception { get; set; }
    }
}
