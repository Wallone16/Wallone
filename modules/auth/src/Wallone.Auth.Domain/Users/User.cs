namespace Wallone.Auth.Domain.Users
{
    public sealed class User
    {
        public Guid Id { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public byte[] Password { get; private set; }
        public IEnumerable<Role> Roles { get; private set; }

        protected User() { }

        public User(
            Guid id,
            string userName,
            string email,
            byte[] password,
            IEnumerable<Role> roles)
        {
            Id = id;
            UserName = userName;
            Email = email;
            Password = password;
            Roles = roles;
        }
    }
}