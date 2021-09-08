using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin.Services
{
    /// <summary>
    /// An interface defining logging methods.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log an information message.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The format arguments.</param>
        void Info(string messageFormat, params object[] args);

        /// <summary>
        /// Log a warning message.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The format arguments.</param>
        void Warning(string messageFormat, params object[] args);

        /// <summary>
        /// Log an error message.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The format arguments.</param>
        void Error(string messageFormat, params object[] args);

        /// <summary>
        /// Log a fatal message.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The format arguments.</param>
        void Fatal(string messageFormat, params object[] args);
    }
}
