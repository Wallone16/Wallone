using Microsoft.Extensions.DependencyInjection;
using Wallone.Auth.Services.Contracts.Users;
using Wallone.Auth.Services.Users;
using Wallone.Auth.EntityFramework;
using FluentValidation;
using Wallone.Auth.Services.Contracts.Users.Commands;
using Wallone.Identity.Application.Validators;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Wallone.Shared.Services;

namespace Wallone.Auth.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(x =>
                x.RegisterServicesFromAssemblyContaining<Program>());

            services.AddScoped<IHashPasswordService, HashPasswordService>();
            services.AddScoped<IAuthService, AuthService>();

            services
                .AddMapping()
                .AddValidators()
                .AddAuthEntityFramework(configuration)
                .AddSharedServices();

            return services;
        }

        private static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();

            services.AddScoped<IValidator<RegisterUserCommand>, RegisterUserValidator>();
            services.AddScoped<IValidator<LoginUserCommand>, LoginUserValidator>();

            return services;
        }

        private static IServiceCollection AddMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AuthModuleMappings));

            return services;
        }
    }
}