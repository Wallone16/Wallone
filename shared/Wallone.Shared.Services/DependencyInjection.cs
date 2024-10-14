using Microsoft.Extensions.DependencyInjection;
using Wallone.Shared.Services.Implementations;
using Wallone.Shared.Services.Interfaces;

namespace Wallone.Shared.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services)
        {
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IGuidGenerator, GuidGenerator>();

            return services;
        }
    }
}