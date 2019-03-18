using CloudStorage;
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
    public partial class TechniquePage : ContentPage
    {

        Technique tech;
        CloudStorageHandler storage;

        public TechniquePage(Technique tech)
        {
            InitializeComponent();
            this.tech = tech;
            storage = new CloudStorageHandler();
        }

        protected override void OnAppearing()
        {
            Title = tech.name;
            minRank.Text = tech.minRank.ToString();
            XP.Text = tech.xp.ToString();
            techniqueImg.Source = tech.imageSorce;
            techniqueNote.Text = tech.note;
        }

        private async void CheckMarkHendler (object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            var response = await DisplayAlert("App message", "Did You finished practice the technique?", "yes", "no");
            if (response)
            {
                await button.ScaleTo(0, 300, Easing.SinInOut);
                animationView.IsVisible = true;
                animationView.Play();
                App.MyXP += tech.xp;
                await Task.WhenAll(
                    storage.incrUserXPAsync(App.MyUserName, tech.xp),
                    Task.Delay(1200)
                );
                await Navigation.PushAsync(new TechniquesListPage());
            }
            
            
        }
    }
}