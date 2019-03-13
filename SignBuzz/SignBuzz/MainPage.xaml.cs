using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SignBuzz.Solo;
using Microsoft.WindowsAzure.MobileServices;

namespace SignBuzz
{
    public partial class MainPage : ContentPage
    {
        // Track whether the user has authenticated.
        bool authenticated = false;

        public MainPage()
        {
            InitializeComponent();
            menuGrid.IsVisible = authenticated;

        }
        int userLevel = 1;
        String userName = "";
        int userPrizes = 0;
        String userPhoto = " ";
        
        async void playsolo(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new MediaPage());
            await Navigation.PushAsync(new StartSolo());
        }
        async void playtwo(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Compete.CompetePage());
            //Application.Current.MainPage = ;

        }
        async void profile(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new MediaPage());
            await Navigation.PushAsync(new TabbedPage1());
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            
            if (App.user != null)
            {
                Console.WriteLine(App.user.UserId);
                List<User> items = await MainUserManager.DefaultManager.CurrentUserTable
                        .Where(user => user.UserId == App.userId)
                        .ToListAsync();
                if (items.Count > 0)
                {
                    photo.Source = items[0].Image;

                }
            }
            nameEntry.IsVisible = false;
            Submit.IsVisible = false;
            // Refresh items only when authenticated.
            if (authenticated == true)
            {

                // Hide the Sign-in button.
                loginButton.IsVisible = false;
                menuGrid.IsVisible = true;
                loginButtonFacebook.IsVisible = false;
               

            }
            else
            {
                loginButton.IsVisible = true;
                menuGrid.IsVisible = false;
                loginButtonFacebook.IsVisible = true;
            }
        }
        
        async void submit(object sender, EventArgs e)
        {
            Busy();
            await MainUserManager.DefaultManager.SaveUserAsync(new User { UserId = App.user.UserId , Name = nameEntry.Text , Stage = 1, Prizes=0 , Image= "https://cdn.pixabay.com/photo/2013/07/13/10/07/man-156584__340.png" });
            await MainUserManager.DefaultManager.SaveUserGame2Async(new User_game2 { UserId = App.user.UserId, Ex1_g2 = 0, Stage = 2, Ex2_g2 = 0, Ex3_g2 = 0, Ex4_g2 = 0 , Ex5_g2 = 0 });
            await MainUserManager.DefaultManager.SaveUserGame3Async(new User_game3 { UserId = App.user.UserId, Ex1_g3 = 0, Stage = 2, Ex2_g3 = 0, Ex3_g3 = 0, Ex4_g3 = 0, Ex5_g3 = 0 });
            await MainUserManager.DefaultManager.SaveUserGameAsync(new User_game { UserId = App.user.UserId, Ex1_g1 = 0,  Ex2_g1 = 0, Ex3_g1 = 0, Ex4_g1 = 0, Ex5_g1 = 0 ,Ex6_g1 = 0, Ex7_g1 = 0, Ex8_g1 = 0, Ex9_g1 = 0, Ex10_g1 = 0, Ex11_g1 = 0, Ex12_g1 = 0, Ex13_g1 = 0, Ex14_g1 = 0, Ex15_g1 = 0, Ex16_g1 = 0, Ex17_g1 = 0, Ex18_g1 = 0, Ex19_g1 = 0, Ex20_g1 = 0, Ex21_g1 = 0, Ex22_g1 = 0, Ex23_g1 = 0, Ex24_g1 = 0, Ex25_g1 = 0, Ex26_g1 = 0 });
            this.userName = nameEntry.Text;
            this.userLevel = 1;
            this.userPrizes = 0;
            this.userPhoto = "https://cdn.pixabay.com/photo/2013/07/13/10/07/man-156584__340.png";
            photo.Source = "https://cdn.pixabay.com/photo/2013/07/13/10/07/man-156584__340.png";
            NotBusy();
            nameEntry.IsVisible = false;
            Submit.IsVisible = false;
            mainPageText.Text = "Choose what's next";
            menuGrid.IsVisible = true;
            loginGrid.IsVisible = false;

        }
        async void loginButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
            {
                Busy();
                mainPageText.FontSize = 30;
                mainPageText.Text = "We will start shortly";
                authenticated = await App.Authenticator.Authenticate(true);
                if (authenticated == true)
                {
                    List<User> items = await MainUserManager.DefaultManager.CurrentUserTable
                        .Where(user => user.UserId == App.userId)
                        .ToListAsync();
                    // user first time access the application
                    if (items.Count == 0)
                    {
                        mainPageText.Text = "We're Almost there..";
                        nameEntry.IsVisible = true;
                        Submit.IsVisible = true;
                        menuGrid.IsVisible = false;

                    }
                    else
                    {
                        mainPageText.FontSize = 30;
                        this.userName = items[0].Name;
                        this.userLevel = items[0].Stage;
                        this.userPrizes = items[0].Prizes;
                        this.userPhoto = items[0].Image;
                        photo.Source = items[0].Image;
                        mainPageText.Text = "Welcome " + items[0].Name;
                        nameEntry.IsVisible = false;
                        Submit.IsVisible = false;
                        menuGrid.IsVisible = true;
                    }
                    NotBusy();
                    loginGrid.IsVisible = false;



                }
            }
        }
        async void loginButton_Clicked_Facebook(object sender, EventArgs e)
        {
            Busy();
            mainPageText.Text = "We will start shortly";
            mainPageText.FontSize = 30;
            authenticated = await App.Authenticator.Authenticate(false);
            if (authenticated == true)
            {
                List<User> items = await MainUserManager.DefaultManager.CurrentUserTable
                    .Where(user => user.UserId == App.user.UserId)
                    .ToListAsync();
                // user first time access the application
                if (items.Count == 0)
                {
                    mainPageText.Text = "We're Almost there..";
                    nameEntry.IsVisible = true;
                    Submit.IsVisible = true;
                    menuGrid.IsVisible = false;

                }
                else
                {
                    mainPageText.FontSize = 30;
                    mainPageText.Text = "Welcome " + items[0].Name;
                    this.userName = items[0].Name;
                    this.userLevel = items[0].Stage;
                    this.userPrizes = items[0].Prizes;
                    this.userPhoto = items[0].Image;
                    photo.Source = items[0].Image;
                    nameEntry.IsVisible = false;
                    Submit.IsVisible = false;
                    menuGrid.IsVisible = true;
                }
                NotBusy();
                loginGrid.IsVisible = false;
                
            }
        }
        public void Busy()
        {
            uploadIndicator.IsVisible = true;
            uploadIndicator.IsRunning = true;
            nameEntry.IsVisible = false;
            Submit.IsVisible = false;
            menuGrid.IsVisible = false;
            loginGrid.IsVisible = false;
        }

        public void NotBusy()
        {
            uploadIndicator.IsVisible = false;
            uploadIndicator.IsRunning = false;
        }
    }
}
