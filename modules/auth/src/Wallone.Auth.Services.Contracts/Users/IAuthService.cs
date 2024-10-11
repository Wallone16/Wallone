using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using OpenIddict.Abstractions;
using System.Security.Claims;

namespace Wallone.Auth.Services.Contracts.Users
{
    public interface IAuthService
    {
        bool IsAuthenticated(AuthenticateResult authenticateResult, OpenIddictRequest request);
        IDictionary<string, StringValues> ParseOAuthParameters(HttpContext httpContext, IEnumerable<string>? excluding = null);
        string BuildRedirectUrl(HttpRequest request, IDictionary<string, StringValues> parameters);
        IEnumerable<string> GetDestinations(Claim claim);
    }
}