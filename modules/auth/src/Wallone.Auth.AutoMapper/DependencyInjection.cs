using Microsoft.Extensions.DependencyInjection;
using Wallone.Auth.AutoMapper.Users;

namespace Wallone.Auth.AutoMapper
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserMappingProfile));

            return services;
        }
    }
}