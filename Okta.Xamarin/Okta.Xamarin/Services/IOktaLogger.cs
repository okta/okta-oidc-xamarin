using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.Xamarin.Services
{
    /// <summary>
    /// An interface defining Okta specific event subscription handling methods.
    /// </summary>
    public interface IOktaLogger : ILogger
    {
        /// <summary>
        /// Subscribes to and logs all relevant events.
        /// </summary>
        void LogAllEvents();

        /// <summary>
        /// Subscribes to and logs `ReqeustException` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogRequestExceptionEvents(EventHandler<RequestExceptionEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `LoadStateStarted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogLoadStateStartedEvents(EventHandler<SecureStorageEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `LoadStateCompleted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogLoadStateCompletedEvents(EventHandler<SecureStorageEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `LoadStateException` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogLoadStateExceptionEvents(EventHandler<SecureStorageExceptionEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `SecureStorageStarted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogSecureStorageWriteStartedEvents(EventHandler<SecureStorageEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `SecureStorageWriteCompleted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogSecureStorageWriteCompletedEvents(EventHandler<SecureStorageEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `SecureStorageWriteException` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogSecureStorageWriteExceptionEvents(EventHandler<SecureStorageExceptionEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `SecureStorageReadStarted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogSecureStorageReadStartedEvents(EventHandler<SecureStorageEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `SecureStorageReadCompleted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogSecureStorageReadCompletedEvents(EventHandler<SecureStorageEventArgs> eventHandler = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogSecureStorageReadExceptionEvents(EventHandler<SecureStorageExceptionEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `SignInStarted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogSignInStartedEvents(EventHandler<SignInEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `SignInCompleted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogSignInCompletedEvents(EventHandler<SignInEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `SignOutStarted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogSignOutStartedEvents(EventHandler<SignOutEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `SignOutCompleted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogSignOutCompletedEvents(EventHandler<SignOutEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `AuthenticationFailed` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogAuthenticationFailedEvents(EventHandler<AuthenticationFailedEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `RevokeStarted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogRevokeStartedEvents(EventHandler<RevokeEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `RevokeCompleted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogRevokeCompletedEvents(EventHandler<RevokeEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `RevokeException` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogRevokeExceptionEvents(EventHandler<RevokeExceptionEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `GetUserStarted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogGetUserStartedEvents(EventHandler<GetUserEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `GetUserCompleted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogGetUserCompletedEvents(EventHandler<GetUserEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `IntrospectStarted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogIntrospectStartedEvents(EventHandler<IntrospectEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `IntrospectCompleted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogIntrospectCompletedEvents(EventHandler<IntrospectEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `RenewStarted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogRenewStartedEvents(EventHandler<RenewEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `RenewCompleted` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogRenewCompletedEvents(EventHandler<RenewEventArgs> eventHandler = null);

        /// <summary>
        /// Subscribes to and logs `RenewException` events.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        void LogRenewExceptionEvents(EventHandler<RenewExceptionEventArgs> eventHandler = null);
    }
}
