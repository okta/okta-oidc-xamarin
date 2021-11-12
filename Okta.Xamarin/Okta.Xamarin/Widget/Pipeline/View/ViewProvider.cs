// <copyright file="ViewProvider.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace Okta.Xamarin.Widget.Pipeline.View
{
    public class ViewProvider : IViewProvider
    {
        public IViewRenderer ViewRenderer { get; set; }

        public IViewPresenter ViewPresenter { get; set; }

        // OKTA-439268
        public PresentResult Present(object state = null)
        {
            throw new NotImplementedException();
        }

        public RenderResult Render(object state = null)
        {
            throw new NotImplementedException();
        }
        // - OKTA-439268
    }
}
