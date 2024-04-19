using AutoMapper;
using Microsoft.Extensions.Logging;
using WC.Library.Data.Repository;
using WC.Library.Shared.Exceptions;

namespace WC.Library.Domain.Services;

public abstract class DataProviderBase<TProvider, TRepository, TDomain, TEntity> : IDataProvider<TDomain>
    where TProvider : IDataProvider<TDomain>
    where TRepository : IRepository<TEntity>
    where TDomain : class
    where TEntity : class
{
    protected DataProviderBase(
        IMapper mapper,
        ILogger<TProvider> logger,
        TRepository repository
    )
    {
        Mapper = mapper;
        Logger = logger;
        Repository = repository;
    }

    protected IMapper Mapper { get; }

    protected TRepository Repository { get; }

    private ILogger<TProvider> Logger { get; }

    public virtual async Task<ICollection<TDomain>> Get(CancellationToken cancellationToken = default)
    {
        try
        {
            var res = await Repository.Get(cancellationToken);

            return Mapper.Map<ICollection<TDomain>>(res);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while receiving data: {Message}", ex.Message);
            throw;
        }
    }

    public virtual async Task<TDomain?> GetOneById(Guid id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);
        try
        {
            var res = await Repository.GetOneById(id, cancellationToken);

            if (res == null)
            {
                throw new NotFoundException($"User with id {id} not found.");
            }

            return Mapper.Map<TDomain>(res);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error when retrieving data by ID: {Message}", ex.Message);
            throw;
        }
    }
}