using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SleepItOff
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignOutPage : ContentPage
	{
		public SignOutPage ()
		{
			InitializeComponent ();
            NavigationPage.SetHasBackButton(this, false);

        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

    }
}