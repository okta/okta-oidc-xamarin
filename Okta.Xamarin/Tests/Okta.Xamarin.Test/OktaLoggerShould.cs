// <copyright file="OktaLoggerShould.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Okta.Xamarin.Services;
using Xunit;

namespace Okta.Xamarin.Test
{
    public class OktaLoggerShould
    {
        [Fact]
        public void LogLoadStateStartedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            bool? wasCalled = false;
            testLogger.LogLoadStateStartedEvents((sender, args) => wasCalled = true);
            testLogger.LogLoadStateStartedEvents(); // uses default event handler
            testOktaContext.RaiseLoadStateStartedEvent();

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogLoadStateCompletedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            bool? wasCalled = false;
            testLogger.LogLoadStateCompletedEvents((sender, args) => wasCalled = true);
            testLogger.LogLoadStateCompletedEvents(); // use default event handler
            testOktaContext.RaiseLoadCompletedEvent(new OktaStateManager());

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogLoadStateExceptionEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            Exception testException = new Exception("this is a test exception");
            bool? wasCalled = false;
            testLogger.LogLoadStateExceptionEvents((sender, args) =>
            {
                args.Exception.Should().Be(testException);
                wasCalled = true;
            });
            testLogger.LogLoadStateExceptionEvents(); // use default event handler
            testOktaContext.RaiseLoadStateExceptionEvent(testException);

