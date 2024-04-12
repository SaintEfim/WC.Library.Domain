using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using WC.Library.Data.Repository;

namespace WC.Library.Domain.Services;

public abstract class DataManagerBase<TManager, TRepository, TDomain, TEntity> : IDataManager<TDomain>
    where TManager : IDataManager<TDomain>
    where TRepository : IRepository<TEntity>
    where TDomain : class
{
    protected DataManagerBase(
        IMapper mapper,
        ILogger<TManager> logger,
        TRepository repository,
        IEnumerable<IValidator> validators)
    {
        Mapper = mapper;
        Logger = logger;
        Repository = repository;
        Validators = validators;
    }

    protected IMapper Mapper { get; }

    protected TRepository Repository { get; }

    private ILogger<TManager> Logger { get; }

    protected IEnumerable<IValidator> Validators { get; }

    public async Task Create(TDomain model, CancellationToken cancellationToken = default)
    {
        Validate(model);
        await CreateAction(model, cancellationToken);
    }

    public async Task Update(TDomain model, CancellationToken cancellationToken = default)
    {
        Validate(model);
        await UpdateAction(model, cancellationToken);
    }

    public virtual async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(id);

            await Repository.Delete(id, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error when deleting an model: {Message}", ex.Message);
            throw;
        }
    }

    protected virtual async Task CreateAction(
        TDomain model,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await Repository.Create(Mapper.Map<TEntity>(model), cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error creating entity: {Message}", ex.Message);
            throw;
        }
    }

    protected virtual async Task UpdateAction(
        TDomain model,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await Repository.Update(Mapper.Map<TEntity>(model), cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error updating model: {Message}", ex.Message);
            throw;
        }
    }

    protected void Validate<T>(T model)
    {
        var errors = Validators.OfType<IValidator<T>>().Select(validator => validator.Validate(model))
            .SelectMany(result => result.Errors).ToList();

        if (errors.Count > 0)
        {
            throw new ValidationException(errors);
        }
    }
}