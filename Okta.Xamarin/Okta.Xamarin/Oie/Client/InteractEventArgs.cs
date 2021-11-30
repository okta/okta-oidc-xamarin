// <copyright file="InteractEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;

namespace Okta.Xamarin.Oie.Client
{
    public class InteractEventArgs : EventArgs
    {
        public string ClientId { get; set; }

        public List<string> Scopes { get; set; }

        public string CodeChallengeMethod { get; set; }

        public string RedirectUri { get; set; }

        public string State { get; set; }

        public Exception Exception { get; set; }
    }
}
