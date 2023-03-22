using Senshost.Views;
using Senshost.ViewModels;
using Senshost.Constants;
using Senshost.Services;
using Senshost.Common.Interfaces;
using Senshost.Platforms.iOS.Services;

namespace Senshost.Platforms
{
    public static partial class PlatformDependencies
    {
        public static partial IServiceCollection RegisterPlatformDependencies(this IServiceCollection services)
        {
            //Views
            services.AddSingleton<LoginPage>()
                .AddSingleton<DashboardPage>()
                .AddSingleton<EventListPage>()
                .AddSingleton<NotificationListPage>()
                .AddSingleton<LoadingPage>()
                .AddSingleton<AppShell>();

            //Services
            services
                .AddSingleton<HttpClient>(new HttpClient() { BaseAddress = new Uri(AppConstants.WebApiBaseUrl) })
                .AddSingleton<IStorageService, SecureStorageService>()
                .AddTransient<IGetDeviceInfo, GetDeviceInfo>();

            //ViewModels
            services.AddSingleton<LoginPageViewModel>()
                            .AddSingleton<AppShellViewModel>()
                            .AddSingleton<DashboardPageViewModel>()
                            .AddSingleton<EventListPageViewModel>()
                            .AddSingleton<NotificationListPageViewModel>()
                            .AddSingleton<UserStateContext>()
                            .AddSingleton<NotificationDetailPage>();

            services.RegisterServiceDependencies();
            return services;
        }
    }
}
