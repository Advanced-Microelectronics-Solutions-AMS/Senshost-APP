using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Bundled.Platforms.Android;
using Plugin.Firebase.Bundled.Shared;
using Plugin.Firebase.Crashlytics;
using Senshost_APP.Common.Interfaces;
using Senshost_APP.Platforms.Android.CustomRenderers;
using Senshost_APP.Platforms.Android.Services;

namespace Senshost_APP.Platforms
{
    public static partial class PlatformDependencies
    {
        public static partial MauiAppBuilder RegisterPlatformDependencies(this MauiAppBuilder builder)
        {
            //Services
            builder.Services
                .AddTransient<IGetDeviceInfo, GetDeviceInfo>();

            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<Shell, CustomShellHandler>();
            });

            builder.RegisterFirebaseServices();
            return builder;
        }

        private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
        {
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddAndroid(android => android.OnCreate((activity, state) =>
                    CrossFirebase.Initialize(activity, CreateCrossFirebaseSettings())));
                CrossFirebaseCrashlytics.Current.SetCrashlyticsCollectionEnabled(true);
            });

            builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);

            return builder;
        }

        private static CrossFirebaseSettings CreateCrossFirebaseSettings()
        {
            return new CrossFirebaseSettings(isAuthEnabled: true, isCloudMessagingEnabled: false, isAnalyticsEnabled: true);
        }
    }
}
