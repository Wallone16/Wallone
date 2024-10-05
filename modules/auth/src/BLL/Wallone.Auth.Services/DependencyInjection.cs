using Microsoft.Extensions.DependencyInjection;

namespace Wallone.Auth.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services)
        {
            return services;
        }
    }
}