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
    public partial class LogInPage : ContentPage
    {
        CloudStorageHandler storage = null;
        public LogInPage()
        {
            InitializeComponent();
            storage = new CloudStorageHandler();
        }

        private async void LogInHendler(object sender, EventArgs e)
        {
            string uname = userNameEntry.Text;
            string pword = passwordEntry.Text;
            if(uname == null||pword == null||uname ==""||pword == "")
            {
                if(uname == null || uname == "") 
                userNameEntry.BackgroundColor = Color.FromHex("ff5a5a");
                if(pword == null || pword == "")
                passwordEntry.BackgroundColor = Color.FromHex("ff5a5a");
                await DisplayAlert("App message", "Please enter a username & password", "OK");
                return;
            }
            if (!await storage.isUsernameTaken(uname))
            {
                userNameEntry.BackgroundColor = Color.FromHex("ff5a5a");
                await DisplayAlert("App message","Username not exist. please reenter username.", "OK");
                return;
            }

            try
            {
                await storage.userPasswordQueryAsync(uname, pword);
            }
            catch (WrongPasswordException)
            {
                passwordEntry.BackgroundColor = Color.FromHex("ff5a5a");
                await DisplayAlert("App message", "Worng password. Please try again.", "OK");
                return;
            }

            App.MyUserName = uname;
            var x = await storage.getUserProfileData(uname);
            App.IsCoach = x.isCoach;
            App.MyXP = x.xp;
            App.MyRank = x.rank;
            App.Current.MainPage = new MainPage();

        }

        private async void signUpHendler(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SingUpPage());
        }

        private void Handle_TextChanged(object sender, EventArgs e)
        {
            Entry entry = sender as Entry;
            entry.BackgroundColor = Color.Transparent;
        }

        private void switch_onToggled(object sender, ToggledEventArgs e)
        {
            passwordEntry.IsPassword = !e.Value;
        }
    }
}