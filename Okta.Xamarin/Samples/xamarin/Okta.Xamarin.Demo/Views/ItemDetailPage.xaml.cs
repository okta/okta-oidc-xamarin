using Okta.Xamarin.Demo.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Okta.Xamarin.Demo.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}