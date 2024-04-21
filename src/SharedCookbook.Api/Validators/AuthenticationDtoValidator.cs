using FluentValidation;
using shared_cookbook_api.Data.Dtos;

namespace shared_cookbook_api.Validators;

public class AuthenticationDtoValidator : AbstractValidator<AuthenticationDto>
{
    public AuthenticationDtoValidator()
    {
        RuleFor(x => x)
            .NotNull().WithMessage("Authentication cannot be null.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(6, 20).WithMessage("Password must be between 6 and 20 characters.")
            .Must(HaveLetterAndNumber).WithMessage("Password must contain both letters and numbers.");
    }

    private bool HaveLetterAndNumber(string password)
    {
        return !string.IsNullOrEmpty(password) && password.Any(char.IsLetter) && password.Any(char.IsDigit);
    }
}