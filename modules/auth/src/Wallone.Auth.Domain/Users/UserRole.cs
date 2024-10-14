namespace Wallone.Auth.Domain.Users
{
    public sealed class UserRole
    {
        public Guid UserId { get; private set; }
        public int RoleId { get; private set; }

        public UserRole(
            Guid userId,
            int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}