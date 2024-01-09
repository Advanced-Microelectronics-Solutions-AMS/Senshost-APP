using Senshost_APP.Common.Interfaces;
using Senshost_APP.Services.Auth;

namespace Senshost_APP.Services
{
    public static class ServiceDependencies
    {
        public static IServiceCollection RegisterServiceDependencies(this IServiceCollection services)
        {
            //Services
            services
                .AddTransient<IAuthService, AuthService>()
               .AddTransient<INotificationService, NotificationService>();

            return services;
        }
    }
}
