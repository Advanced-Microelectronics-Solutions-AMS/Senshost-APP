using Microsoft.Extensions.DependencyInjection;
using Senshost.Common.Interfaces;
using Senshost.Services.Auth;

namespace Senshost.Services
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
