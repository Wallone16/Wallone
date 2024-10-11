using Microsoft.Extensions.DependencyInjection;

namespace Wallone.Shared.EntityFramework
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedEntityFramework(this IServiceCollection services)
        {
            return services;
        }
    }
}