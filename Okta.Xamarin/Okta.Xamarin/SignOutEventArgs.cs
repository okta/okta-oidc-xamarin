using System;

namespace Okta.Xamarin
{
    public class SignOutEventArgs: EventArgs
    {
        public OktaState StateManager { get; set; }
    }
}