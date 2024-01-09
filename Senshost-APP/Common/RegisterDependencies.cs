using Senshost_APP.Services;
using Senshost_APP.ViewModels;
using Senshost_APP.Common.Interfaces;
using Senshost_APP.Constants;
using Senshost_APP.Platforms;
using Senshost_APP.Views;

namespace Senshost_APP.Common
{
    public static class RegisterDependencies
    {
        public static MauiAppBuilder RegisterAppDependencies(this MauiAppBuilder builder)
        {
            //Views
            builder.Services.AddSingleton<LoginPage>()
                .AddSingleton<DashboardPage>()
                .AddSingleton<EventListPage>()
                .AddSingleton<AssetManagementPage>()
                .AddSingleton<NotificationListPage>()
                .AddTransient<NotificationDetailPage>();

            builder.Services
                .AddSingleton<HttpClient>(new HttpClient() { BaseAddress = new Uri(AppConstants.WebApiBaseUrl) })
                .AddSingleton<IStorageService, SecureStorageService>()
                .AddSingleton<LoginPageViewModel>()
                .AddSingleton<DashboardPageViewModel>()
                .AddSingleton<EventListPageViewModel>()
                .AddSingleton<AssetManagementPageViewModel>()
                .AddSingleton<NotificationListPageViewModel>()
                .AddSingleton<UserStateContext>()
                .AddTransient<NotificationDetailPageViewModel>();

            builder.RegisterPlatformDependencies();
            builder.Services.RegisterServiceDependencies();
            return builder;
        }
    }
}
