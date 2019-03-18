using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eHealthWorkshopGroup4.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InfoPage : ContentPage
	{
		public InfoPage()
		{
			InitializeComponent ();
		}

        private async void TechniqueHandler(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TechniquesListPage());
        }

        private async void PoomsaesHandler(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PoomsaesListPage());
        }

    }
}