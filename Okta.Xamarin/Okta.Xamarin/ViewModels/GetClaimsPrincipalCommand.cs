using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Okta.Xamarin.ViewModels
{
    public class GetClaimsPrincipalCommand : Command
    {
        public GetClaimsPrincipalCommand() : base(async () => await OktaContext.Current.GetClaimsPrincipalAsync())
        {
        }
    }
}
