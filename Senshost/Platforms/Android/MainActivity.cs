using System.Globalization;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Senshost.Common.Interfaces;
using Senshost.Models.Constants;
//using Plugin.Firebase.CloudMessaging;
using Senshost.Models.Notification;
using Senshost.Services;
using Senshost.Views;

namespace Senshost;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    internal static readonly string Channel_ID = "TestChannel";
    internal static readonly int NotificationID = 101;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        if (Intent.Extras != null)
        {
            var keySet = Intent.Extras.KeySet();
            var notification = Intent.Extras.ToString();

            var extra = Intent.GetStringExtra("NotificationContent");

            //var notificationID = "";

            var notiFic = new Models.Notification.Notification();

            foreach (var key in Intent.Extras.KeySet())
            {
                string idValue = Intent.Extras.GetString(key);
                Console.WriteLine(idValue);
                if(key.ToUpper() == "Severity".ToUpper())
                {
                    SeverityLevel tmpVal;                    
                    if(Enum.TryParse<SeverityLevel>(idValue, out tmpVal))
                    {
                        notiFic.Severity = tmpVal;
                    }
                }
                else if (key.ToUpper() == "Date".ToUpper())
                {
                    DateTime tmpVal;
                    string[] formats = {"MM/dd/yyyy HH:mm:ss" };
                    if (DateTime.TryParseExact(idValue, formats,
                        null, DateTimeStyles.None, out tmpVal))
                    {
                        notiFic.CreationDate = tmpVal;
                    }
                }
                else if (key.ToUpper() == "AccountId".ToUpper())
                {
                    notiFic.AccountId = new Guid(idValue);
                }
                else if (key.ToUpper() == "ID".ToUpper())
                {
                    notiFic.Id = new Guid(idValue);
                }
                else if (key.ToUpper() == "Title".ToUpper())
                {
                    notiFic.Title = idValue;
                }
                else if (key.ToUpper() == "Body".ToUpper())
                {
                    notiFic.Body = idValue;
                }
                

                if (key == "NavigationID")
                {
                    //string idValue = Intent.Extras.GetString(key);
                    if (Preferences.ContainsKey("NavigationID"))
                        Preferences.Remove("NavigationID");

                    Preferences.Set("NavigationID", idValue);
                }
            }

            Senshost.App.IsNotificationReceived = true;
            Shell.Current.GoToAsync($"//NotificationListPage?isToReloadPage=true", false);

            var cities = new Dictionary<string, object>();
            //Shell.Current.GoToAsync($"//tabPages/NotificationDetailPage", true);

            Shell.Current.Navigation.PushAsync(new NotificationDetailPage(new ViewModels.NotificationDetailPageViewModel() { Notification = notiFic}));
        }

        //HandleIntent(Intent);
        //CreateNotificationChannelIfNeeded();

        CreateNotificationChannel();
    }


    private void CreateNotificationChannel()
    {
        if (OperatingSystem.IsOSPlatformVersionAtLeast("android", 26))
        {
            var channel = new NotificationChannel(Channel_ID, "Test Notfication Channel", NotificationImportance.High);

            var notificaitonManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificaitonManager.CreateNotificationChannel(channel);
        }
    }

    //protected override void OnCreate(Bundle savedInstanceState)
    //{
    //    base.OnCreate(savedInstanceState);
    //    HandleIntent(Intent);
    //    CreateNotificationChannelIfNeeded();
    //}

    //protected override void OnNewIntent(Intent intent)
    //{
    //    base.OnNewIntent(intent);
    //    HandleIntent(intent);
    //}

    //private static void HandleIntent(Intent intent)
    //{
    //    FirebaseCloudMessagingImplementation.OnNewIntent(intent);
    //}

    //private void CreateNotificationChannelIfNeeded()
    //{
    //    if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
    //    {
    //        CreateNotificationChannel();
    //    }
    //}

    //private void CreateNotificationChannel()
    //{
    //    var channelId = $"{PackageName}.general";
    //    var notificationManager = (NotificationManager)GetSystemService(NotificationService);
    //    var channel = new NotificationChannel(channelId, "General", NotificationImportance.Default);
    //    notificationManager.CreateNotificationChannel(channel);
    //    FirebaseCloudMessagingImplementation.ChannelId = channelId;
    //    //FirebaseCloudMessagingImplementation.SmallIconRef = Resource.Drawable.ic_push_small;
    //}
}
