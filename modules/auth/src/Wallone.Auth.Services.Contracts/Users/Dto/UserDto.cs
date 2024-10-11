namespace Wallone.Auth.Services.Contracts.Users.Dto
{
    public sealed record UserDto(
        Guid Id,
        string Email,
        string UserName);
}