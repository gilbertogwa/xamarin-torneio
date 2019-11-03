using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace TIFA.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "Reclassificar";

        }

        public ICommand OpenWebCommand { get; }
    }
}