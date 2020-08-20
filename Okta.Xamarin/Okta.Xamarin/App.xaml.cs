using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Okta.Xamarin.Services;
using Okta.Xamarin.Views;

namespace Okta.Xamarin
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
