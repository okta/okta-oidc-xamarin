// <copyright file="IViewProvider.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

namespace Okta.Xamarin.Oie.Views
{
    public interface IViewProvider
    {
        IViewRenderer ViewRenderer { get; }

        IViewPresenter ViewPresenter { get; }

        PresentResult Present(object state = null);

        RenderResult Render(object state = null);
    }
}
