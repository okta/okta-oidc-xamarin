using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Okta.Xamarin.ViewModels
{
    /// <summary>
    /// Command used to revoke the access token.
    /// </summary>
    public class RevokeAccessTokenCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RevokeAccessTokenCommand"/> class.
        /// </summary>
        public RevokeAccessTokenCommand() : base(async () => await OktaContext.Current.RevokeAsync(TokenKind.AccessToken))
        { }
    }
}
