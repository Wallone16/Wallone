using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Wallone.Auth.Services.Contracts.Users.Commands;

namespace Wallone.Auth.Web.Pages
{
    public sealed class LoginModel : PageModel
    {
        private readonly IMediator _mediator;

        public LoginModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public LoginUserCommand LoginUser { get; set; }
        [BindProperty]
        public string ReturnUrl { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; } = Enumerable.Empty<string>();

        public IActionResult OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator
                .Send(command, cancellationToken);

            if (!response.IsSuccess)
            {
                ErrorMessages = response.ErrorMessages;

                return Page();
            }

            var user = response.Data;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, command.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var principals = new ClaimsPrincipal(
                new List<ClaimsIdentity>
                {
                    new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)
                }
            );

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principals);

            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            return Page();
        }
    }
}