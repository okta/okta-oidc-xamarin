﻿using System;
using System.Collections.Generic;
using Okta.Xamarin.ViewModels;
using Okta.Xamarin.Views;
using Xamarin.Forms;

namespace Okta.Xamarin
{
    public partial class OktaAppShell : Shell
    {
        public OktaAppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(DiagnosticsPage), typeof(DiagnosticsPage));
        }

        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            _ = OktaContext.Current.SignOutAsync();
        }
    }
}
