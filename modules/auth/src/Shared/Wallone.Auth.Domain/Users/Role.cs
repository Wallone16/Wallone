using Wallone.Shared.Domain.Users;

namespace Wallone.Auth.Domain.Users
{
    public sealed class Role
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string InternalName { get; private set; }
        public UserRole UserRole { get; private set; }
        public IEnumerable<Permission> Permissions { get; private set; }

        public Role(
            int id,
            string name,
            string internalName)
        {
            Id = id;
            Name = name;
            InternalName = internalName;
            UserRole = new();
            Permissions = Enumerable.Empty<Permission>();
        }
    }
}