using System.Collections.Immutable;
using FluentValidation;
using WC.Library.Domain.Models;

namespace WC.Library.Domain.Services.Validators;

public abstract class ValidatorBase<TDomain>
    where TDomain : class, IModel
{
    protected ValidatorBase(IEnumerable<IValidator> validators)
    {
        Validators = validators;
    }

    protected IEnumerable<IValidator> Validators { get; }

    protected void Validate<TV>(
        TDomain model,
        CancellationToken cancellationToken = default)
    {
        var validators = Validators.Where(v => v.GetType().IsAssignableTo(typeof(TV))).Cast<IValidator<TDomain>>();

        Validate(model, validators, cancellationToken);
    }

    private static void Validate<TPayload>(
        TPayload model,
        IEnumerable<IValidator<TPayload>> source,
        CancellationToken cancellationToken = default)
        where TPayload : class
    {
        var tasks = source.Select(async x => await x.ValidateAsync(model, cancellationToken));
        var results = Task.WhenAll(tasks).Result;

        var failures = results.SelectMany(x => x.Errors)
            .Where(x => x != null)
            .ToImmutableList();

        if (!failures.IsEmpty)
        {
            throw new ValidationException(failures);
        }
    }
}