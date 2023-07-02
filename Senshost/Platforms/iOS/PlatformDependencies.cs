using Senshost.Views;
using Senshost.ViewModels;
using Senshost.Constants;
using Senshost.Services;
using Senshost.Common.Interfaces;
using Senshost.Platforms.iOS.Services;
using Plugin.Firebase.Bundled.Platforms.IOS;

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
                events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) =>
                {
                    CrossFirebase.Initialize(app, launchOptions, CreateCrossFirebaseSettings());
                    return false;
                }));
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
