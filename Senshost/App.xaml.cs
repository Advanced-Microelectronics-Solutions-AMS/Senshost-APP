using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Platform;
using Plugin.Firebase.CloudMessaging;
using Senshost.Common.Interfaces;
using Senshost.Handlers;
using Senshost.Models.Account;
using Senshost.Models.Constants;
using Senshost.Models.Notification;
using Senshost.ViewModels;
using Senshost.Views;
using AppConst = Senshost.Constants;

namespace Senshost;

public partial class App : Application
{
    public static LogedInUserDetails UserDetails;
    public static string ApiToken;
    public static bool IsNotificationReceived;
    private readonly IServiceProvider serviceProvider;
    public static int StatusBarHeight;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
#if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
#pragma warning disable CA1416 // Validate platform compatibility
                handler.PlatformView.TextCursorDrawable.SetTint(Colors.White.ToPlatform());
#pragma warning restore CA1416 // Validate platform compatibility
                handler.PlatformView.SetPadding(40, 0, 40, 0);
#elif __IOS__
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
                //handler.PlatformView.SetPadding(40, 0, 40, 0);
#endif
            }
        });

        //MainPage = new ContentPage();

        MainPage = new AppShell(serviceProvider.GetService<AppShellViewModel>());
        this.serviceProvider=serviceProvider;
    }

    protected override async void OnStart()
    {
        base.OnStart();

        var userStateContext = serviceProvider.GetService<UserStateContext>();
        await userStateContext.CheckUserLoginDetails();

        CrossFirebaseCloudMessaging.Current.NotificationTapped += async (sender, e) =>
        {
            IsNotificationReceived = true;

            var notiFic = new Models.Notification.Notification();
            foreach (var keyValue in e.Notification.Data)
            {
                if (keyValue.Key.ToUpper() == "Severity".ToUpper())
                {
                    SeverityLevel tmpVal;
                    if (Enum.TryParse<SeverityLevel>(keyValue.Value, out tmpVal))
                    {
                        notiFic.Severity = tmpVal;
                    }
                }
                else if (keyValue.Key.ToUpper() == "Date".ToUpper())
                {
                    DateTime tmpVal;
                    if (DateTime.TryParse(keyValue.Value, out tmpVal))
                    {
                        notiFic.CreationDate = tmpVal;
                    }
                }
                else if (keyValue.Key.ToUpper() == "AccountId".ToUpper())
                {
                    notiFic.AccountId = new Guid(keyValue.Value);
                }
                else if (keyValue.Key.ToUpper() == "ID".ToUpper())
                {
                    notiFic.Id = new Guid(keyValue.Value);
                }
                else if (keyValue.Key.ToUpper() == "Title".ToUpper())
                {
                    notiFic.Title = keyValue.Value;
                }
                else if (keyValue.Key.ToUpper() == "Body".ToUpper())
                {
                    notiFic.Body = keyValue.Value;
                }
            }

            await App.Current.MainPage.Navigation.PushAsync(new NotificationDetailPage(notiFic));

            var notificationService = serviceProvider.GetService<INotificationService>();

            await notificationService.AddUpdateNotificationStatus(new UserNotification()
            {
                UserId = new Guid(App.UserDetails.UserId ?? App.UserDetails.AccountId),
                NotificationId = notiFic.Id.Value,
                Status = Models.Constants.NotificationStatus.Read
            });
            WeakReferenceMessenger.Default.Send("notification-read");
        };

        CrossFirebaseCloudMessaging.Current.NotificationReceived += (sender, e) =>
        {
            WeakReferenceMessenger.Default.Send("notification-received");
        };

        CrossFirebaseCloudMessaging.Current.TokenChanged += async (sender, e) =>
        {
            if (Preferences.ContainsKey(AppConst.AppConstants.UserDeviceTokenIdKey))
            {
                var notificationService = serviceProvider.GetService<INotificationService>();
                var id = Preferences.Get(AppConst.AppConstants.UserDeviceTokenIdKey, default(string));
                Preferences.Remove(AppConst.AppConstants.UserDeviceTokenIdKey);

                await notificationService.DeleteDeviceToken(id);
            }

            if (Preferences.ContainsKey(AppConst.AppConstants.FCMDeviceTokenKey))
                Preferences.Remove(AppConst.AppConstants.FCMDeviceTokenKey);
        };
    }

    protected override void OnResume()
    {
        Console.WriteLine("onresume");
        base.OnResume();
    }

    protected override void OnSleep()
    {
        Console.WriteLine("OnSleep");
        base.OnSleep();
    }

}
