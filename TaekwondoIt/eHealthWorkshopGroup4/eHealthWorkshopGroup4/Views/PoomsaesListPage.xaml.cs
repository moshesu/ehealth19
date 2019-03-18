using CloudStorage;
using eHealthWorkshopGroup4.Models;
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
	public partial class PoomsaesListPage : ContentPage
	{
        CloudStorageHandler storage;
        bool showList;
		public PoomsaesListPage ()
		{
			InitializeComponent ();
            storage = new CloudStorageHandler();
            showList = false;
        }

        private async void ListView_OnItemTapped(object x, ItemTappedEventArgs e)
        {
            Poomsae poomsae = e.Item as Poomsae;
            await Navigation.PushAsync(new PoomsaePage(poomsae));
        }

        private async void showPoomsaesList (object x, EventArgs e)
        {
            showList = !showList;
            if (showList)
            {
                
                PoomsaesListView.TranslationY = -500;
                PoomsaesListView.IsVisible = true;
                await PoomsaesListView.TranslateTo(0, 0, 500, Easing.SinIn);
                
            }
            else
            {
                PoomsaesListView.TranslationY = 0;
                await PoomsaesListView.TranslateTo(0, -500, 800, Easing.SinIn);
                PoomsaesListView.IsVisible = false;
            }
               
        }

        private async void PoomsaeForUser(object x, EventArgs e)
        {
            Poomsae poomsae = await storage.getPoomsaeForUser(App.MyUserName);
            await Navigation.PushAsync(new PoomsaePage(poomsae));
        }
    }
}