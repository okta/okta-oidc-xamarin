// <copyright file="OktaLogger.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Okta.Xamarin.Services
{
    /// <summary>
    /// The default Okta logger implementation.
    /// </summary>
    public class OktaLogger : IOktaLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OktaLogger"/> class.
        /// </summary>
        /// <param name="logger">The base logger.</param>
        public OktaLogger(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.Logger = logger;
            this.DefaultInfoEventhandler = (sender, args) =>
            {
                try
                {
                    this.Info("Sender: {0}\r\nArgs: {1}", JsonConvert.SerializeObject(sender), JsonConvert.SerializeObject(args));
                }
                catch
                {
                    /* don't crash */
                }
            };
            this.DefaultWarningEventHandler = (sender, args) =>
            {
                try
                {
                    this.Warning("Sender: {0}\r\nArgs: {1}", JsonConvert.SerializeObject(sender), JsonConvert.SerializeObject(args));
                }
                catch
                {
                    /* don't crash */
                }
            };
            this.DefaultErrorEventHandler = (sender, args) =>
            {
                try
                {
                    this.Error("Sender: {0}\r\nArgs: {1}", JsonConvert.SerializeObject(sender), JsonConvert.SerializeObject(args));
                }
                catch
                {
                    /* don't crash */
                }
            };
            this.DefaultFatalEventHandler = (sender, args) =>
            {
                try
                {
                    this.Fatal("Sender: {0}\r\nArgs: {1}", JsonConvert.SerializeObject(sender), JsonConvert.SerializeObject(args));
                }
                catch
                {
                    /* don't crash */
                }
            };
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Gets or sets the default information event handler.
        /// </summary>
        protected EventHandler DefaultInfoEventhandler { get; set; }

        /// <summary>
        /// Gets or sets the default warning event handler.
        /// </summary>
        protected EventHandler DefaultWarningEventHandler { get; set; }

        /// <summary>
        /// Gets or sets the default error event handler.
        /// </summary>
        protected EventHandler DefaultErrorEventHandler { get; set; }

        /// <summary>
        /// Gets or sets the default fatal event handler.
        /// </summary>
        protected EventHandler DefaultFatalEventHandler { get; set; }

        /// <summary>
        /// Log the specified error message.
        /// </summary>
        /// <param name="messageFormat">The string compatible message format.</param>
        /// <param name="args">The message arguments.</param>
        public virtual void Error(string messageFormat, params object[] args)
        {
            this.Logger.Error(messageFormat, args);
        }

        /// <summary>
        /// Log the specified fatal message.
        /// </summary>
        /// <param name="messageFormat">The string compatible message format.</param>
        /// <param name="args">The message arguments.</param>
        public virtual void Fatal(string messageFormat, params object[] args)
        {
            this.Logger.Fatal(messageFormat, args);
        }

        /// <summary>
        /// Log the specified information message.
        /// </summary>
        /// <param name="messageFormat">The string compatible message format.</param>
        /// <param name="args">The message arguments.</param>
        public virtual void Info(string messageFormat, params object[] args)
        {
            this.Logger.Info(messageFormat, args);
        }

        /// <summary>
        /// Log the specified warning message.
        /// </summary>
        /// <param name="messageFormat">The string compatible message format.</param>
        /// <param name="args">The message arguments.</param>
        public virtual void Warning(string messageFormat, params object[] args)
        {
            this.Logger.Warning(messageFormat, args);
        }

        /// <summary>
        /// Log all relevant events using the default logging behavior.
        /// </summary>
        public void LogAllEvents()
        {
            this.LogLoadStateStartedEvents();
            this.LogLoadStateCompletedEvents();
            this.LogLoadStateExceptionEvents();
            this.LogSecureStorageWriteStartedEvents();
            this.LogSecureStorageWriteCompletedEvents();
            this.LogSecureStorageWriteExceptionEvents();
            this.LogSecureStorageReadStartedEvents();
            this.LogSecureStorageReadCompletedEvents();
            this.LogSecureStorageReadExceptionEvents();
            this.LogRequestExceptionEvents();
            this.LogSignInStartedEvents();
            this.LogSignInCompletedEvents();
            this.LogSignOutStartedEvents();
            this.LogSignOutCompletedEvents();
            this.LogAuthenticationFailedEvents();
            this.LogRevokeStartedEvents();
            this.LogRevokeCompletedEvents();
            this.LogRevokeExceptionEvents();
            this.LogGetUserStartedEvents();
            this.LogGetUserCompletedEvents();
            this.LogIntrospectStartedEvents();
            this.LogIntrospectCompletedEvents();
            this.LogRenewStartedEvents();
            this.LogRenewCompletedEvents();
            this.LogRenewExceptionEvents();
        }

        /// <inheritdoc/>
        public void LogRequestExceptionEvents(EventHandler<RequestExceptionEventArgs> eventHandler = null)
        {
            OktaContext.Current.RequestException += (sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultErrorEventHandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            };
        }

        /// <inheritdoc/>
        public void LogAuthenticationFailedEvents(EventHandler<AuthenticationFailedEventArgs> eventHandler = null)
        {
            OktaContext.AddAuthenticationFailedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultErrorEventHandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogGetUserCompletedEvents(EventHandler<GetUserEventArgs> eventHandler = null)
        {
            OktaContext.AddGetUserCompletedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogGetUserStartedEvents(EventHandler<GetUserEventArgs> eventHandler = null)
        {
            OktaContext.AddGetUserStartedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogIntrospectCompletedEvents(EventHandler<IntrospectEventArgs> eventHandler = null)
        {
            OktaContext.AddIntrospectCompletedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogIntrospectStartedEvents(EventHandler<IntrospectEventArgs> eventHandler = null)
        {
            OktaContext.AddIntrospectStartedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogLoadStateCompletedEvents(EventHandler<SecureStorageEventArgs> eventHandler = null)
        {
            OktaContext.AddLoadStateCompletedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogLoadStateExceptionEvents(EventHandler<SecureStorageExceptionEventArgs> eventHandler = null)
        {
            OktaContext.AddLoadStateExceptionListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultErrorEventHandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogLoadStateStartedEvents(EventHandler<SecureStorageEventArgs> eventHandler = null)
        {
            OktaContext.AddLoadStateStartedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogRenewCompletedEvents(EventHandler<RenewEventArgs> eventHandler = null)
        {
            OktaContext.AddRenewCompletedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogRenewExceptionEvents(EventHandler<RenewExceptionEventArgs> eventHandler = null)
        {
            OktaContext.AddRenewExceptionListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultErrorEventHandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogRenewStartedEvents(EventHandler<RenewEventArgs> eventHandler = null)
        {
            OktaContext.AddRenewStartedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogRevokeCompletedEvents(EventHandler<RevokeEventArgs> eventHandler = null)
        {
            OktaContext.AddRevokeCompletedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogRevokeExceptionEvents(EventHandler<RevokeExceptionEventArgs> eventHandler = null)
        {
            OktaContext.AddRevokeExceptionListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultErrorEventHandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogRevokeStartedEvents(EventHandler<RevokeEventArgs> eventHandler = null)
        {
            OktaContext.AddRevokeStartedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogSecureStorageReadCompletedEvents(EventHandler<SecureStorageEventArgs> eventHandler = null)
        {
            OktaContext.AddSecureStorageReadCompletedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogSecureStorageReadExceptionEvents(EventHandler<SecureStorageExceptionEventArgs> eventHandler = null)
        {
            OktaContext.AddSecureStorageReadExceptionListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultErrorEventHandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogSecureStorageReadStartedEvents(EventHandler<SecureStorageEventArgs> eventHandler = null)
        {
            OktaContext.AddSecureStorageReadStartedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogSecureStorageWriteCompletedEvents(EventHandler<SecureStorageEventArgs> eventHandler = null)
        {
            OktaContext.AddSecureStorageWriteCompletedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogSecureStorageWriteExceptionEvents(EventHandler<SecureStorageExceptionEventArgs> eventHandler = null)
        {
            OktaContext.AddSecureStorageWriteExceptionListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultErrorEventHandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogSecureStorageWriteStartedEvents(EventHandler<SecureStorageEventArgs> eventHandler = null)
        {
            OktaContext.AddSecureStorageWriteStartedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogSignInCompletedEvents(EventHandler<SignInEventArgs> eventHandler = null)
        {
            OktaContext.AddSignInCompletedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogSignInStartedEvents(EventHandler<SignInEventArgs> eventHandler = null)
        {
            OktaContext.AddSignInStartedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogSignOutCompletedEvents(EventHandler<SignOutEventArgs> eventHandler = null)
        {
            OktaContext.AddSignOutCompletedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }

        /// <inheritdoc/>
        public void LogSignOutStartedEvents(EventHandler<SignOutEventArgs> eventHandler = null)
        {
            OktaContext.AddSignOutStartedListener((sender, args) =>
            {
                if (eventHandler == null)
                {
                    this.DefaultInfoEventhandler(sender, args);
                }
                else
                {
                    eventHandler(sender, args);
                }
            });
        }
    }
}
