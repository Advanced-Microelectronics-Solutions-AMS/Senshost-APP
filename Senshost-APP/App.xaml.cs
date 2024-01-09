using Android.OS;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using Firebase.Crashlytics;
using Microsoft.Maui.Platform;
using Senshost_APP.Common.Interfaces;
using Senshost_APP.Handlers;
using Senshost_APP.Models.Account;
using Senshost_APP.Models.Notification;
using Senshost_APP.ViewModels;
using Senshost_APP.Views;

namespace Senshost_APP
{
    public partial class App : Application
    {
        public static LogedInUserDetails UserDetails;
        public static string ApiToken;
        private readonly UserStateContext userStateContext;
        private readonly IServiceProvider serviceProvider;
        private bool checkingUserDetails;

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
            {
                if (view is BorderlessEntry)
                {
#if __ANDROID__
                    handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());

                    if (OperatingSystem.IsOSPlatformVersionAtLeast("android", 29))
                        handler.PlatformView.TextCursorDrawable.SetTint(Colors.White.ToPlatform());

                    handler.PlatformView.SetPadding(40, 0, 40, 0);
                    handler.PlatformView.BackgroundTintList =
                    Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
#elif __IOS__
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
                //handler.PlatformView.SetPadding(40, 0, 40, 0);
#endif
                }
            });

            Microsoft.Maui.Handlers.WebViewHandler.Mapper.AppendToMapping(nameof(WebView), (handler, view) =>
            {
#if __ANDROID__
                handler.PlatformView.Settings.MediaPlaybackRequiresUserGesture = false;
#endif
            });

            userStateContext = serviceProvider.GetService<UserStateContext>();
            MainPage = new AppShell(userStateContext);
            WeakReferenceMessenger.Default.Register<NotificationTapped>(this, ShowNotificationDetailPage());
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            this.serviceProvider = serviceProvider;
            //Connectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
        }

        protected override async void OnStart()
        {
            base.OnStart();

            if (!checkingUserDetails)
            {
                checkingUserDetails = true;
                await userStateContext.CheckUserLoginDetails();

                if (userStateContext.IsAuthorized)
                    _ = Task.Run(() =>
                    {
                        serviceProvider.GetService<NotificationListPage>();
                    });
            }

            PermissionStatus status = await Permissions.CheckStatusAsync<Senshost_APP.Platforms.Android.PostNotifications>();
            if (status != PermissionStatus.Granted)
                status = await Permissions.RequestAsync<Senshost_APP.Platforms.Android.PostNotifications>();
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (userStateContext.IsAuthorized)
                WeakReferenceMessenger.Default.Send("notification-check");
        }

        protected override void OnSleep()
        {
            base.OnSleep();
        }

        private MessageHandler<object, NotificationTapped> ShowNotificationDetailPage()
        {
            return async (r, m) =>
            {
                if (!checkingUserDetails)
                {
                    checkingUserDetails = true;
                    await userStateContext.CheckUserLoginDetails();
                }

                await Shell.Current.GoToAsync($"{nameof(NotificationDetailPage)}", true, new Dictionary<string, object> { { "Notification", m.Notification } });

                _ = Task.Run(async () =>
                {
                    try
                    {
                        var notificationService = serviceProvider.GetService<INotificationService>();

                        await notificationService.AddUpdateNotificationStatus(new UserNotification()
                        {
                            UserId = new Guid(App.UserDetails.UserId ?? App.UserDetails.AccountId),
                            NotificationId = m.Notification.Id.Value,
                            Status = Models.Constants.NotificationStatus.Read
                        });

                        WeakReferenceMessenger.Default.Send("notification-read");
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToLower() == "invalid action.")
                            return;

                        AppShell.Current.Dispatcher.Dispatch(async () =>
                        {
                            await AppShell.Current.DisplayAlert("Error", ex.Message, "Close");
                        });
                    }
                });
            };
        }

        private void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            FirebaseCrashlytics.Instance.RecordException(Java.Lang.Throwable.FromException(e.Exception));
        }

        //private async void Current_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        //{
        //    if (e.NetworkAccess != NetworkAccess.Internet || e.NetworkAccess == NetworkAccess.None)
        //    {
        //        var toast = Toast.Make("Connection lost!", ToastDuration.Long);
        //        await toast.Show();
        //    }
        //    else if (e.NetworkAccess == NetworkAccess.Internet)
        //    {
        //        var toast = Toast.Make("Back online", ToastDuration.Long);
        //        await toast.Show();
        //    }
        //}
    }
}