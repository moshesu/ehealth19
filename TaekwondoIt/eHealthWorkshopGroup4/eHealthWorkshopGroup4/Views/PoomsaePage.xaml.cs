using CloudStorage;
using eHealthWorkshopGroup4.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eHealthWorkshopGroup4.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PoomsaePage : ContentPage
	{
        Poomsae poomsae;
        CloudStorageHandler storage = null;

        public PoomsaePage (Poomsae poomsae)
		{
			InitializeComponent ();
            this.poomsae = poomsae;
            storage = new CloudStorageHandler();
        }

        protected async override void OnAppearing()
        {
            Title = poomsae.name.ToString();
            minRank.Text = poomsae.minRank.ToString();
            XP.Text = poomsae.xpBonus.ToString();
            Stream imageStream = await storage.getPoomsaeImage(poomsae);
            PoomsaeImg.Source = ImageSource.FromStream(() => imageStream);
        }

        private async void CheckMarkHendler(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            var response = await DisplayAlert("App message", "Did You finished practice the poomsae?", "yes", "no");
            if (response)
            {
                await button.ScaleTo(0, 300, Easing.SinInOut);
                animationView.IsVisible = true;
                animationView.Play();
                App.MyXP += poomsae.xpBonus;
                await Task.WhenAll(
                    storage.incrUserXPAsync(App.MyUserName, poomsae.xpBonus),
                    Task.Delay(1200)
                );
                await Navigation.PushAsync(new PoomsaesListPage());
            }

        }

    }
}