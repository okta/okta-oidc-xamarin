using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin
{
    public class SignInEventArgs : EventArgs
    {
        public OktaState StateManager { get; set; }
    }
}
