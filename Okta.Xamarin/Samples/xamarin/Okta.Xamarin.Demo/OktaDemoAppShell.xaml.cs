using Okta.Xamarin.Demo.ViewModels;
using Okta.Xamarin.Demo.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Okta.Xamarin.Demo
{
    public partial class OktaDemoAppShell : Shell
    {
        public OktaDemoAppShell()
        {
            InitializeComponent();
			Routing.RegisterRoute(nameof(DemoPage), typeof(DemoPage));
			Routing.RegisterRoute(nameof(IntrospectPage), typeof(IntrospectPage));
			Routing.RegisterRoute(nameof(RenewPage), typeof(RenewPage));
			Routing.RegisterRoute(nameof(RevokePage), typeof(RevokePage));
        }

        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            _ = OktaContext.Current.SignOutAsync();
        }
	}
}
