using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Okta.Xamarin.ViewModels
{
    /// <summary>
    /// Command used to revoke the refresh token.
    /// </summary>
    public class RevokeRefreshTokenCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RevokeRefreshTokenCommand"/> class.
        /// </summary>
        public RevokeRefreshTokenCommand()
        : base(async () => await OktaContext.Current.RevokeAsync(TokenKind.RefreshToken))
        {
		}
    }
}
