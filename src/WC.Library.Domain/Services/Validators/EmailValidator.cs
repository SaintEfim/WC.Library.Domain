using System.Text.RegularExpressions;
using FluentValidation;

namespace WC.Library.Domain.Services.Validators;

public partial class EmailValidator : AbstractValidator<string>
{
    public EmailValidator()
    {
        RuleFor(x => x)
            .EmailAddress()
            .Custom((email, context) =>
            {
                if (!TryGetDomain(email, out var domain))
                {
                    context.AddFailure("Invalid email format or domain");
                    return;
                }

                var allowedDomains = new List<string?>
                {
                    "gmail.com",
                    "mail.ru",
                    "yahoo.com",
                    "hotmail.com",
                    "outlook.com"
                };

                if (domain != null && !allowedDomains.Contains(domain))
                {
                    context.AddFailure($"Invalid domain '{domain}'");
                }
            });
    }

    private static bool TryGetDomain(string email, out string? domain)
    {
        domain = null;
        try
        {
            var match = MyRegex().Match(email);
            if (!match.Success)
            {
                return false;
            }

            domain = match.Groups[2].Value;
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    [GeneratedRegex(@"^(.+)@(.+)$")]
    private static partial Regex MyRegex();
}