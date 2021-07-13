using Okta.Xamarin.Demo.Services;
using Okta.Xamarin.Demo.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Okta.Xamarin.Demo
{
    public partial class OktaDemoApp : Application
    {

        public OktaDemoApp()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new OktaDemoAppShell();
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
