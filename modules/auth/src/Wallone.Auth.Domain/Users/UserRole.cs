namespace Wallone.Auth.Domain.Users
{
    public sealed class UserRole
    {
        public int Id { get; private set; }
        public Role Role { get; private set; }
        public User User { get; private set; }
    }
}