using FluentValidation;

namespace WC.Library.Domain.Services.Validators;

public class EmailValidator : AbstractValidator<string>
{
    public EmailValidator()
    {
        RuleFor(x => x)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(8).WithMessage("Email must be at least 8 characters.")
            .MaximumLength(64).WithMessage("Email must be no more than 64 characters.")
            .Matches(@"^\S*$").WithMessage("Email cannot contain whitespace characters.")
            .EmailAddress().WithMessage("Invalid email address format.")
            .Custom((email, context) =>
            {
                var domain = email.Split('@').LastOrDefault();

                var allowedDomains = new List<string>
                {
                    "gmail.com",
                    "mail.ru",
                    "yahoo.com",
                    "hotmail.com",
                    "outlook.com",
                    "yandex.ru",
                    "protonmail.com",
                    "aol.com",
                    "icloud.com",
                    "zoho.com"
                };

                if (domain != null && !allowedDomains.Contains(domain))
                {
                    context.AddFailure($"Invalid domain '{domain}'");
                }
            });
    }
}