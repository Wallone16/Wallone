namespace Wallone.Auth.Domain.Users
{
    public sealed class User
    {
        public Guid Id { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }

        public User(
            Guid id,
            string userName,
            string email)
        {
            Id = id;
            UserName = userName;
            Email = email;
        }
    }
}