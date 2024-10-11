namespace Wallone.Auth.Services.Contracts.Users
{
    public interface IHashPasswordService
    {
        byte[] HashPassword(string password);
        bool IsVerifyPassword(string password, byte[] currentUserPassword);
    }
}