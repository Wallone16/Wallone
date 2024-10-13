namespace Wallone.Auth.Domain.Users
{
    public sealed class Role
    {
        public static Role Admin = new Role(1, "Admin", "Администратор");
        public static Role User = new Role(2, "User", "Пользователь");
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string InternalName { get; private set; }
        public ICollection<Permission> Permissions { get; init; }
        public ICollection<User> Users { get; init; }

        public Role(
            int id,
            string name,
            string internalName)
        {
            Id = id;
            Name = name;
            InternalName = internalName;
            Permissions = [];
            Users = [];
        }
    }
}