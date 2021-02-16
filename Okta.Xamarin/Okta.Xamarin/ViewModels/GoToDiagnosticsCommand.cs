using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Okta.Xamarin.ViewModels
{
    public class GoToDiagnosticsCommand : Command
    {
        public GoToDiagnosticsCommand()
            : base(async () => await Shell.Current.GoToAsync("DiagnosticsPage"))
        {
        }
    }
}
