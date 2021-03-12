using Okta.Xamarin.Demo.ViewModels;
using Okta.Xamarin.Demo.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Okta.Xamarin.Demo
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
