﻿// <copyright file="ServiceProvider.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Okta.Xamarin.Ioc;
using Okta.Xamarin.Oie.Data;
using Okta.Xamarin.Oie.Client;
using Okta.Xamarin.Oie.Client.Data;
using Okta.Xamarin.Oie.Client.View;
using Okta.Xamarin.Oie.Logging;
using Okta.Xamarin.Oie.Session;
using Okta.Xamarin.Oie.Views;

namespace Okta.Xamarin.Oie
{
    public class ServiceProvider : IServiceProvider
    {
        private static ServiceProvider defaultServiceProvider;

        private static readonly object defaultLock = new object();

        private readonly TinyIoCContainer container = new TinyIoCContainer();

        public static ServiceProvider Default
        {
            get
            {
                if (defaultServiceProvider == null)
                {
                    lock (defaultLock)
                    {
                        if (defaultServiceProvider == null)
                        {
                            ServiceProvider temp = new ServiceProvider();
                            temp.RegisterService<IServiceProvider>(temp);
                            temp.RegisterService<IIdentityClient>(new IdentityClient());
                            temp.RegisterService<IIdentityViewModelProvider, IdentityViewModelProvider>();
                            temp.RegisterService<IIdentityDataProvider, IdentityDataProvider>();
                            temp.RegisterService<ISessionProvider>(new SecureSessionProvider());
                            temp.RegisterService<ILoggingProvider>(new LoggingProvider());
                            temp.RegisterService<IStorageProvider, FileStorageProvider>();
                            temp.RegisterService<IViewProvider>(new ViewProvider());
                            defaultServiceProvider = temp;
                        }
                    }
                }

                return defaultServiceProvider;
            }
        }

        public IType GetService<IType>()
            where IType : class
        {
            return this.container.Resolve<IType>();
        }

        public void RegisterService<IType, CType>()
            where IType : class
            where CType : class, IType
        {
            this.container.Register<IType, CType>();
        }

        public void RegisterService<IType>(IType implementation)
            where IType : class
        {
            this.container.Register<IType>(implementation);
        }
    }
}
