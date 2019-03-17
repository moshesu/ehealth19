using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SignBuzz.Compete
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompetePage : CarouselPage
    {
		public CompetePage ()
		{
			InitializeComponent ();
		}
        public int[] questions_array = new int [26];
        int[] arr = { 0 };
        protected async override void OnAppearing()
        {
            if (this.arr[0] == 0)
            {
                ShowingTimer();

            }
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
            if (count == 26)
            {
                List<User> users = await MainUserManager.DefaultManager.CurrentUserTable
                    .Where(user => user.UserId == App.userId)
                    .ToListAsync();
                users[0].Stage = 2;
                users[0].Prizes = 1;
                await MainUserManager.DefaultManager.UpdateUserAsync(users[0]);
                await DisplayAlert("Awesome!", "You complete the second stage", "OK");
            }
        }
        public int checkRes()
        {
            int res = 0;
            for (int i = 0 ; i < questions_array.Length ; i++)
            {
                if (questions_array[i] == 1)
                {
                    res = res + 10;
                }
            }
            return res;
        }
        private async void ShowingTimer()
        {

            int _end = 0;
            for (int _minute = 0; _minute >= 0; _minute--)
            {
                for (int _second = 30; _second >= 0; _second--)
                {
                    
                    if (_second == 0 && _minute == 0)
                    {
                        int res = checkRes();
                        await Navigation.PushModalAsync(new Submit(res));
                    }
                    if (_second < 10)
                    {
                        _secondView1.Text = Convert.ToString("0" + _second);
                        _secondView2.Text = Convert.ToString("0" + _second);

                    }
                    else
                    {
                        _secondView1.Text = Convert.ToString(_second);
                        _secondView2.Text = Convert.ToString(_second);

                    }
                    _minuteView1.Text = Convert.ToString("0" + _minute);
                    _minuteView2.Text = Convert.ToString("0" + _minute);

                    await Task.Delay(1000);
                }


                _end++;
                if (_end == 1) { break; }


            }

        }
        async void takePicture(object sender, EventArgs e)
        {

            ImageButton btn = (ImageButton)sender;
            Console.WriteLine(btn.ClassId);
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
            await Navigation.PushAsync(new MediaPage(btn.ClassId, null , 0 , this.arr ,questions_array));
        }
    }
}
	