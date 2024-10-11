using Microsoft.EntityFrameworkCore;
using Wallone.Auth.Domain.Users;
using Wallone.Auth.EntityFramework.EntityFramework;

namespace Wallone.Auth.EntityFramework.Users
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;

        public UserRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<User> InsertAsync(User entity, CancellationToken cancellationToken = default)
        {
            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}