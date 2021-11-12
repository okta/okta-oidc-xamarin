namespace Okta.Xamarin.Widget.Pipeline.View
{
    public interface IViewPresenter
    {
        PresentResult Present(object state = null);
    }
}
