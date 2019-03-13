using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using System.Net.Http;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SignBuzz
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate(bool google);
    }

    public partial class App : Application
    {
        public static IAuthenticate Authenticator { get; private set; }
        public static MobileServiceUser user { get; set; }
        public static String userId { get; set; }
        public static MobileServiceClient appClient = new MobileServiceClient("https://signbuzz.azurewebsites.net");
        public static readonly HttpClient client = new HttpClient();

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            //MainPage = new TodoList();

        }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }

        protected override void OnStart()
        {
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
