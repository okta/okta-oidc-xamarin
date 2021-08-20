// <copyright file="RenewCommand.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Xamarin.Forms;

namespace Okta.Xamarin.ViewModels
{
    /// <summary>
    /// A command to renew authentication tokens.
    /// </summary>
    public class RenewCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenewCommand"/> class.
        /// </summary>
        public RenewCommand()
            : base(async () =>
            {
                if (!string.IsNullOrEmpty(OktaContext.RefreshToken))
                {
                    await OktaContext.Current.RenewAsync();
                }
            })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenewCommand"/> class.
        /// </summary>
        /// <param name="refreshIdToken">A value indicating whether to refresh the ID token.</param>
        /// <param name="authorizationServerId">The authorization server ID, the default is "default".</param>
        public RenewCommand(bool refreshIdToken)//, string authorizationServerId = null)
            : base(async () =>
            {
                if (!string.IsNullOrEmpty(OktaContext.RefreshToken))
                {
					await OktaContext.RenewAsync(refreshIdToken);//, authorizationServerId);
                }
            })
        {
        }
    }
}
