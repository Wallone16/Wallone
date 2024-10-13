using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using Wallone.Auth.Services.Contracts.Users;

namespace Wallone.Auth.Web.Authentication
{
    public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PermissionAuthorizationHandler(
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            string? userId = context.User.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (!Guid.TryParse(userId, out Guid parsedUserId))
            {
                return;
            }

            using IServiceScope scope = _serviceScopeFactory.CreateScope();

            IPermissionService permissionService = scope.ServiceProvider
                .GetRequiredService<IPermissionService>();

            var cancellationToken = new CancellationTokenSource().Token;

            HashSet<string> permissions = await permissionService
                .GetPermissionsAsync(parsedUserId, cancellationToken);

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
