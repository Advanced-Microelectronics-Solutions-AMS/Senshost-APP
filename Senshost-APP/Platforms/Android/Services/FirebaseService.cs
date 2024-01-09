using Android.App;
using Android.Content;
using AndroidX.Core.App;
using CommunityToolkit.Mvvm.Messaging;
using Firebase.Messaging;
using AppConst = Senshost_APP.Constants;

namespace Senshost_APP.Platforms.Android.Services
{
    [Service(Exported = true)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseService : FirebaseMessagingService
    {
        public FirebaseService() { }

        public override void OnNewToken(string token)
        {
            base.OnNewToken(token);
            if (Preferences.ContainsKey(AppConst.AppConstants.FCMDeviceTokenKey))
                Preferences.Remove(AppConst.AppConstants.FCMDeviceTokenKey);

            if (Preferences.ContainsKey(AppConst.AppConstants.UserDeviceTokenIdKey))
                Preferences.Remove(AppConst.AppConstants.UserDeviceTokenIdKey);

            // TODO :: delete from DB
            // var id = Preferences.Get(AppConst.AppConstants.UserDeviceTokenIdKey, default(string));
            // await notificationService.DeleteDeviceToken(id);
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            SendNotification(message.Data);
            WeakReferenceMessenger.Default.Send("notification-received");
        }

        private void SendNotification(IDictionary<string, string> data)
        {

            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.AddFlags(ActivityFlags.SingleTop);

            foreach (var key in data.Keys)
            {
                string value = data[key];
                intent.PutExtra(key, value);
            }

            var title = data.FirstOrDefault(x => x.Key == "Title").Value;
            var body = data.FirstOrDefault(x => x.Key == "Body").Value;
            var Severity = data.FirstOrDefault(x => x.Key == "Severity").Value;
            var Id = data.FirstOrDefault(x => x.Key == "Id").Value;

            PendingIntent pendingIntent;

            if (OperatingSystem.IsOSPlatformVersionAtLeast("android", 23))
                pendingIntent = PendingIntent.GetActivity(this, MainActivity.NotificationID, intent, PendingIntentFlags.OneShot | PendingIntentFlags.Immutable);
            else
                pendingIntent = PendingIntent.GetActivity(this, MainActivity.NotificationID, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.Channel_ID)
                .SetContentTitle(title)
                .SetSubText(Severity)
                .SetBadgeIconType(NotificationCompat.BadgeIconSmall)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetContentText(body)
                .SetStyle(new NotificationCompat.BigTextStyle()
                        .BigText(body))
                .SetCategory(Severity)
                .SetColor(Resource.Color.colorNotificationIcon)
                .SetColorized(true)
                .SetChannelId(MainActivity.Channel_ID)
                .SetGroup(Severity).SetGroupSummary(true)
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true)
                .SetPriority((int)NotificationPriority.Max);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(Id, MainActivity.NotificationID, notificationBuilder.Build());
        }
    }
}
