namespace Okta.Xamarin.Oie.Views
{
    public interface IViewPresenter
    {
        PresentResult Present(object state = null);
    }
}
