using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin
{
    /// <summary>
    /// Arguments relevant to renew exception events.
    /// </summary>
    public class RenewExceptionEventArgs : RenewEventArgs
    {
        /// <summary>
        /// Gets or sets the exception that occurred.
        /// </summary>
        public Exception Exception { get; set; }
    }
}
