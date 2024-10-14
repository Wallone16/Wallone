namespace Wallone.Auth.Domain.Users
{
    public interface IUserRepository
    {
        Task<User> InsertAsync(User entity, CancellationToken cancellationToken = default);
        Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}