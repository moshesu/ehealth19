using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SignBuzz.Solo.Game2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPage1Detail : ContentPage
    {
        public MasterDetailPage1Detail()
        {
            InitializeComponent();
        }
        int[] firstQ_array = { 0, 0, 0 };
        int[] secQ_array = { 0, 0, 0 };
        int[] thirdQ_array = { 0, 0, 0, 0, 0};
        int[] fourthQ_array = { 0, 0, 0, 0, 0, 0};
        int[] fifthQ_array = { 0, 0, 0, 0, 0};
        int currQ = 0;
        int[] fromMediaPage = { 0 };
        static int[] qustions_array = { StartSolo.ex1_g2, StartSolo.ex2_g2, StartSolo.ex3_g2, StartSolo.ex4_g2, StartSolo.ex5_g2 };

        protected override void OnAppearing()
        {
            checkArray(this.currQ);
            if (fromMediaPage[0] == 1)
            {
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
        }

            
        
        async void first_ex(object sender, EventArgs e)
        {
                Render(0);
        }
        
        public void Render(int i)
        {
            switch (i)
            {

                // Dog
                case 0:
                    if (qustions_array[0] == 1)
                    {
                        finish.IsVisible = true;
                    }
                    animalImg.Source = "https://i.barkpost.com/wp-content/uploads/2015/10/reddit-dog-jokes-20.jpg?q=70&fit=crop&crop=entropy&w=808&h=808";
                    firstQ.IsVisible = true;
                    ins.IsVisible = false;
                    break;
                // Cat
                case 1:
                    if (qustions_array[1] == 1)
                    {
                        finish.IsVisible = true;
                    }
                    animalImg.Source = "https://pbs.twimg.com/profile_images/378800000532546226/dbe5f0727b69487016ffd67a6689e75a_400x400.jpeg";
                    secQ.IsVisible = true;
                    ins.IsVisible = false;
                    break;
                // Horse
                case 2:
                    if (qustions_array[2] == 1)
                    {
                        finish.IsVisible = true;
                    }
                    animalImg.Source = "https://www.ufaw.org.uk/images/paint-horse_cropped_1000_667.jpg";
                    thirdQ.IsVisible = true ;
                    ins.IsVisible = false;
                    break;
                case 3:
                    // Monkey
                    if (qustions_array[3] == 1)
                    {
                        finish.IsVisible = true;
                    }
                    animalImg.Source = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSIbccZmcg-2Vq31-1MQ8pCP4u0fuvo9_WqJmZt7g5Sr64e6K-C";
                    fourthQ.IsVisible = true;
                    ins.IsVisible = false;
                    break;
                // Camel
                case 4:
                    if (qustions_array[4] == 1)
                    {
                        finish.IsVisible = true;
                    }
                    animalImg.Source = "https://cdn.britannica.com/s:300x500/94/152294-131-DC3E25DB.jpg";
                    fifthQ.IsVisible = true;
                    ins.IsVisible = false;
                    break;
                default:
                    Console.WriteLine("You did not select a valid option.");
                    break;
            }
            
        }
        async void Btn_handler_firstQ(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Console.WriteLine(btn.Text);
            switch (btn.Text)
            {
                case "d":
                    await Navigation.PushAsync(new MediaPage("d" , this.firstQ_array, 0, fromMediaPage , null));
                    this.currQ = 0;
                    break;
                case "o":
                    await Navigation.PushAsync(new MediaPage("o", this.firstQ_array, 1, fromMediaPage , null));
                    this.currQ = 0;
                    break;
                case "g":
                    await Navigation.PushAsync(new MediaPage("d", this.firstQ_array, 2, fromMediaPage  , null));
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
                case "c":
                    await Navigation.PushAsync(new MediaPage("c", this.secQ_array, 0, fromMediaPage,null));
                    this.currQ = 1;
                    break;
                case "a":
                    await Navigation.PushAsync(new MediaPage("a", this.secQ_array, 1, fromMediaPage, null));
                    this.currQ = 1;
                    break;
                case "t":
                    await Navigation.PushAsync(new MediaPage("t", this.secQ_array, 2, fromMediaPage, null));
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
                case "h":
                    await Navigation.PushAsync(new MediaPage("h", this.thirdQ_array, 0, fromMediaPage, null));
                    this.currQ = 2;
                    break;
                case "o":
                    await Navigation.PushAsync(new MediaPage("o", this.thirdQ_array, 1, fromMediaPage, null));
                    this.currQ = 2;
                    break;
                case "r":
                    await Navigation.PushAsync(new MediaPage("r", this.thirdQ_array, 2, fromMediaPage, null));
                    this.currQ = 2;
                    break;
                case "s":
                    await Navigation.PushAsync(new MediaPage("s", this.thirdQ_array, 3, fromMediaPage, null));
                    this.currQ = 2;
                    break;
                case "e":
                    await Navigation.PushAsync(new MediaPage("e", this.thirdQ_array, 4, fromMediaPage, null));
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
                case "m":
                    await Navigation.PushAsync(new MediaPage("m", this.fourthQ_array, 0  , fromMediaPage, null));
                    this.currQ = 3;
                    break;
                case "o":
                    await Navigation.PushAsync(new MediaPage("o", this.fourthQ_array, 1, fromMediaPage, null));
                    this.currQ = 3;
                    break;
                case "n":
                    await Navigation.PushAsync(new MediaPage("n", this.fourthQ_array, 2, fromMediaPage, null));
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
                case "y":
                    await Navigation.PushAsync(new MediaPage("y", this.fourthQ_array, 5, fromMediaPage, null));
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
        public async void checkArray(int question)
        {
            int counter = 0;

            switch (question)
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
                        finish.IsVisible = true;

                        if (qustions_array[0] == 0)
                        {
                            qustions_array[0] = 1;
                            ins.IsVisible = false;
                            List<User_game2> items = await MainUserManager.DefaultManager.CurrentUser_Game2Table
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex1_g2 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGame2Async(items[0]);

                        }
                    }
                    break;
                case 1:
                    for (int i = 0; i < this.secQ_array.Length; i++)
                    {
                        if (this.secQ_array[i] == 1)
                        {
                            counter++ ;
                        }
                    }
                    if (counter == 3)
                    {
                        finish.IsVisible = true;

                        if (qustions_array[1] == 0)
                        {
                            qustions_array[1] = 1;
                            ins.IsVisible = false;
                            List<User_game2> items = await MainUserManager.DefaultManager.CurrentUser_Game2Table
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex2_g2 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGame2Async(items[0]);

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
                        finish.IsVisible = true;

                        if (qustions_array[2] == 0)
                        {
                            qustions_array[2] = 1;
                            ins.IsVisible = false;
                            List<User_game2> items = await MainUserManager.DefaultManager.CurrentUser_Game2Table
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex3_g2 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGame2Async(items[0]);

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
                        finish.IsVisible = true;
                        if (qustions_array[3] == 0)
                        {
                            qustions_array[3] = 1;
                            ins.IsVisible = false;
                            List<User_game2> items = await MainUserManager.DefaultManager.CurrentUser_Game2Table
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex4_g2 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGame2Async(items[0]);

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
                        finish.IsVisible = true;

                        if (qustions_array[4] == 0)
                        {
                            qustions_array[4] = 1;
                            ins.IsVisible = false;
                            List<User_game2> items = await MainUserManager.DefaultManager.CurrentUser_Game2Table
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex5_g2 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGame2Async(items[0]);

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
            if (count == 5 && StartSolo.level == 2)
            {
                
                List<User> users = await MainUserManager.DefaultManager.CurrentUserTable
                    .Where(user => user.UserId == App.userId)
                    .ToListAsync();
                users[0].Stage = 3;
                users[0].Prizes = 2;
                await MainUserManager.DefaultManager.UpdateUserAsync(users[0]);
                await DisplayAlert("Awesome!", "You complete the second stage", "OK");

            }
        }
    }

}