using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Text;
using Java.Util;
using Newtonsoft.Json;

namespace UVapp
{
    [Activity(Label = "Skin Type", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SkintypeActivity : AppCompatActivity
    {
        static readonly int takePictureRequestCode = 1;
        string currentPhotoPath;

        ImageView resultImageView;
        TextView resultTextView;
        TextView resultNumView;
        TextView resultExplainView;


        Button okButton;
        Button takePhotoButton;

        User user;

        HttpClient httpClient = new HttpClient();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SkintypeActivityLayout);

            takePhotoButton = FindViewById<Button>(Resource.Id.takePhotoButton);
            takePhotoButton.Click += OnClickTakePhoto;
            okButton = FindViewById<Button>(Resource.Id.okButton);
            okButton.Click += OnClickOk;

            resultTextView = FindViewById<TextView>(Resource.Id.resultText);
            resultImageView = FindViewById<ImageView>(Resource.Id.resultImg);
            resultNumView = FindViewById<TextView>(Resource.Id.resultNumText);
            resultExplainView = FindViewById<TextView>(Resource.Id.resultExplainText);

            user = User.deserializeJson(Intent.GetStringExtra("userJson"));
        }

        private void OnClickOk(object sender, EventArgs e)
        {
            UserManager.createUser(user);

            Intent mainIntent = new Intent(this, typeof(MainActivity));
            mainIntent.PutExtra("userJson", user.ToString());
            MainActivity.loggedIn = true;
            StartActivity(mainIntent);
        }

        private void OnClickTakePhoto(object sender, EventArgs eventArgs)
        {
            Intent takePictureIntent = new Intent(MediaStore.ActionImageCapture);

            File photoFile = null;
            try
            {
                photoFile = createImageFile();
            }
            catch (IOException)
            {
                resultTextView.Text = "Error accessing photo";
            }

            if (photoFile != null)
            {
                Android.Net.Uri photoUri = FileProvider.GetUriForFile(this, "UVapp.UVapp.fileprovider", photoFile);

                takePictureIntent.PutExtra(MediaStore.ExtraOutput, photoUri);
                StartActivityForResult(takePictureIntent, takePictureRequestCode);

            }
        }

        private async void SkintypeClassificationProcess(Bitmap image)
        {
            RunOnUiThread(() => { resultTextView.Text = $"Identifying skin type..."; });
                
            HttpResponseMessage skintypeResponse = await SkintypeClassification.serverClassifyImage(image, httpClient);
            if (skintypeResponse.IsSuccessStatusCode)
            {
                var responseContent = await skintypeResponse.Content.ReadAsStringAsync();

                SkintypeClassification.ClassifiedColor deserialized = JsonConvert.DeserializeObject<SkintypeClassification.ClassifiedColor>(responseContent);
                SkinType skinType = (SkinType)deserialized.skinType;
                user.skinType = deserialized.skinType;

                RunOnUiThread(() => {
                    resultTextView.Text = $"Your skin type is";
                    resultNumView.Text = $"{deserialized.skinType}";
                    resultExplainView.Text = "1 - Palest\n6 - Darkest";
                    takePhotoButton.Text = "Retake";
                    okButton.Visibility = ViewStates.Visible;
                    okButton.Clickable = true;
                });
            }
            else
            {
                RunOnUiThread(() =>
                {
                    resultTextView.Text = $"Error: {skintypeResponse.ReasonPhrase}";
                });
            }   
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == takePictureRequestCode && resultCode == Result.Ok)
            {

                try
                {
                    Bitmap fullsize_bitmap = MediaStore.Images.Media.GetBitmap(this.ContentResolver, Android.Net.Uri.FromFile(new File(currentPhotoPath)));

                    int newHeight;
                    int newWidth;

                    bool verticalOrientation = fullsize_bitmap.Height > fullsize_bitmap.Width;
                    if (verticalOrientation)
                    {
                        double aspectRatio = ((double)fullsize_bitmap.Height) / fullsize_bitmap.Width;
                        newHeight = (int)(600.0 * aspectRatio);
                        newWidth = 600;
                    }
                    else
                    {
                        double aspectRatio = ((double)fullsize_bitmap.Width) / fullsize_bitmap.Height;
                        newHeight = 600;
                        newWidth = (int)(600.0 * aspectRatio);
                    }


                    Bitmap bitmap = Bitmap.CreateScaledBitmap(fullsize_bitmap, newWidth, newHeight, false);
                    fullsize_bitmap.Recycle();  // Yes, we are freeing memory, like the good ol' days of C
                   

                    resultImageView.SetImageBitmap(bitmap);
                    SkintypeClassificationProcess(bitmap);
                    
                }
                catch (IOException e)
                {
                    System.Console.WriteLine(e.StackTrace);
                    resultTextView.Text = "Error getting photo";
                }

            }
            else
            {
                base.OnActivityResult(requestCode, resultCode, data);
            }
        }


        private File createImageFile()
        {
            // Create an image file name
            String timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss").Format(new Date());
            String imageFileName = "JPEG_" + timeStamp + "_";
            File storageDir = GetExternalFilesDir(Android.OS.Environment.DirectoryPictures);
            File image = File.CreateTempFile(
                imageFileName,  /* prefix */
                ".jpg",         /* suffix */
                storageDir      /* directory */
            );

            // Save a file: path for use with ACTION_VIEW intents
            currentPhotoPath = image.AbsolutePath;
            return image;
        }
    }
}