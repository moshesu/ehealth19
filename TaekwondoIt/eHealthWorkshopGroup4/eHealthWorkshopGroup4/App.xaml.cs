using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using eHealthWorkshopGroup4.Services;
using eHealthWorkshopGroup4.Views;
using eHealthWorkshopGroup4.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace eHealthWorkshopGroup4
{
    public partial class App : Application
    {
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        public static string AzureBackendUrl = "http://localhost:5000";
        public static bool UseMockDataStore = true;
        public static bool IsCoach = false;
        public static string MyUserName { get; set; } = "";
        public static int MyXP = 0;
        public static Rank MyRank = Rank.Beginner;

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LogInPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
