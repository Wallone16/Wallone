using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wallone.Auth.Domain.Users;
using Wallone.Auth.EntityFramework.EntityFramework;
using Wallone.Auth.EntityFramework.Users;

namespace Wallone.Auth.EntityFramework
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();

            services
                .AddOpenIddict()
                .AddCoreAuth();

            return services;
        }

        private static OpenIddictBuilder AddCoreAuth(this OpenIddictBuilder openIddictBuilder)
        {
            openIddictBuilder.AddCore(options =>
            {
                options
                    .UseEntityFrameworkCore()
                    .UseDbContext<AuthDbContext>();
            });

            return openIddictBuilder;
        }
    }
}