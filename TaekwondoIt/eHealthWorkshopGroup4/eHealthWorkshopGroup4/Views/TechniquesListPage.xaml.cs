using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using eHealthWorkshopGroup4.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eHealthWorkshopGroup4.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TechniquesListPage : ContentPage
	{
        public TechniquesListPage ()
		{
			InitializeComponent ();
        }

        public async void ListView_OnItemTapped(object x, ItemTappedEventArgs e)
        {
            Technique tech = e.Item as Technique;
            await Navigation.PushAsync(new TechniquePage(tech));
        }
	}
}