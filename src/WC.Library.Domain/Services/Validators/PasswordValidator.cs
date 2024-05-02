using FluentValidation;

namespace WC.Library.Domain.Services.Validators;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .MaximumLength(64).WithMessage("Password must be no more than 64 characters.")
            .Matches(@"(?=.*\d)").WithMessage("Password must contain at least one digit.")
            .Matches("(?=.*[a-z])").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("(?=.*[A-Z])").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[!@#$%^&*()-+=]").WithMessage("Password must contain at least one special character.")
            .Matches(@"^\S*$").WithMessage("Password cannot contain whitespace characters.");
    }
}