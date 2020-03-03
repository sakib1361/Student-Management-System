using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Messaging;
using Mobile.Core.Models.Core;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MobileApp.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class IITFirebaseMessagingService : FirebaseMessagingService
    {
        //private static int NotificationID = 0;
        const string TAG = "MyFirebaseMsgService";
        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);
            var body = message.GetNotification();
            SendNotification(body, message.Data);
        }

        public override void OnNewToken(string p0)
        {
            base.OnNewToken(p0);
            Log.Debug(TAG, p0);
        }

        void SendNotification(RemoteMessage.Notification message, IDictionary<string, string> data)
        {
            using (var intent = new Intent(this, typeof(MainActivity)))
            {
                intent.AddFlags(ActivityFlags.ClearTop);
                if (data != null)
                {
                    foreach (var key in data.Keys)
                    {
                        intent.PutExtra(key, data[key]);
                    }
                }
            }
            Device.BeginInvokeOnMainThread(() =>
            {
                AppService.ShowAlert(message.Title, message.Body);
            });
        }
    }
}