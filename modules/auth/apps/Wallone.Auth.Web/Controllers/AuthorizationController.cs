using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Security.Claims;
using OpenIddict.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Wallone.Auth.Web.Attributes;
using Wallone.Auth.Services.Contracts.Users.Queries;
using Wallone.Auth.Services.Contracts.Users;

namespace Wallone.Auth.Web.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public sealed class AuthorizationController : ControllerBase
    {
        private readonly IOpenIddictApplicationManager _applicationManager;
        private readonly IOpenIddictAuthorizationManager _authorizationManager;
        private readonly IOpenIddictScopeManager _scopeManager;
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;

        public AuthorizationController(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictAuthorizationManager authorizationManager,
            IOpenIddictScopeManager scopeManager,
            IMediator mediator,
            IAuthService authService)
        {
            _applicationManager = applicationManager;
            _authorizationManager = authorizationManager;
            _scopeManager = scopeManager;
            _mediator = mediator;
            _authService = authService;
        }

        [HttpGet("~/connect/authorize")]
        [HttpPost("~/connect/authorize")]
        public async Task<IActionResult> Authorize(CancellationToken cancellationToken)
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var isAuthenticated = _authService.IsAuthenticated(result, request);

            var parameters = _authService.ParseOAuthParameters(HttpContext);

            if (!isAuthenticated)
            {
                return Challenge(
                    authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = _authService.BuildRedirectUrl(HttpContext.Request, parameters),
                    }
                );
            }

            var response = await _mediator
                .Send(new GetUserByEmailQuery
                {
                    Email = result.Principal!.FindFirst(ClaimTypes.Email)!.Value
                },
                cancellationToken);

            if (!response.IsSuccess)
            {
                return Challenge(
                    authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = _authService.BuildRedirectUrl(HttpContext.Request, parameters)
                    }
                );
            }

            var user = response.Data;

            var application = await _applicationManager.FindByClientIdAsync(request.ClientId!, cancellationToken) ??
                throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

            var identity = new ClaimsIdentity(
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role);

            identity
                .SetClaim(Claims.Subject, user!.Email)
                .SetClaim(ClaimTypes.Email, user.Email)
                .SetClaim(ClaimTypes.NameIdentifier, user.Id.ToString());

            identity.SetScopes(request.GetScopes());
            identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());

            var authorizations = await _authorizationManager
                .FindAsync(
                    subject: user.Email,
                    client: await _applicationManager.GetIdAsync(application),
                    status: Statuses.Valid,
                    type: AuthorizationTypes.Permanent,
                    scopes: request.GetScopes())
                .ToListAsync();

            var authorization = authorizations.LastOrDefault();

            authorization ??= await _authorizationManager.CreateAsync(
                identity: identity,
                subject: user.Email,
                client: await _applicationManager.GetIdAsync(application, cancellationToken),
                type: AuthorizationTypes.Permanent,
                scopes: identity.GetScopes(),
                cancellationToken);

            identity.SetAuthorizationId(await _authorizationManager.GetIdAsync(authorization));
            identity.SetDestinations(_authService.GetDestinations);

            return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpPost("~/connect/token")]
        public async Task<IActionResult> Exchange(CancellationToken cancellationToken)
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("Запрос на подключение OpenID Connect не получен.");

            if (!request.IsAuthorizationCodeGrantType() && !request.IsRefreshTokenGrantType())
            {
                throw new InvalidOperationException("Предоставленный grant_type не поддерживается");
            }

            var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            var response = await _mediator
                .Send(new GetUserByEmailQuery
                {
                    Email = result.Principal!.GetClaim(Claims.Subject)!
                },
                cancellationToken);

            if (!response.IsSuccess)
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "Невалидный токен"
                    }!));
            }

            var user = response.Data;

            var identity = new ClaimsIdentity(
                claims: result.Principal!.Claims,
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role
            );

            identity
                .SetClaim(Claims.Subject, user!.Email)
                .SetClaim(Claims.Email, user.Email)
                .SetClaim(ClaimTypes.NameIdentifier, user.Id.ToString());

            identity.SetDestinations(_authService.GetDestinations);

            return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [Authorize, FormValueRequired("submit.Accept")]
        [HttpPost("~/connect/authorize"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(CancellationToken cancellationToken = default)
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            var userResult = await _mediator.Send(new GetUserByEmailQuery
            {
                Email = result.Principal!.GetClaim(Claims.Subject)!
            });

            var user = userResult.Data;

            if (user == null)
            {
                throw new InvalidOperationException("The user details cannot be retrieved.");
            }

            var application = await _applicationManager.FindByClientIdAsync(request.ClientId!, cancellationToken) ??
                throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

            var authorizations = await _authorizationManager.FindAsync(
                subject: user.Email,
                client: await _applicationManager.GetIdAsync(application, cancellationToken),
                status: Statuses.Valid,
                type: AuthorizationTypes.Permanent,
                scopes: request.GetScopes()).ToListAsync(cancellationToken);

            if (authorizations.Count is 0 && await _applicationManager.HasConsentTypeAsync(application, ConsentTypes.External))
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The logged in user is not allowed to access this client application."
                    }!));
            }

            var identity = new ClaimsIdentity(
                claims: result.Principal!.Claims,
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role
            );

            identity
                .SetClaim(Claims.Subject, user.Email)
                .SetClaim(Claims.Email, user.Email)
                .SetClaim(ClaimTypes.NameIdentifier, user.Id.ToString());

            identity.SetDestinations(_authService.GetDestinations);

            var authorization = authorizations.LastOrDefault();

            authorization ??= await _authorizationManager.CreateAsync(
                identity: identity,
                subject: user.Email,
                client: await _applicationManager.GetIdAsync(application, cancellationToken),
                type: AuthorizationTypes.Permanent,
                scopes: identity.GetScopes());

            identity.SetAuthorizationId(await _authorizationManager.GetIdAsync(authorization));
            identity.SetDestinations(_authService.GetDestinations);

            return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [Authorize, FormValueRequired("submit.Deny")]
        [HttpPost("~/connect/authorize"), ValidateAntiForgeryToken]
        public IActionResult Deny() => Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        [HttpPost("~/connect/logout")]
        public async Task<IActionResult> LogoutPost()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return SignOut(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = "/"
                }
            );
        }
    }
}