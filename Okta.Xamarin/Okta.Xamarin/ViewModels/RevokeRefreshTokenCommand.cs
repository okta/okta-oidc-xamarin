using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Okta.Xamarin.ViewModels
{
    public class RevokeRefreshTokenCommand : Command
    {
        public RevokeRefreshTokenCommand() : base(async () => await OktaContext.Current.RevokeAsync(TokenKind.RefreshToken))
        { }
    }
}