            wasCalled.Should().BeTrue();
            mockLogger.Received().Error(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogSecureStorageWriteStartedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            bool? wasCalled = false;
            testLogger.LogSecureStorageWriteStartedEvents((sender, args) => wasCalled = true);
            testLogger.LogSecureStorageWriteStartedEvents(); // use default event handler
            testOktaContext.RaiseSecureStorageWriteStartedEvent();

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogSecureStorageWriteCompletedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            bool? wasCalled = false;
            testLogger.LogSecureStorageWriteCompletedEvents((sender, args) => wasCalled = true);
            testLogger.LogSecureStorageWriteCompletedEvents(); // use default event handler
            testOktaContext.RaiseSecureStorageWriteCompletedEvent();

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogSecureStorageWriteExceptionEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            Exception testException = new Exception("this is a test exception");
            bool? wasCalled = false;
            testLogger.LogSecureStorageWriteExceptionEvents((sender, args) =>
            {
                args.Exception.Should().Be(testException);
                wasCalled = true;
            });
            testLogger.LogSecureStorageWriteExceptionEvents(); // use default event handler
            testOktaContext.RaiseSecureStorageWriteExceptionEvent(testException);

            wasCalled.Should().BeTrue();
            mockLogger.Received().Error(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogSecureStorageReadStartedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            bool? wasCalled = false;
            testLogger.LogSecureStorageReadStartedEvents((sender, args) => wasCalled = true);
            testLogger.LogSecureStorageReadStartedEvents(); // use default event handler
            testOktaContext.RaiseSecureStorageReadStartedEvent();

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogSecureStorageReadCompletedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            bool? wasCalled = false;
            testLogger.LogSecureStorageReadCompletedEvents((sender, args) => wasCalled = true);
            testLogger.LogSecureStorageReadCompletedEvents(); // use default event handler
            testOktaContext.RaiseSecureStorageReadCompletedEvent();

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogSecureStorageReadExceptionEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            Exception testException = new Exception("this is a test exception");
            bool? wasCalled = false;
            testLogger.LogSecureStorageReadExceptionEvents((sender, args) =>
            {
                args.Exception.Should().Be(testException);
                wasCalled = true;
            });
            testLogger.LogSecureStorageReadExceptionEvents(); // use default event handler
            testOktaContext.RaiseSecureStorageReadExceptionEvent(testException);

            wasCalled.Should().BeTrue();
            mockLogger.Received().Error(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogRequestExceptionEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            Exception testException = new Exception("this is a test exception");
            bool? wasCalled = false;
            testLogger.LogRequestExceptionEvents((sender, args) =>
            {
                args.Exception.Should().Be(testException);
                wasCalled = true;
            });
            testLogger.LogRequestExceptionEvents(); // use default event handler
            testOktaContext.RaiseRequestExceptionEvent(testException);

            wasCalled.Should().BeTrue();
            mockLogger.Received().Error(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogSignInStartedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            bool? wasCalled = false;
            testLogger.LogSignInStartedEvents((sender, args) => wasCalled = true);
            testLogger.LogSignInStartedEvents(); // use default event handler
            testOktaContext.RaiseSignInStartedEvent();

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogSignInCompletedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            bool? wasCalled = false;
            testLogger.LogSignInCompletedEvents((sender, args) => wasCalled = true);
            testLogger.LogSignInCompletedEvents(); // use default event handler
            testOktaContext.RaiseSignInCompletedEvent();

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogSignOutStartedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            bool? wasCalled = false;
            testLogger.LogSignOutStartedEvents((sender, args) => wasCalled = true);
            testLogger.LogSignOutStartedEvents(); // use default event handler
            testOktaContext.RaiseSignOutStartedEvent();

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogSignOutCompletedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            bool? wasCalled = false;
            testLogger.LogSignOutCompletedEvents((sender, args) => wasCalled = true);
            testLogger.LogSignOutCompletedEvents(); // use default event handler
            testOktaContext.RaiseSignOutCompletedEvent();

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogAuthenticationFailedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            OAuthException testException = new OAuthException();
            bool? wasCalled = false;
            testLogger.LogAuthenticationFailedEvents((sender, args) =>
            {
                args.OAuthException.Should().Be(testException);
                wasCalled = true;
            });
            testLogger.LogAuthenticationFailedEvents(); // use default event handler
            testOktaContext.RaiseAuthenticationFailedEvent(testException);

            wasCalled.Should().BeTrue();
            mockLogger.Received().Error(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogRevokeStartedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            TokenKind tokenKind = TokenKind.AccessToken;
            string testToken = "this is a test token";
            bool? wasCalled = false;
            testLogger.LogRevokeStartedEvents((sender, args) =>
            {
                args.Token.Should().BeEquivalentTo(testToken);
                args.TokenKind.Should().Be(tokenKind);
                wasCalled = true;
            });
            testLogger.LogRevokeStartedEvents(); // use default event handler
            testOktaContext.RaiseRevokeStartedEvent(tokenKind, testToken);

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogRevokeCompletedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            TokenKind tokenKind = TokenKind.RefreshToken;
            bool? wasCalled = false;
            testLogger.LogRevokeCompletedEvents((sender, args) =>
            {
                args.TokenKind.Should().Be(tokenKind);
                wasCalled = true;
            });
            testLogger.LogRevokeCompletedEvents(); // use default event handler
            testOktaContext.RaiseRevokeCompletedEvent(tokenKind);

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogRevokeExceptionEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            TokenKind tokenKind = TokenKind.IdToken;
            Exception testException = new Exception("this is a test exception");

            bool? wasCalled = false;
            testLogger.LogRevokeExceptionEvents((sender, args) =>
            {
                args.Exception.Should().Be(testException);
                args.TokenKind.Should().Be(tokenKind);
                wasCalled = true;
            });
            testLogger.LogRevokeExceptionEvents(); // use default event handler
            testOktaContext.RaiseRevokeExceptionEvent(tokenKind, testException);

            wasCalled.Should().BeTrue();
            mockLogger.Received().Error(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogGetUserStartedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            bool? wasCalled = false;
            testLogger.LogGetUserStartedEvents((sender, args) => wasCalled = true);
            testLogger.LogGetUserStartedEvents(); // use default event handler
            testOktaContext.RaiseGetUserStartedEvent();

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogGetUserCompletedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            object testUserInfo = new object();
            bool? wasCalled = false;
            testLogger.LogGetUserCompletedEvents((sender, args) =>
            {
                args.UserInfo.Should().Be(testUserInfo);
                wasCalled = true;
            });
            testLogger.LogGetUserCompletedEvents(); // use default event handler
            testOktaContext.RaiseGetUserCompletedEvent(testUserInfo);

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogIntrospectStartedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            TokenKind tokenKind = TokenKind.IdToken;
            string testToken = "this is a test token";
            bool? wasCalled = false;
            testLogger.LogIntrospectStartedEvents((sender, args) =>
            {
                args.Token.Should().Be(testToken);
                args.TokenKind.Should().Be(tokenKind);
                wasCalled = true;
            });
            testLogger.LogIntrospectStartedEvents(); // use default event handler
            testOktaContext.RaiseIntrospectStartedEvent(tokenKind, testToken);

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogIntrospectCompletedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            TokenKind tokenKind = TokenKind.IdToken;
            Dictionary<string, object> testResponse = new Dictionary<string, object>();
            string testToken = "this is a test token";
            bool? wasCalled = false;
            testLogger.LogIntrospectCompletedEvents((sender, args) =>
            {
                args.Token.Should().Be(testToken);
                args.TokenKind.Should().Be(tokenKind);
                wasCalled = true;
            });
            testLogger.LogIntrospectCompletedEvents(); // use default event handler
            testOktaContext.RaiseIntrospectCompletedEvent(tokenKind, testToken, testResponse);

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogRenewStartedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            string refreshToken = "this is a test refresh token";
            string authServerId = "this is a test authorization server id";
            bool? wasCalled = false;
            testLogger.LogRenewStartedEvents((sender, args) => wasCalled = true);
            testLogger.LogRenewStartedEvents(); // use default event handler
            testOktaContext.RaiseRenewStartedEvent(refreshToken, true, authServerId);

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogRenewCompletedEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            string refreshToken = "this is a test refresh token";
            string authServerId = "this is a test authorization server id";
            RenewResponse renewResponse = new RenewResponse();
            bool? wasCalled = false;
            testLogger.LogRenewCompletedEvents((sender, args) => wasCalled = true);
            testLogger.LogRenewCompletedEvents(); // use default event handler
            testOktaContext.RaiseRenewCompletedEvent(refreshToken, true, authServerId, renewResponse);

            wasCalled.Should().BeTrue();
            mockLogger.Received().Info(Arg.Any<string>(), Arg.Any<object[]>());
        }

        [Fact]
        public void LogRenewExceptionEvent()
        {
            ILogger mockLogger = Substitute.For<ILogger>();
            TestOktaContext testOktaContext = new TestOktaContext();
            OktaContext.Current = testOktaContext;
            OktaLogger testLogger = new OktaLogger(mockLogger);
            Exception testException = new Exception("this is a test exception");
            bool? wasCalled = false;
            testLogger.LogRenewExceptionEvents((sender, args) =>
            {
                args.Exception.Should().Be(testException);
                wasCalled = true;
            });
            testLogger.LogRenewExceptionEvents(); // use default event handler
            testOktaContext.RaiseRenewException(testException);

            wasCalled.Should().BeTrue();
            mockLogger.Received().Error(Arg.Any<string>(), Arg.Any<object[]>());
        }

		[Fact]
		public void LogWarning()
		{
			ILogger mockLogger = Substitute.For<ILogger>();
			OktaLogger testLogger = new OktaLogger(mockLogger);			
			string format = "this is a test {0}";
			object[] args = new[] { "message" };
			testLogger.Warning(format, args);

			mockLogger.Received().Warning(format, args);
		}

		[Fact]
		public void LogFatal()
		{
			ILogger mockLogger = Substitute.For<ILogger>();
			OktaLogger testLogger = new OktaLogger(mockLogger);
			string format = "this is a test {0}";
			object[] args = new[] { "message" };
			testLogger.Fatal(format, args);

			mockLogger.Received().Fatal(format, args);
		}
    }
}
