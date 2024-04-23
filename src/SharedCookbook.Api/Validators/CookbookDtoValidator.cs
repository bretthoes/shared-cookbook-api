using FluentValidation;
using SharedCookbook.Api.Data.Dtos;

namespace SharedCookbook.Api.Validators;

public class CreateCookbookDtoValidator : AbstractValidator<CreateCookbookDto>
{
    public CreateCookbookDtoValidator()
    {
        RuleFor(x => x)
            .NotNull().WithMessage("Cookbook cannot be null.");
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.Title)
            .Length(2, 18).WithMessage("Title must be 2-18 characters.");

    }
}