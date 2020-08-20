using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Okta.Xamarin.ViewModels
{
	public partial class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenOktaApiReferenceCommand = new Command(async () => await Browser.OpenAsync("https://developer.okta.com/docs/reference/"));
            SignInCommand = new Command(async () => await SignIn());
        }

        public ICommand OpenOktaApiReferenceCommand { get; }
		public ICommand SignInCommand { get; set; }

		public async Task SignIn()
		{
			await OktaContext.Current.SignIn();
		}
	}
}
