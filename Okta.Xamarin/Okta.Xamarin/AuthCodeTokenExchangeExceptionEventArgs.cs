using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin
{
    /// <summary>
    /// Data relevant to AuthCodeTokenExchangeFailed events.
    /// </summary>
    public class AuthCodeTokenExchangeExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the OidcClient.
        /// </summary>
        public IOidcClient OidcClient { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        public Exception Exception { get; set; }
    }
}
