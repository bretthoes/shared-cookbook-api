using FluentValidation;
using shared_cookbook_api.Data.Dtos;

namespace shared_cookbook_api.Validators;

public class CookbookDtoValidator : AbstractValidator<CookbookDto>
{
    public CookbookDtoValidator()
    {
        RuleFor(x => x)
            .NotNull().WithMessage("Cookbook cannot be null.");
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");
    }
}