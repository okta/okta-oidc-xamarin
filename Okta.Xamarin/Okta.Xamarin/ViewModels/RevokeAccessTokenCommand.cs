using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Okta.Xamarin.ViewModels
{
    public class RevokeAccessTokenCommand : Command
    {
        public RevokeAccessTokenCommand() : base(async () => await OktaContext.Current.RevokeTokenAsync(TokenKind.AccessToken))
        { }
    }
}
