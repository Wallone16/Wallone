using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using Wallone.Auth.Services.Contracts.Users;

namespace Wallone.Auth.Services.Users
{
    public sealed class HashPasswordService(
        IOptions<SaltSettings> saltSettings) : IHashPasswordService
    {
        public byte[] HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256
                    .ComputeHash(Encoding.Unicode.GetBytes(password + saltSettings.Value.Salt));
            }
        }

        public bool IsVerifyPassword(string password, byte[] currentUserPassword)
        {
            return HashPassword(password).SequenceEqual(currentUserPassword);
        }
    }
}