﻿using Android.App;
using Android.Content.PM;
using Android.OS;

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
            foreach (var key in Intent.Extras.KeySet())
            {
                if (key == "NavigationID")
                {
                    string idValue = Intent.Extras.GetString(key);
                    if (Preferences.ContainsKey("NavigationID"))
                        Preferences.Remove("NavigationID");

                    Preferences.Set("NavigationID", idValue);
                }
            }

            Senshost.App.IsNotificationReceived = true;
            Shell.Current.GoToAsync($"//NotificationListPage", true);

        }
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
}
