// <copyright file="RevokeTokenEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace Okta.Xamarin
{
    public class RevokeTokenEventArgs : EventArgs
    {
        public string Token{ get; set; }

        public TokenType TokenType { get; set; }

        public OktaStateManager StateManager { get; set; }
    }
}
