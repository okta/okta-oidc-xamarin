// <copyright file="IServiceProvider.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

namespace Okta.Xamarin.Widget.Pipeline
{
    public interface IServiceProvider
    {
        void RegisterService<IType, CType>()
            where IType : class 
            where CType : class, IType;

        void RegisterService<IType>(IType implementation) where IType : class;

        IType GetService<IType>() where IType : class;
    }
}
