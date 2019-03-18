using CloudStorage;
using Plugin.Media;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eHealthWorkshopGroup4.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        CloudStorageHandler storage = null;
        string uname;

        public Profile()
        {
            InitializeComponent();
            storage = new CloudStorageHandler();
            uname = App.MyUserName;
        }

        protected override async void OnAppearing()
        {
            
            Stream profileStream = await storage.getUserImage(uname, true);
            Stream subjectStream = await storage.getUserImage(uname, false);

            ProfileImage.Source = ImageSource.FromStream(() => profileStream);
            SubjectImage.Source = ImageSource.FromStream(() => subjectStream);
            var x = await storage.getUserProfileData(uname);
            FullName.Text = x.fullname;
            NickName.Text = uname;
            UserRank.Text = x.rank.ToString();
            GroupName.Text = x.group;
            CoachName.Text = await storage.getGroupCoach(x.group);
            XPindex.Text = x.xp.ToString();
            
        }


        async void PickPicture(object sender, EventArgs e)
        {
            var response = await DisplayAlert("App note", "Where do you want to take the picture from?", "gallary", "cammera");
            if (response)
            {
                ChangeProfilePictureButton.IsEnabled = false;
                Stream stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();
                
                if (stream != null)
                {
                    Image image = new Image
                    {
                        //Source = ImageSource.FromStream(() => stream),
                        BackgroundColor = Color.Gray
                    };

                    TapGestureRecognizer recognizer = new TapGestureRecognizer();
                    recognizer.Tapped += (sender2, args) =>
                    {
                        ChangeProfilePictureButton.IsEnabled = true;
                    };
                    image.GestureRecognizers.Add(recognizer);
                    
                    if (sender == ChangeProfilePictureButton)
                    {
                        updatePicture(stream, true);
                    }
                    else
                    {
                        updatePicture(stream, false);
                    }

                }
                else
                {
                    ChangeProfilePictureButton.IsEnabled = true;
                }
            }
            else
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }
                var photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "test.jpg"
                });
                if (photo != null)
                {
                    Stream photoStream = photo.GetStream();
                    if (sender == ChangeProfilePictureButton)
                    {
                        updatePicture(photoStream, true);
                    }
                    else
                    {
                        updatePicture(photoStream, false);
                    }
                }

            }

        }

        private async void updatePicture(Stream photoStream, bool isProfile)
        {
            await storage.updateUserImage(uname, photoStream, isProfile);
            Stream getStorageStream = await storage.getUserImage(uname, isProfile);
            if (isProfile)
            {
                ProfileImage.Source = ImageSource.FromStream(() => getStorageStream);
            }
            else
            {
                SubjectImage.Source = ImageSource.FromStream(() => getStorageStream);
            }
            
        }



    }
}