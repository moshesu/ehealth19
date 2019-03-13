using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.WindowsAzure.Storage;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SignBuzz
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaPage : ContentPage
    {

        string letterToCheck = "";// what letter the user should guess
        int[] arrayToChange; // change the array if the user was correct
        int index; // in wat index of the array above
        int[] bitArray;// for separating different onappear functions
        int[] gameone;
        private MediaFile _mediaFile;
        private string URL { get; set; }
        public MediaPage(String Char , int[] array , int index , int[] bit, int[] gameone)
        {
            InitializeComponent();
            this.letterToCheck = Char;
            this.arrayToChange = array;
            this.index = index;
            headline.Text = Char.ToUpper();
            this.bitArray = bit;
            this.gameone = gameone;
            
        }
        protected async override void OnAppearing()
        {
            if (bitArray != null)
            {
                bitArray[0] = 1;
            }
        }
        //Picture choose from device    
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
                _mediaFile = await CrossMedia.Current.PickPhotoAsync();
                if (_mediaFile == null) return;
                imageView.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
            }
            
        }
        public async void activeIfTrue()
        {
            if (arrayToChange != null)
            {
                this.arrayToChange[index] = 1;
                this.bitArray[0] = 1;
            }
            if (gameone != null)
            {

                switch (this.letterToCheck)
                {
                    case "A":
                        if (this.gameone[0] == 0)
                        {
                            this.gameone[0] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex1_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "B":
                        if (this.gameone[1] == 0)
                        {
                            this.gameone[1] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex2_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "C":
                        if (this.gameone[2] == 0)
                        {
                            this.gameone[2] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex3_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "D":
                        if (this.gameone[3] == 0)
                        {
                            this.gameone[3] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex4_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "E":
                        if (this.gameone[4] == 0)
                        {
                            this.gameone[4] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex5_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "F":
                        if (this.gameone[5] == 0)
                        {
                            this.gameone[5] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex6_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "G":
                        if (this.gameone[6] == 0)
                        {
                            this.gameone[6] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex7_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "H":
                        if (this.gameone[7] == 0)
                        {
                            this.gameone[7] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex8_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "I":
                        if (this.gameone[8] == 0)
                        {
                            this.gameone[8] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex9_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "J":
                        if (this.gameone[9] == 0)
                        {
                            this.gameone[9] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex10_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "K":
                        if (this.gameone[10] == 0)
                        {
                            this.gameone[10] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex11_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "L":
                        if (this.gameone[11] == 0)
                        {
                            this.gameone[11] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex12_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "M":
                        if (this.gameone[12] == 0)
                        {
                            this.gameone[12] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex13_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "N":
                        if (this.gameone[13] == 0)
                        {
                            this.gameone[13] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex14_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "O":
                        if (this.gameone[14] == 0)
                        {
                            this.gameone[14] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex15_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "P":
                        if (this.gameone[15] == 0)
                        {
                            this.gameone[15] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex16_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "Q":
                        if (this.gameone[16] == 0)
                        {
                            this.gameone[16] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex17_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "R":
                        if (this.gameone[17] == 0)
                        {
                            this.gameone[17] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex18_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "S":
                        if (this.gameone[18] == 0)
                        {
                            this.gameone[18] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex19_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "T":
                        if (this.gameone[19] == 0)
                        {
                            this.gameone[19] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex20_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "U":
                        if (this.gameone[20] == 0)
                        {
                            this.gameone[20] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex21_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "V":
                        if (this.gameone[21] == 0)
                        {
                            this.gameone[21] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex22_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "W":
                        if (this.gameone[22] == 0)
                        {
                            this.gameone[22] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex23_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "X":
                        if (this.gameone[23] == 0)
                        {
                            this.gameone[23] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex24_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    case "Y":
                        if (this.gameone[24] == 0)
                        {
                            this.gameone[24] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex25_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                    default:
                        if (this.gameone[25] == 0)
                        {
                            this.gameone[25] = 1;
                            List<User_game> items = await MainUserManager.DefaultManager.CurrentUser_GameTable
                            .Where(user => user.UserId == App.userId)
                            .ToListAsync();
                            items[0].Ex26_g1 = 1;
                            await MainUserManager.DefaultManager.UpdateUserGameAsync(items[0]);
                        }
                        break;
                }
            }
        }
        //Upload picture button    
        private async void btnUpload_Clicked(object sender, EventArgs e)
        {
            if (_mediaFile == null)
            {
                await DisplayAlert("Error", "There was an error when trying to get your image.", "OK");
                return;
            }
            else
            {
                UploadImage(_mediaFile.GetStream());
            }
        }

        //Take picture from camera    
        private async void btnTakePic_Clicked(object sender, EventArgs e)
        {

            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":(No Camera available.)", "OK");
                return;
            }
            else
            {
                _mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "myImage.jpg"
                });

                if (_mediaFile == null) return;
                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                imageView.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
                }
        }

        //Upload to blob function    
        private async void UploadImage(Stream stream)
        {
            // Upload image to blob
            Busy();
            var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=group6;AccountKey=emLbRRuzA5x29nstt/9AU6hIcXYihpQnUsAIcIZvfIFukJxHE9Flm340+rItRN7XfEPsfwZ8pEXoxkIDZXxSHw==;EndpointSuffix=core.windows.net");
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference("images");
            await container.CreateIfNotExistsAsync();
            var name = Guid.NewGuid().ToString();
            var blockBlob = container.GetBlockBlobReference($"{name}.png");
            await blockBlob.UploadFromStreamAsync(stream);
            URL = blockBlob.Uri.OriginalString;
            await this.MakePredictionRequest(URL);
            NotBusy();
        }


        async Task MakePredictionRequest(string imageFilePath)
        {
            // Request headers - replace this example key with your valid subscription key.
            App.client.DefaultRequestHeaders.Add("Prediction-Key", "fa7f04740f2a4ca686c7a59d48ed4177");
            App.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); ;

            // Prediction URL - replace this example URL with your valid prediction URL.
            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/3fd83e1a-4fda-4163-8226-2e5046aca950/url?iterationId=0cd53642-8b43-4cf9-bca3-0a03b79ead40";

            HttpResponseMessage response;

            // Request body. Try this sample with a locally stored image.
            var values = new Dictionary<string, string>
            { { "Url", imageFilePath }};
            var content = new FormUrlEncodedContent(values);
            response = await App.client.PostAsync(url, content);
            String res = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(res);
            String letter_result = (string) json.SelectToken("predictions[0].tagName");
            if (this.letterToCheck.ToLower() == letter_result.ToLower())
            {
                correct.IsVisible = true;
                activeIfTrue();
                await DisplayAlert("Awesome!", "You Guess the letter correctly!" , "OK");
                await Navigation.PopAsync();
            }
            else
            {
                error_grid.IsVisible = true;
                await DisplayAlert("Almost!", "The letter you did is: " + letter_result.ToUpper(), "OK");
                output_letter.Text = "The letter you did is: " + letter_result.ToUpper();
            }
            
        }
        public void Busy()
        {
            uploadIndicator.IsVisible = true;
            uploadIndicator.IsRunning = true;
            btnSelectPic.IsEnabled = false;
            btnTakePic.IsEnabled = false;
            btnUpload.IsEnabled = false;
        }

        public void NotBusy()
        {
            uploadIndicator.IsVisible = false;
            uploadIndicator.IsRunning = false;
            btnSelectPic.IsEnabled = true;
            btnTakePic.IsEnabled = true;
            btnUpload.IsEnabled = true;
        }
    }
}
