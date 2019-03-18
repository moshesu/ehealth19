using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace eHealthWorkshopGroup4.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            //Title = "About Our App";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://docs.google.com/document/d/1Iy3nmmb1cG6sRpm18xTsl1cq7OS2PpXScxguADDBSI0/edit?usp=sharing")));
        }

        public ICommand OpenWebCommand { get; }
    }
}