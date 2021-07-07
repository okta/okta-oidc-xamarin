using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Okta.Xamarin.ViewModels
{
    public class SignOutCommand : Command
    {
        public SignOutCommand()
            : base(async () =>
            {
                if (OktaContext.Current.OidcClient != null &&
                OktaContext.Current.StateManager != null)
                {
                    await OktaContext.Current.SignOutAsync();
                }
            })
        {
        }
    }
}
