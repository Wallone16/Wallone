using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wallone.Auth.Services.Contracts.Users.Commands;

namespace Wallone.Auth.Web.Pages
{
    public sealed class RegisterModel : PageModel
    {
        private readonly IMediator _mediator;

        public RegisterModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public string ReturnUrl { get; set; }
        public RegisterUserCommand RegisterUser { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; } = Enumerable.Empty<string>();

        public IActionResult OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator
                .Send(request, cancellationToken);

            if (!response.IsSuccess)
            {
                ErrorMessages = response.ErrorMessages;

                return Page();
            }

            return Redirect(ReturnUrl);
        }
    }
}