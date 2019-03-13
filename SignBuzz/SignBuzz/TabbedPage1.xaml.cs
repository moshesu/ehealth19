using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Microsoft.WindowsAzure.Storage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace SignBuzz
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPage1 : TabbedPage
    {
        
        MediaFile _mediaFile = null;

        public TabbedPage1 ()
        {
            InitializeComponent();
            

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            
            
            List<User> items = await MainUserManager.DefaultManager.CurrentUserTable
                    .Where(user => user.UserId == App.userId)
                    .ToListAsync();
            userName.Text = items[0].Name;
            userName.FontSize = 18;
            userPhoto.Source = this._mediaFile == null ? items[0].Image : ImageSource.FromStream(() => this._mediaFile.GetStream());
            Mode.Text = "You are at the " + items[0].Stage + " Stage!";
            if (items[0].Prizes == 1)
            {
                noPrizes.IsVisible = false;
                star2.IsVisible = false;
                star3.IsVisible = false;
            }
            else if (items[0].Prizes == 2)
            {
                noPrizes.IsVisible = false;
                star3.IsVisible = false;
            }
            else if (items[0].Prizes == 0)
            {
                star1.IsVisible = false;
                star2.IsVisible = false;
                star3.IsVisible = false;
            }
            else
            {
                noPrizes.IsVisible = false;
            }
            try
            {
                double lati;
                double longi;
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    lati = location.Latitude;
                    longi = location.Longitude;
                    Console.WriteLine(Math.Abs(lati - 32.1123251) < 0.1);
                    Console.WriteLine(Math.Abs(longi - 34.804497) < 0.1);
                    List<User_compete> users = await MainUserManager.DefaultManager.CurrentUser_CompeteTable
                   .Where(user => true)
                   .ToListAsync();
                    var table = new TableView();
                    table.Intent = TableIntent.Settings;
                    var layout = new StackLayout() { Orientation = StackOrientation.Horizontal };
                    
                    table.Root = new TableRoot(){
                                 new TableSection("Leader Board"){
                                     new ViewCell() {View = layout}
                                 }
                    };
                    for (int i = 0; i < users.Count; i++)
                    {
                        if (Math.Abs(lati - 32.1123251) < 0.1 && Math.Abs(longi - 34.804497) < 0.1)
                        {
                            var layout1 = new StackLayout() { Orientation = StackOrientation.Horizontal };
                            table.Root.Add(new TableSection("") { new ViewCell() { View = layout1 } });

                            layout1.Children.Add(new Label()
                            {
                                Text = users[i].Name,
                                TextColor = Color.FromHex("#f35e20"),
                                VerticalOptions = LayoutOptions.Center
                            });
                            layout1.Children.Add(new Label()
                            {
                                Text = users[i].Res.ToString(),
                                TextColor = Color.FromHex("#503026"),
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.EndAndExpand
                            });
                            layout1.Children.Add(new Label
                                ()
                            { Text = (i + 1).ToString() });
                        }
                    }
                        
                    lay.Children.Add(table);
                }
            }
            catch
            {

            }
        }
        private async void btnSelectPic_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "This is not support on your device.", "OK");
                return;
            }
            else
            {
                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                this._mediaFile = await CrossMedia.Current.PickPhotoAsync();
                if (this._mediaFile == null) return;
                userPhoto.Source = ImageSource.FromStream(() => this._mediaFile.GetStream());


            }

        }
        private async void UploadImage(object sender, EventArgs e)
        {
            if (this._mediaFile != null)
            {
                userPhoto.IsVisible = false;
                uploadIndicator.IsVisible = true;
                uploadIndicator.IsRunning = true;
                var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=group6;AccountKey=emLbRRuzA5x29nstt/9AU6hIcXYihpQnUsAIcIZvfIFukJxHE9Flm340+rItRN7XfEPsfwZ8pEXoxkIDZXxSHw==;EndpointSuffix=core.windows.net");
                var client = account.CreateCloudBlobClient();
                var container = client.GetContainerReference("images");
                await container.CreateIfNotExistsAsync();
                var name = Guid.NewGuid().ToString();
                var blockBlob = container.GetBlockBlobReference($"{name}.png");
                await blockBlob.UploadFromStreamAsync(this._mediaFile.GetStream());
                string URL = blockBlob.Uri.OriginalString;
                List<User> items = await MainUserManager.DefaultManager.CurrentUserTable
                    .Where(user => user.UserId == App.userId)
                    .ToListAsync();
                items[0].Image = URL;
                await MainUserManager.DefaultManager.UpdateUserAsync(items[0]);
                userPhoto.IsVisible = true;
                uploadIndicator.IsVisible = false;
                uploadIndicator.IsRunning = false;
            }

        }

    }
}
