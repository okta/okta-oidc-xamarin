using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Okta.Xamarin.Demo.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

		private async Task OnSignInClicked(object sender, EventArgs e)
		{
			_ = Shell.Current.GoToAsync("DemoPage");
		}
    }
}