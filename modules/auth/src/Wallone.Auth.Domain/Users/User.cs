namespace Wallone.Auth.Domain.Users
{
    public sealed class User
    {
        public Guid Id { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public byte[] Password { get; private set; }
        public UserRole UserRole { get; private set; }

        public User(
            Guid id,
            string userName,
            string email,
            byte[] password,
            UserRole userRole)
        {
            Id = id;
            UserName = userName;
            Email = email;
            Password = password;
            UserRole = userRole;
        }
    }
}