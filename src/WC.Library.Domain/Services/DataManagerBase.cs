using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using WC.Library.Data.Models;
using WC.Library.Data.Repository;
using WC.Library.Data.Services;
using WC.Library.Domain.Models;
using WC.Library.Domain.Services.Validators;
using WC.Library.Domain.Validators;

namespace WC.Library.Domain.Services;

public abstract class DataManagerBase<TManager, TRepository, TDomain, TEntity>
    : ValidatorBase<TDomain>,
        IDataManager<TDomain>
    where TManager : IDataManager<TDomain>
    where TRepository : IRepository<TEntity>
    where TDomain : class, IModel
    where TEntity : class, IEntity
{
    protected DataManagerBase(
        IMapper mapper,
        ILogger<TManager> logger,
        TRepository repository,
        IEnumerable<IValidator> validators)
        : base(validators)
    {
        Mapper = mapper;
        Logger = logger;
        Repository = repository;
    }

    protected IMapper Mapper { get; }

    protected TRepository Repository { get; }

    private ILogger<TManager> Logger { get; }

    public async Task<TDomain> Create(
        TDomain model,
        IWcTransaction? transaction,
        CancellationToken cancellationToken = default)
    {
        Validate<IDomainCreateValidator>(model, cancellationToken);
        return await CreateAction(model, transaction, cancellationToken);
    }

    public async Task<TDomain> Update(
        TDomain model,
        IWcTransaction? transaction,
        CancellationToken cancellationToken = default)
    {
        Validate<IDomainUpdateValidator>(model, cancellationToken);
        return await UpdateAction(model, transaction, cancellationToken);
    }

    public virtual async Task<TDomain> Delete(
        Guid id,
        IWcTransaction? transaction,
        CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetOneById(id, true, transaction, cancellationToken);

        Validate<IDomainDeleteValidator>(Mapper.Map<TDomain>(entity), cancellationToken);

        return await DeleteAction(id, transaction, cancellationToken);
    }

    protected virtual async Task<TDomain> CreateAction(
        TDomain model,
        IWcTransaction? transaction,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return Mapper.Map<TDomain>(await Repository.Create(Mapper.Map<TEntity>(model), transaction,
                cancellationToken));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error creating entity: {Message}", ex.Message);
            throw;
        }
    }

    protected virtual async Task<TDomain> UpdateAction(
        TDomain model,
        IWcTransaction? transaction,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return Mapper.Map<TDomain>(await Repository.Update(Mapper.Map<TEntity>(model), transaction,
                cancellationToken));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error updating model: {Message}", ex.Message);
            throw;
        }
    }

    protected virtual async Task<TDomain> DeleteAction(
        Guid id,
        IWcTransaction? transaction,
        CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(id);

            return Mapper.Map<TDomain>(await Repository.Delete(id, transaction, cancellationToken));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error when deleting an model: {Message}", ex.Message);
            throw;
        }
    }
}
