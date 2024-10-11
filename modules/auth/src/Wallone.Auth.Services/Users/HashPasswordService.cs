using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using Wallone.Auth.Services.Contracts.Users;
using Wallone.Auth.Services.Settings;

namespace Wallone.Auth.Services.Users
{
    public sealed class HashPasswordService : IHashPasswordService
    {
        private readonly IOptions<SaltOptions> _saltOptions;

        public HashPasswordService(IOptions<SaltOptions> saltOptions)
        {
            _saltOptions = saltOptions;
        }

        public byte[] HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256
                    .ComputeHash(Encoding.Unicode.GetBytes(password + _saltOptions.Value.Salt));
            }
        }

        public bool IsVerifyPassword(string password, byte[] currentUserPassword)
        {
            return HashPassword(password).SequenceEqual(currentUserPassword);
        }
    }
}