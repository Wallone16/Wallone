using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using OpenIddict.Abstractions;
using System.Security.Claims;
using Wallone.Auth.Services.Contracts.Users;

namespace Wallone.Auth.Services.Users
{
    internal sealed class AuthService : IAuthService
    {
        public string BuildRedirectUrl(HttpRequest request, IDictionary<string, StringValues> parameters)
        {
            var url = $"{request.PathBase}{request.Path}{QueryString.Create(parameters)}";
            return url;
        }

        public IEnumerable<string> GetDestinations(Claim claim)
        {
            var destinations = new List<string>();

            if (claim.Type is OpenIddictConstants.Claims.Email or ClaimTypes.NameIdentifier or ClaimTypes.Email)
            {
                destinations.Add(OpenIddictConstants.Destinations.AccessToken);
            }

            return destinations;
        }

        public bool IsAuthenticated(AuthenticateResult authenticateResult, OpenIddictRequest request)
        {
            if (!authenticateResult.Succeeded)
            {
                return false;
            }

            if (request.MaxAge.HasValue && authenticateResult.Properties != null)
            {
                var maxAgeSeconds = TimeSpan.FromSeconds(request.MaxAge.Value);
                var expired = !authenticateResult.Properties.IssuedUtc.HasValue ||
                    DateTimeOffset.UtcNow - authenticateResult.Properties.IssuedUtc > maxAgeSeconds;

                if (expired)
                {
                    return false;
                }
            }

            return true;
        }

        public IDictionary<string, StringValues> ParseOAuthParameters(HttpContext httpContext, IEnumerable<string>? excluding = null)
        {
            excluding ??= new List<string>();

            var parameters = httpContext.Request.HasFormContentType
                ? httpContext.Request.Form
                    .Where(x => !excluding.Contains(x.Key))
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                : httpContext.Request.Query
                    .Where(x => !excluding.Contains(x.Key))
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return parameters;
        }
    }
}