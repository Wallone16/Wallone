using Microsoft.EntityFrameworkCore;
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
            services.AddDbContext<AuthDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("ConnectionString:Auth")));

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}