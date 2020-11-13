using Okta.Xamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Okta.Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = new ProfileViewModel(this);
			if(Instance == null)
			{
				Instance = this;
			}
        }

		public static ProfilePage Instance { get; set; }

		public void SetClaims(Dictionary<string, object> claims)
		{
			StackLayout claimsLayout = (StackLayout)this.FindByName("Claims");
			claimsLayout.Children.Clear();
			foreach (string key in claims.Keys)
			{
				Label label = new Label { Text = key };
				label.FontSize = Device.GetNamedSize(NamedSize.Medium, label);
				Label value = new Label { Text = claims[key]?.ToString() };
				value.FontSize = Device.GetNamedSize(NamedSize.Small, value);
				
				claimsLayout.Children.Add(label);
				claimsLayout.Children.Add(value);
			}
		}
    }
}
