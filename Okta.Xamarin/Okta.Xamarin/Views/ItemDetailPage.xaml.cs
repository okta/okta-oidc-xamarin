using System.ComponentModel;
using Xamarin.Forms;
using Okta.Xamarin.ViewModels;

namespace Okta.Xamarin.Views
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
