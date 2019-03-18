using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Lottie.Forms.Droid;
using Android.OS;
using System.Threading.Tasks;
using System.IO;
using Android.Content;
using Xamarin.Forms.Platform.Android;
using ImageCircle.Forms.Plugin.Droid;
using Plugin.CurrentActivity;

namespace eHealthWorkshopGroup4.Droid
{
    [Activity(Label = "Taekwondo It", Icon = "@drawable/Yin_yang", Theme = "@style/MainTheme",
        MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        //public static  MainActivity Instance { get; internal set; }

        protected override void OnCreate(Bundle bundle)
        {
            //Instance = this;
            //TabLayoutResource = Resource.Layout.Tabbar;
            //ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);

            ImageCircleRenderer.Init();
            CrossCurrentActivity.Current.Init(this, bundle);
            AnimationViewRenderer.Init();
            LoadApplication(new App());
        }

        // Field, property, and method for Picture Picker
        public static readonly int PickImageId = 1000;

        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);

                    // Set the Stream as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(stream);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }

    }

}