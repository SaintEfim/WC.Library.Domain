using FluentValidation;

namespace WC.Library.Domain.Services.Validators;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .Length(8, 64)
            .Matches(@"(?=.*\d)").WithMessage("At least one digit (0-9)")
            .Matches("(?=.*[a-z])").WithMessage("At least one lowercase letter (a-z)")
            .Matches("(?=.*[A-Z])").WithMessage("At least one uppercase letter (A-Z)")
            .Matches(@"^\S*$");
    }
}