using SignBuzz.Solo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SignBuzz.Solo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartSolo : ContentPage
    {
        public StartSolo()
        {
            InitializeComponent();
        }
        static public int ex1_g1 = 0;
        static public int ex2_g1 = 0;
        static public int ex3_g1 = 0;
        static public int ex4_g1 = 0;
        static public int ex5_g1 = 0;
        static public int ex6_g1 = 0;
        static public int ex7_g1 = 0;
        static public int ex8_g1 = 0;
        static public int ex9_g1 = 0;
        static public int ex10_g1 = 0;
        static public int ex11_g1 = 0;
        static public int ex12_g1 = 0;
        static public int ex13_g1 = 0;
        static public int ex14_g1 = 0;
        static public int ex15_g1 = 0;
        static public int ex16_g1 = 0;
        static public int ex17_g1 = 0;
        static public int ex18_g1 = 0;
        static public int ex19_g1 = 0;
        static public int ex20_g1 = 0;
        static public int ex21_g1 = 0;
        static public int ex22_g1 = 0;
        static public int ex23_g1 = 0;
        static public int ex24_g1 = 0;
        static public int ex25_g1 = 0;
        static public int ex26_g1 = 0;
        static public int ex1_g2 = 0;
        static public int ex2_g2 = 0;
        static public int ex3_g2 = 0;
        static public int ex4_g2 = 0;
        static public int ex5_g2 = 0;
        static public int ex1_g3 = 0;
        static public int ex2_g3 = 0;
        static public int ex3_g3 = 0;
        static public int ex4_g3 = 0;
        static public int ex5_g3 = 0;
        static public int level = 1;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Busy();
            List<User> users = await MainUserManager.DefaultManager.CurrentUserTable
                    .Where(user => user.UserId == App.userId)
                    .ToListAsync();
            level = users[0].Stage;
            List<User_game> items_1 = await MainUserManager.DefaultManager.CurrentUser_GameTable
                        .Where(user => user.UserId == App.userId)
                        .ToListAsync();
            ex1_g1 = items_1[0].Ex1_g1;
            ex2_g1 = items_1[0].Ex2_g1;
            ex3_g1 = items_1[0].Ex3_g1;
            ex4_g1 = items_1[0].Ex4_g1;
            ex5_g1 = items_1[0].Ex5_g1;
            ex6_g1 = items_1[0].Ex6_g1;
            ex7_g1 = items_1[0].Ex7_g1;
            ex8_g1 = items_1[0].Ex8_g1;
            ex9_g1 = items_1[0].Ex9_g1;
            ex10_g1 = items_1[0].Ex10_g1;
            ex11_g1 = items_1[0].Ex11_g1;
            ex12_g1 = items_1[0].Ex12_g1;
            ex13_g1 = items_1[0].Ex13_g1;
            ex14_g1 = items_1[0].Ex14_g1;
            ex15_g1 = items_1[0].Ex15_g1;
            ex16_g1 = items_1[0].Ex16_g1;
            ex17_g1 = items_1[0].Ex17_g1;
            ex18_g1 = items_1[0].Ex18_g1;
            ex19_g1 = items_1[0].Ex19_g1;
            ex20_g1 = items_1[0].Ex20_g1;
            ex21_g1 = items_1[0].Ex21_g1;
            ex22_g1 = items_1[0].Ex22_g1;
            ex23_g1 = items_1[0].Ex23_g1;
            ex24_g1 = items_1[0].Ex24_g1;
            ex25_g1 = items_1[0].Ex25_g1;
            ex26_g1 = items_1[0].Ex26_g1;
            List<User_game2> items = await MainUserManager.DefaultManager.CurrentUser_Game2Table
                        .Where(user => user.UserId == App.userId)
                        .ToListAsync();
            ex1_g2 = items[0].Ex1_g2;
            ex2_g2 = items[0].Ex2_g2;
            ex3_g2 = items[0].Ex3_g2;
            ex4_g2 = items[0].Ex4_g2;
            ex5_g2 = items[0].Ex5_g2;
            List<User_game3> items_3 = await MainUserManager.DefaultManager.CurrentUser_Game3Table
                       .Where(user => user.UserId == App.userId)
                       .ToListAsync();
            ex1_g3 = items_3[0].Ex1_g3;
            ex2_g3 = items_3[0].Ex2_g3;
            ex3_g3 = items_3[0].Ex3_g3;
            ex4_g3 = items_3[0].Ex4_g3;
            ex5_g3 = items_3[0].Ex5_g3;
            NotBusy();
            if (level == 3)
            {
                Two.IsEnabled = true;
                Three.IsEnabled = true;
            }
            else if (level == 2)
            {
                Two.IsEnabled = true;
            }
            else if (level == 4)
            {
                Two.IsEnabled = true;
                Three.IsEnabled = true;
                finishGame.IsVisible = true;
            }
        }
        public void Busy()
        {
            One.IsEnabled = false;
            Debug_mode.IsEnabled = false;
            Two.IsEnabled = false;
            Three.IsEnabled = false;
            uploadIndicator.IsVisible = true;
            uploadIndicator.IsRunning = true;
           
        }

        public void NotBusy()
        {
            Debug_mode.IsEnabled = true;
            One.IsEnabled = true;
            uploadIndicator.IsVisible = false;
            uploadIndicator.IsRunning = false;
        }

        async void startGameOne(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new MediaPage());
            await Navigation.PushAsync(new Game1.GameOnePage());
        }
        async void startGameTwo(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new MediaPage());
            await Navigation.PushAsync(new Game2.MasterDetailPage1());
        }
        async void startGameThree(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new MediaPage());
            await Navigation.PushAsync(new Game3.IstructionsGameThree());
        }
        async void debug_mode(object sender, EventArgs e)
        {
            Two.IsEnabled = true;
            Three.IsEnabled = true;
        }
    }
}