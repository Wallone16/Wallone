using Microsoft.AspNetCore.Authorization;

namespace Wallone.Auth.Web.Authentication
{
    public sealed class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}