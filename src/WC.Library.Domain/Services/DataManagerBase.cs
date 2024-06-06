using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using WC.Library.Data.Models;
using WC.Library.Data.Repository;
using WC.Library.Domain.Models;

namespace WC.Library.Domain.Services;

public abstract class DataManagerBase<TManager, TRepository, TDomain, TEntity> : IDataManager<TDomain>
    where TManager : IDataManager<TDomain>
    where TRepository : IRepository<TEntity>
    where TDomain : IModel
    where TEntity : class, IEntity
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

    public async Task<TDomain> Create(TDomain model, CancellationToken cancellationToken = default)
    {
        Validate(model);
        return await CreateAction(model, cancellationToken);
    }

    public async Task<TDomain> Update(TDomain model, CancellationToken cancellationToken = default)
    {
        Validate(model);
        return await UpdateAction(model, cancellationToken);
    }

    public virtual async Task<TDomain> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        return await DeleteAction(id, cancellationToken);
    }

    protected virtual async Task<TDomain> CreateAction(
        TDomain model,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return Mapper.Map<TDomain>(await Repository.Create(Mapper.Map<TEntity>(model), cancellationToken));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error creating entity: {Message}", ex.Message);
            throw;
        }
    }

    protected virtual async Task<TDomain> UpdateAction(
        TDomain model,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return Mapper.Map<TDomain>(await Repository.Update(Mapper.Map<TEntity>(model), cancellationToken));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error updating model: {Message}", ex.Message);
            throw;
        }
    }

    protected virtual async Task<TDomain> DeleteAction(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(id);

            return Mapper.Map<TDomain>(await Repository.Delete(id, cancellationToken));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error when deleting an model: {Message}", ex.Message);
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