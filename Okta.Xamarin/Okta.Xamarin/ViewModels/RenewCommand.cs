using System;
using System.Collections.Generic;
using System.Text;
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
        public RenewCommand(bool refreshIdToken, string authorizationServerId = "default")
            : base(async () =>
            {
                if (!string.IsNullOrEmpty(OktaContext.RefreshToken))
                {
					await OktaContext.Current.RenewAsync(refreshIdToken, authorizationServerId);
                }
            })
        {
        }
    }
}
