using System;
using System.Collections.Generic;
using System.Text;
using Okta.Xamarin.TinyIoC;

namespace Okta.Xamarin
{
    public class InitServicesEventArgs : EventArgs
    {
        public TinyIoCContainer TinyIoCContainer { get; set; }

        public Exception Exception { get; set; }
    }
}
