using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.Content;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.Firebase.CloudMessaging;
using Senshost_APP.Models.Constants;
using Senshost_APP.Models.Notification;
using Resource = Microsoft.Maui.Resource;

namespace Senshost_APP
{
    [Activity(Theme = "@style/SplashTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
        ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        internal static readonly string Channel_ID = "EventAlerts";
        internal static readonly int NotificationID = 101;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HandleIntent(Intent);

            if (OperatingSystem.IsOSPlatformVersionAtLeast("android", 23))
            {
                var color = ContextCompat.GetColor(this, Resource.Color.colorPrimary);
                Window.SetNavigationBarColor(new Android.Graphics.Color(color));
            }
            else
            {
                Window.SetNavigationBarColor(Resources.GetColor(Resource.Color.colorPrimaryDark));
            }

            CreateNotificationChannelIfNeeded();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            HandleIntent(intent);
        }

        private void HandleIntent(Intent intent)
        {
            if (intent.Extras != null)
            {
                var notiFic = new Models.Notification.Notification();
                foreach (var key in intent.Extras.KeySet())
                {
                    if (key.ToUpper() == "Severity".ToUpper())
                    {
                        SeverityLevel tmpVal;
                        if (Enum.TryParse<SeverityLevel>(intent.Extras.GetString(key), out tmpVal))
                            notiFic.Severity = tmpVal;
                    }
                    else if (key.ToUpper() == "Date".ToUpper())
                    {
                        DateTime tmpVal;
                        if (DateTime.TryParse(intent.Extras.GetString(key), out tmpVal))
                            notiFic.CreationDate = tmpVal;
                    }
                    else if (key.ToUpper() == "AccountId".ToUpper())
                        notiFic.AccountId = new Guid(intent.Extras.GetString(key));
                    else if (key.ToUpper() == "ID".ToUpper())
                        notiFic.Id = new Guid(intent.Extras.GetString(key));
                    else if (key.ToUpper() == "Title".ToUpper())
                        notiFic.Title = intent.Extras.GetString(key);
                    else if (key.ToUpper() == "Body".ToUpper())
                        notiFic.Body = intent.Extras.GetString(key);
                }

                WeakReferenceMessenger.Default.Send(new NotificationTapped() { Notification = notiFic });

                var notificaitonManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
                notificaitonManager.CancelAll();
            }
        }

        private void CreateNotificationChannelIfNeeded()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                CreateNotificationChannel();
            }
        }

        private void CreateNotificationChannel()
        {
            if (OperatingSystem.IsOSPlatformVersionAtLeast("android", 26))
            {
                var channel = new NotificationChannel(Channel_ID, "Senshost Event Channel", NotificationImportance.High);

                var notificaitonManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
                notificaitonManager.CreateNotificationChannel(channel);
                FirebaseCloudMessagingImplementation.ChannelId = Channel_ID;
            }
        }

    }
}