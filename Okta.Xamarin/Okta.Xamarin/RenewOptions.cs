﻿// <copyright file="RenewOptions.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

namespace Okta.Xamarin
{
    public class RenewOptions
    {
        public RenewOptions()
        {
            AuthorizationServerId = "default";
        }

        public string RefreshToken { get; set; }

        public bool RefreshIdToken { get; set; }

        public string AuthorizationServerId { get; set; }
    }
}
