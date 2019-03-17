using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4;
using Android.Support.V4.App;


namespace UVapp
{
    [Service]
    public class NotificationService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }




        public override void OnCreate()
        {
            base.OnCreate();
        }




        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            string update = intent.GetStringExtra("update");
            string title = intent.GetStringExtra("title");

            NotificationCompat.BigTextStyle textStyle = new NotificationCompat.BigTextStyle();
            textStyle.BigText(update);

            Intent Nintent = new Intent(this, typeof(MainActivity));
            PendingIntent Pintent = PendingIntent.GetService(this, 0, Nintent, PendingIntentFlags.UpdateCurrent);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, MainActivity.CHANNEL_ID)
                .SetContentTitle(title)
                .SetContentIntent(Pintent)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                .SetStyle(textStyle)
                .SetSmallIcon(Resource.Drawable.notification_bg);


            Notification n = builder.Build();
            n.Flags =  NotificationFlags.AutoCancel | NotificationFlags.ForegroundService;
            this.StartForeground(1, n);

            return StartCommandResult.NotSticky;
        }





        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}