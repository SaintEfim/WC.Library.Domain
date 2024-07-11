using System.Collections.Immutable;
using FluentValidation;
using WC.Library.Domain.Models;
using WC.Library.Domain.Validators;

namespace WC.Library.Domain.Services.Validators;

public abstract class ValidatorBase<TDomain>
    where TDomain : class, IModel
{
    protected ValidatorBase(IEnumerable<IValidator> validators)
    {
        Validators = validators;
    }

    protected IEnumerable<IValidator> Validators { get; }

    protected void Validate<TV>(TDomain model, CancellationToken cancellationToken = default)
    {
        var source = Validators
            .Where(v => v is IValidator<TDomain> && v.GetType().IsAssignableTo(typeof(TV)))
            .Cast<IValidator<TDomain>>();

        Validate(model, source, cancellationToken);
    }

    protected void Validate<TPayload, TV>(TPayload model, CancellationToken cancellationToken = default)
        where TPayload : class
    {
        var source = Validators
            .Where(v => v is IValidator<TPayload> && v.GetType().IsAssignableTo(typeof(TV)))
            .Cast<IValidator<TPayload>>();

        Validate(model, source, cancellationToken);
    }

    protected void Validate(
        TDomain model,
        string actionName,
        CancellationToken cancellationToken = default)
    {
        var source = Validators
            .Where(v => v is IDomainCustomValidator customValidator && customValidator.ActionName == actionName)
            .Cast<IValidator<TDomain>>();

        Validate(model, source, cancellationToken);
    }

    private static void Validate<TPayload>(
        TPayload model,
        IEnumerable<IValidator<TPayload>> source,
        CancellationToken cancellationToken = default)
        where TPayload : class
    {
        var results = Task.WhenAll(source.Select(async x => await x.ValidateAsync(model, cancellationToken))).Result;
        var errors = results.SelectMany(x => x.Errors).Where(x => x != null).ToImmutableList();

        if (!errors.IsEmpty)
            throw new ValidationException(errors);
    }
}