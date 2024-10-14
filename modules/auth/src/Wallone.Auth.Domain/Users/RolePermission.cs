namespace Wallone.Auth.Domain.Users
{
    public sealed class RolePermission
    {
        public int RoleId { get; private set; }
        public int PermissionId { get; private set; }

        public RolePermission(
            int roleId,
            int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }
}