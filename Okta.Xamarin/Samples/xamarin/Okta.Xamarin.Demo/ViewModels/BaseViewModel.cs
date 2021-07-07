using Okta.Xamarin.Demo.Models;
using Okta.Xamarin.Demo.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Okta.Xamarin.Demo.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
		#endregion

		#region Okta
		public virtual ContentPage Page { get; set; }

		public virtual IOktaStateManager OktaStateManager { get; set; }

		public string AccessToken { get => OktaContext.AccessToken; }
		public string IdToken { get => OktaContext.IdToken; }
		public string RefreshToken { get => OktaContext.RefreshToken; }

		protected void OnSignInCompleted(object sender, EventArgs e)
		{
			this.OktaStateManager = ((SignInEventArgs)e).StateManager;
		}

		protected void OnSignOutCompleted(object sender, EventArgs e)
		{
			this.OktaStateManager = ((SignOutEventArgs)e).StateManager;
		}

		protected void ShowWorkingImage(string imageName = "WorkingImage")
		{
			Image image = Page?.FindByName<Image>(imageName);
			if(image != null)
			{
				image.IsAnimationPlaying = true;
				image.IsVisible = true;
				Thread.Sleep(300);
			}
		}

		protected void HideWorkingImage(string imageName = "WorkingImage")
		{
			Image image = Page?.FindByName<Image>(imageName);
			if(image != null)
			{
				image.IsVisible = false;
			}
		}
		#endregion
	}
}
