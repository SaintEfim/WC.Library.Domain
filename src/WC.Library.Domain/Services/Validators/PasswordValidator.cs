using FluentValidation;

namespace WC.Library.Domain.Services.Validators;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(x => x)
            .NotEmpty();

        RuleFor(x => x)
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.");

        RuleFor(x => x)
            .MaximumLength(64).WithMessage("Password must be no more than 64 characters.");

        RuleFor(x => x)
            .Matches(@"(?=.*\d)").WithMessage("Password must contain at least one digit.");

        RuleFor(x => x)
            .Matches("(?=.*[a-z])").WithMessage("Password must contain at least one lowercase letter.");

        RuleFor(x => x)
            .Matches("(?=.*[A-Z])").WithMessage("Password must contain at least one uppercase letter.");

        RuleFor(x => x)
            .Matches("[!@#$%^&*()-+=]").WithMessage("Password must contain at least one special character.");

        RuleFor(x => x)
            .Matches(@"^\S*$").WithMessage("Password cannot contain whitespace characters.");
    }
}