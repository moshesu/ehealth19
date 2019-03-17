using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SignBuzz.Solo.Game1
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameOnePage : CarouselPage
    {
        public GameOnePage()
        {
            InitializeComponent();
            //btn.Image = (Xamarin.Forms.FileImageSource)ImageSource.FromResource("splash.png");
        }
        public static int[] questions_array = { StartSolo.ex1_g1 , StartSolo.ex2_g1 , StartSolo.ex3_g1 , StartSolo.ex4_g1
        ,StartSolo.ex5_g1,StartSolo.ex6_g1, StartSolo.ex7_g1, StartSolo.ex8_g1, StartSolo.ex9_g1 ,StartSolo.ex10_g1, StartSolo.ex11_g1, StartSolo.ex12_g1, StartSolo.ex13_g1
        ,StartSolo.ex14_g1,StartSolo.ex15_g1, StartSolo.ex16_g1, StartSolo.ex17_g1, StartSolo.ex18_g1, StartSolo.ex19_g1,
        StartSolo.ex20_g1 , StartSolo.ex21_g1 ,StartSolo.ex22_g1 ,StartSolo.ex23_g1 , StartSolo.ex24_g1 ,StartSolo.ex25_g1, StartSolo.ex26_g1};
        protected async override void OnAppearing()
        {
            ImageButton[] arr = { a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z };
            int count = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (questions_array[i] == 1)
                {
                    count++;
                    arr[i].Opacity = 0.5;
                }

            }
            if (count == 26 && StartSolo.level == 1)
            {
                List<User> users = await MainUserManager.DefaultManager.CurrentUserTable
                    .Where(user => user.UserId == App.userId)
                    .ToListAsync();
                users[0].Stage = 2;
                users[0].Prizes = 1;
                await MainUserManager.DefaultManager.UpdateUserAsync(users[0]);
                await DisplayAlert("Awesome!", "You have completed the first stage", "OK");
            }
        }
        async void takePicture(object sender, EventArgs e)
        {

            ImageButton btn = (ImageButton)sender;
            int vidIndex = 0;
            switch (btn.ClassId)
            {
                case "A":
                    vidIndex = 0;
                    break;
                case "B":
                    vidIndex = 1;
                    break;
                case "C":
                    vidIndex = 2;
                    break;
                case "D":
                    vidIndex = 3;
                    break;
                case "E":
                    vidIndex = 4;
                    break;
                case "F":
                    vidIndex = 5;
                    break;
                case "G":
                    vidIndex = 6;
                    break;
                case "H":
                    vidIndex = 7;
                    break;
                case "I":
                    vidIndex = 8;
                    break;
                case "J":
                    vidIndex = 9;
                    break;
                case "K":
                    vidIndex = 10;
                    break;
                case "L":
                    vidIndex = 11;
                    break;
                case "M":
                    vidIndex = 12;
                    break;
                case "N":
                    vidIndex = 13;
                    break;
                case "O":
                    vidIndex = 14;
                    break;
                case "P":
                    vidIndex = 15;
                    break;
                case "Q":
                    vidIndex = 16;
                    break;
                case "R":
                    vidIndex = 17;
                    break;
                case "S":
                    vidIndex = 18;
                    break;
                case "T":
                    vidIndex = 19;
                    break;
                case "U":
                    vidIndex = 20;
                    break;
                case "V":
                    vidIndex = 21;
                    break;
                case "W":
                    vidIndex = 22;
                    break;
                case "X":
                    vidIndex = 23;
                    break;
                case "Y":
                    vidIndex = 24;
                    break;
                default:
                    vidIndex = 25;
                    break;
            }
            await Navigation.PushAsync(new LetterPage(vidIndex));
        }
    }
}
    