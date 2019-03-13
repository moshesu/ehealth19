using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SignBuzz.Solo.Game3
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPage2 : TabbedPage
    {
        public TabbedPage2()
        {
            CurrentPageChanged += tabChanged;
            InitializeComponent();
        }
        int[] firstQ_array = { 0, 0, 0 };
        int[] secQ_array = { 0, 0, 0 };
        int[] thirdQ_array = { 0, 0, 0, 0, 0 };
        int[] fourthQ_array = { 0, 0, 0, 0, 0, 0 };
        int[] fifthQ_array = { 0, 0, 0, 0, 0 };
        int currQ = 0;
        int[] end = { 0, 0, 0, 0, 0 };
        int[] fromMediaPage = { 1 };
        static int[] qustions_array = { StartSolo.ex1_g3, StartSolo.ex2_g3, StartSolo.ex3_g3, StartSolo.ex4_g3, StartSolo.ex5_g3 };
        protected void tabChanged(object sender, EventArgs args)
        {
            TabbedPage curr = (TabbedPage2)sender;
            switch (curr.CurrentPage.Title)
            {
                case "ex1":
                    this.currQ = 0;
                    this.end[1] = this.end[2] = this.end[3] = this.end[4] = 1;
                    this.end[0] = 0;
                    ShowingTimer(0);
                    if (qustions_array[0] == 1)
                    {
                        finish1.IsVisible = true;
                    }
                    var btn1 = firstQ.Children;
                    for (int i = 0; i < firstQ_array.Length; i++)
                    {
                        
                            ((Button)btn1.ElementAt(i)).IsEnabled = true;
                        
                    }
                    break;
                case "ex2":
                    this.currQ = 1;
                    this.end[0] = this.end[2] = this.end[3] = this.end[4] = 1;
                    this.end[1] = 0;
                    ShowingTimer(1);
                    if (qustions_array[1] == 1)
                    {
                        finish2.IsVisible = true;
                    }
                    var btn2 = secQ.Children;
                    for (int i = 0; i < secQ_array.Length; i++)
                    {
                        
                            ((Button)btn2.ElementAt(i)).IsEnabled = true;
                      
                    }
                    break;
                case "ex3":
                    this.currQ = 2;
                    this.end[1] = this.end[0] = this.end[3] = this.end[4] = 1;
                    this.end[2] = 0;
                    ShowingTimer(2);
                    if (qustions_array[2] == 1)
                    {
                        finish3.IsVisible = true;
                    }
                    var btn3 = thirdQ.Children;
                    for (int i = 0; i < thirdQ_array.Length; i++)
                    {
                        
                        
                            ((Button)btn3.ElementAt(i)).IsEnabled = true;
                        
                    }
                    break;
                case "ex4":
                    this.currQ = 3;
                    this.end[1] = this.end[2] = this.end[0] = this.end[4] = 1;
                    this.end[3] = 0;
                    ShowingTimer(3);
                    if (qustions_array[3] == 1)
                    {
                        finish4.IsVisible = true;
                    }
                    var btn4 = fourthQ.Children;
                    for (int i = 0; i < fourthQ_array.Length; i++)
                    {

                            ((Button)btn4.ElementAt(i)).IsEnabled = true;
                        
                    }
                    break;
                case "ex5":
                    this.currQ = 3;
                    this.end[1] = this.end[2] = this.end[3] = this.end[0] = 1;
                    this.end[4] = 0;
                    ShowingTimer(4);
                    if (qustions_array[4] == 1)
                    {
                        finish5.IsVisible = true;
                    }
                    var btn5 = fifthQ.Children;
                    for (int i = 0; i < fifthQ_array.Length; i++)
                    {
                       ((Button)btn5.ElementAt(i)).IsEnabled = true;
                        
                    }
                    break;
            }
        }
        protected async override void OnAppearing()
        {
            checkArray();

            switch (this.currQ)
                {
                    case 0:
                        var btn = firstQ.Children;
                        for (int i = 0; i < firstQ_array.Length; i++)
                        {
                            if (firstQ_array[i] == 1)
                            {
                                ((Button)btn.ElementAt(i)).IsEnabled = false;
                            }
                        }
                        break;
                    case 1:
                        var btn1 = secQ.Children;
                        for (int i = 0; i < secQ_array.Length; i++)
                        {
                            if (secQ_array[i] == 1)
                            {
                                ((Button)btn1.ElementAt(i)).IsEnabled = false;
                            }
                        }
                        break;
                    case 2:
                        var btn2 = thirdQ.Children;
                        for (int i = 0; i < thirdQ_array.Length; i++)
                        {
                            if (thirdQ_array[i] == 1)
                            {
                                ((Button)btn2.ElementAt(i)).IsEnabled = false;
                            }
                        }
                        break;
                    case 3:
                        var btn3 = fourthQ.Children;
                        for (int i = 0; i < fourthQ_array.Length; i++)
                        {
                            if (fourthQ_array[i] == 1)
                            {
                                ((Button)btn3.ElementAt(i)).IsEnabled = false;
                            }
                        }
                        break;
                    case 4:
                        var btn4 = fifthQ.Children;
                        for (int i = 0; i < fifthQ_array.Length; i++)
                        {
                            if (fifthQ_array[i] == 1)
                            {
                                ((Button)btn4.ElementAt(i)).IsEnabled = false;
                            }
                        }
                        break;
                    default:
                        break;
                }
            
        }
        public async void checkArray()
        {
            int counter = 0;

            switch (this.currQ)
            {
                case 0:
                    for (int i = 0; i < this.firstQ_array.Length; i++)
                    {
                        if (this.firstQ_array[i] == 1)
                        {
                            counter++;
                        }
                    }
                    if (counter == 3)
                    {
                        finish1.IsVisible = true;

                        if (qustions_array[0] == 0)
                        {
                            qustions_array[0] = 1;
                            List<User_game3> items = await MainUserManager.DefaultManager.CurrentUser_Game3Table
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex1_g3 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGame3Async(items[0]);

                        }
                    }
                    break;
                case 1:
                    for (int i = 0; i < this.secQ_array.Length; i++)
                    {
                        if (this.secQ_array[i] == 1)
                        {
                            counter++;
                        }
                    }
                    if (counter == 3)
                    {
                        finish2.IsVisible = true;

                        if (qustions_array[1] == 0)
                        {
                            qustions_array[1] = 1;

                            List<User_game3> items = await MainUserManager.DefaultManager.CurrentUser_Game3Table
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex2_g3 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGame3Async(items[0]);

                        }

                    }
                    break;
                case 2:
                    for (int i = 0; i < this.thirdQ_array.Length; i++)
                    {
                        if (this.thirdQ_array[i] == 1)
                        {
                            counter++;
                        }
                    }
                    if (counter == 5)
                    {
                        finish3.IsVisible = true;

                        if (qustions_array[2] == 0)
                        {
                            qustions_array[2] = 1;
                            List<User_game3> items = await MainUserManager.DefaultManager.CurrentUser_Game3Table
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex3_g3 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGame3Async(items[0]);

                        }

                    }
                    break;
                case 3:
                    for (int i = 0; i < this.fourthQ_array.Length; i++)
                    {
                        if (this.fourthQ_array[i] == 1)
                        {
                            counter += 1;
                        }
                    }
                    if (counter == 6)
                    {
                        finish4.IsVisible = true;
                        if (qustions_array[3] == 0)
                        {
                            qustions_array[3] = 1;
                            List<User_game3> items = await MainUserManager.DefaultManager.CurrentUser_Game3Table
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex4_g3 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGame3Async(items[0]);

                        }

                    }
                    break;
                case 4:
                    for (int i = 0; i < this.fifthQ_array.Length; i++)
                    {
                        if (this.fifthQ_array[i] == 1)
                        {
                            counter += 1;
                        }
                    }
                    if (counter == 5)
                    {
                        finish5.IsVisible = true;

                        if (qustions_array[4] == 0)
                        {
                            qustions_array[4] = 1;
                            List<User_game3> items = await MainUserManager.DefaultManager.CurrentUser_Game3Table
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex5_g3 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGame3Async(items[0]);

                        }

                    }
                    break;
                default:
                    break;
            }
            int count = 0;
            for (int i = 0; i < qustions_array.Length; i++)
            {
                count += qustions_array[i];
            }
            if (count == 5 && StartSolo.level == 3)
            {
                
                List<User> users = await MainUserManager.DefaultManager.CurrentUserTable
                    .Where(user => user.UserId == App.userId)
                    .ToListAsync();
                users[0].Stage = 4;
                users[0].Prizes = 3;
                await MainUserManager.DefaultManager.UpdateUserAsync(users[0]);
                await DisplayAlert("Awesome!", "You complete the third" +
                    " stage", "OK");

            }
        }


        private async void ShowingTimer(int i)
        {

            int _end = 0;
            Label[] arr_sec = { _secondView1, _secondView2, _secondView3, _secondView4, _secondView5 };
            Label[] arr_min = { _minuteView1, _minuteView2, _minuteView3, _minuteView4, _minuteView5 };
            for (int _minute = 0; _minute >= 0; _minute--)
            {
                for (int _second = 100; _second >= 0; _second--)
                {
                    if (this.end[i] == 1)
                    {
                        break;
                    }
                    if (_second == 0 && _minute == 0)
                    {
                        switch (this.currQ)
                        {
                            case 0:
                                var btn = firstQ.Children;
                                for (int j = 0; j < firstQ_array.Length; j++)
                                {

                                    ((Button)btn.ElementAt(j)).IsEnabled = false;

                                }
                                break;
                            case 1:
                                var btn1 = secQ.Children;
                                for (int j = 0; j < secQ_array.Length; j++)
                                {
                                    
                                        ((Button)btn1.ElementAt(j)).IsEnabled = false;
                                    
                                }
                                break;
                            case 2:
                                var btn2 = thirdQ.Children;
                                for (int j = 0; j < thirdQ_array.Length; j++)
                                {

                                    ((Button)btn2.ElementAt(j)).IsEnabled = false;

                                }
                                break;
                            case 3:
                                var btn3 = fourthQ.Children;
                                for (int j = 0; j < fourthQ_array.Length; j++)
                                {

                                    ((Button)btn3.ElementAt(j)).IsEnabled = false;

                                }
                                break;
                            case 4:
                                var btn4 = fifthQ.Children;
                                for (int j = 0; j < fifthQ_array.Length; j++)
                                {

                                    ((Button)btn4.ElementAt(j)).IsEnabled = false;

                                }
                                break;
                            default:
                                break;
                        }
                    }
                    if (_second < 10)
                    {
                        arr_sec[i].Text = Convert.ToString("0" + _second);
                    }
                    else
                    {
                        arr_sec[i].Text = Convert.ToString(_second);
                    }
                    arr_min[i].Text = Convert.ToString("0" + _minute);
                    await Task.Delay(1000);
                }


                _end++;
                if (_end == 1) { break; }
                   

            }
            
        }
        async void Btn_handler_firstQ(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Console.WriteLine(btn.Text);
            switch (btn.Text)
            {
                case "a":
                    await Navigation.PushAsync(new MediaPage("a", this.firstQ_array, 0, fromMediaPage, null));
                    this.currQ = 0;
                    break;
                case "c":
                    await Navigation.PushAsync(new MediaPage("c", this.firstQ_array, 1, fromMediaPage, null));
                    this.currQ = 0;
                    break;
                case "b":
                    await Navigation.PushAsync(new MediaPage("b", this.firstQ_array, 2, fromMediaPage, null));
                    this.currQ = 0;
                    break;
                default:
                    break;
            }


        }
        async void Btn_handler_secQ(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Console.WriteLine(btn.Text);
            switch (btn.Text)
            {
                case "d":
                    await Navigation.PushAsync(new MediaPage("d", this.secQ_array, 0, fromMediaPage, null));
                    this.currQ = 1;
                    break;
                case "e":
                    await Navigation.PushAsync(new MediaPage("e", this.secQ_array, 1, fromMediaPage, null));
                    this.currQ = 1;
                    break;
                case "a":
                    await Navigation.PushAsync(new MediaPage("a", this.secQ_array, 2, fromMediaPage, null));
                    this.currQ = 1;
                    break;
                default:
                    break;
            }

        }
        async void Btn_handler_thirdQ(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Console.WriteLine(btn.Text);
            switch (btn.Text)
            {
                case "e":
                    await Navigation.PushAsync(new MediaPage("e", this.thirdQ_array, 0, fromMediaPage, null));
                    this.currQ = 2;
                    break;
                case "f":
                    await Navigation.PushAsync(new MediaPage("f", this.thirdQ_array, 1, fromMediaPage, null));
                    this.currQ = 2;
                    break;
                case "l":
                    await Navigation.PushAsync(new MediaPage("l", this.thirdQ_array, 2, fromMediaPage, null));
                    this.currQ = 2;
                    break;
                case "m":
                    await Navigation.PushAsync(new MediaPage("m", this.thirdQ_array, 3, fromMediaPage, null));
                    this.currQ = 2;
                    break;
                case "n":
                    await Navigation.PushAsync(new MediaPage("n", this.thirdQ_array, 4, fromMediaPage, null));
                    this.currQ = 2;
                    break;
                default:
                    break;
            }



        }
        async void Btn_handler_fourthQ(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Console.WriteLine(btn.Text);
            switch (btn.Text)
            {
                case "p":
                    await Navigation.PushAsync(new MediaPage("p", this.fourthQ_array, 0, fromMediaPage, null));
                    this.currQ = 3;
                    break;
                case "y":
                    await Navigation.PushAsync(new MediaPage("y", this.fourthQ_array, 1, fromMediaPage, null));
                    this.currQ = 3;
                    break;
                case "x":
                    await Navigation.PushAsync(new MediaPage("x", this.fourthQ_array, 2, fromMediaPage, null));
                    this.currQ = 3;
                    break;
                case "k":
                    await Navigation.PushAsync(new MediaPage("k", this.fourthQ_array, 3, fromMediaPage, null));
                    this.currQ = 3;
                    break;
                case "e":
                    await Navigation.PushAsync(new MediaPage("e", this.fourthQ_array, 4, fromMediaPage, null));
                    this.currQ = 3;
                    break;
                case "s":
                    await Navigation.PushAsync(new MediaPage("s", this.fourthQ_array, 5, fromMediaPage, null));
                    this.currQ = 3;
                    break;
                default:
                    break;
            }

        }
        async void Btn_handler_fifthQ(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Console.WriteLine(btn.Text);
            switch (btn.Text)
            {
                case "c":
                    await Navigation.PushAsync(new MediaPage("c", this.fifthQ_array, 0, fromMediaPage, null));
                    this.currQ = 4;
                    break;
                case "a":
                    await Navigation.PushAsync(new MediaPage("a", this.fifthQ_array, 1, fromMediaPage, null));
                    this.currQ = 4;
                    break;
                case "m":
                    await Navigation.PushAsync(new MediaPage("m", this.fifthQ_array, 2, fromMediaPage, null));
                    this.currQ = 4;
                    break;
                case "e":
                    await Navigation.PushAsync(new MediaPage("e", this.fifthQ_array, 3, fromMediaPage, null));
                    this.currQ = 4;
                    break;
                case "l":
                    await Navigation.PushAsync(new MediaPage("l", this.fifthQ_array, 4, fromMediaPage, null));
                    this.currQ = 4;
                    break;
                default:
                    break;
            }

        }
    }
}