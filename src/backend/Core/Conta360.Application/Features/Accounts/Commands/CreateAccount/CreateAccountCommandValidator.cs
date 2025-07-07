using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Conta360.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.InitialBalance)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must be non-negative.");
        }
    }
}