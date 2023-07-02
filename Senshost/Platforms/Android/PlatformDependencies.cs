using Senshost.Views;
using Senshost.ViewModels;
using Senshost.Constants;
using Senshost.Services;
using Senshost.Common.Interfaces;
using Senshost.Platforms.Android.Services;
using Senshost.Platforms.Android.CustomRenderers;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Bundled.Shared;
using Plugin.Firebase.Bundled.Platforms.Android;
using Senshost.Controls;

namespace Senshost.Platforms
{
    public static partial class PlatformDependencies
    {
        public static partial MauiAppBuilder RegisterPlatformDependencies(this MauiAppBuilder builder)
        {
            builder.RegisterFirebaseServices();

            //Views
            builder.Services.AddSingleton<LoginPage>()
                .AddSingleton<DashboardPage>()
                .AddSingleton<EventListPage>()
                .AddSingleton<NotificationListPage>()
                .AddSingleton<NotificationDetailPage>()
                .AddSingleton<LoadingPage>()
                .AddSingleton<AppShell>();

            //Services
            builder.Services
                .AddSingleton<HttpClient>(new HttpClient() { BaseAddress = new Uri(AppConstants.WebApiBaseUrl) })
                .AddSingleton<IStorageService, SecureStorageService>()
                .AddTransient<IGetDeviceInfo, GetDeviceInfo>();

            //ViewModels
            builder.Services.AddSingleton<LoginPageViewModel>()
                            .AddSingleton<AppShellViewModel>()
                            .AddSingleton<DashboardPageViewModel>()
                            .AddSingleton<EventListPageViewModel>()
                            .AddSingleton<NotificationListPageViewModel>()
                            .AddSingleton<NotificationDetailPageViewModel>()
                            .AddSingleton<UserStateContext>()
                            .AddSingleton<NotificationDetailPage>();

            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<Shell, CustomShellHandler>();
            });


            builder.Services.RegisterServiceDependencies();
            return builder;
        }

        private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
        {
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddAndroid(android => android.OnCreate((activity, state) =>
                    CrossFirebase.Initialize(activity, CreateCrossFirebaseSettings())));
            });

            builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);

            return builder;
        }

        private static CrossFirebaseSettings CreateCrossFirebaseSettings()
        {
            return new CrossFirebaseSettings(isAuthEnabled: true, isCloudMessagingEnabled: true);
        }
    }
}
