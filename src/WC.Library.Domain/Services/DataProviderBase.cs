﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using WC.Library.Data.Models;
using WC.Library.Data.Repository;
using WC.Library.Data.Services;
using WC.Library.Domain.Models;
using WC.Library.Shared.Exceptions;

namespace WC.Library.Domain.Services;

public abstract class DataProviderBase<TProvider, TRepository, TDomain, TEntity> : IDataProvider<TDomain>
    where TProvider : IDataProvider<TDomain>
    where TRepository : IRepository<TEntity>
    where TDomain : class, IModel
    where TEntity : class, IEntity
{
    protected DataProviderBase(
        IMapper mapper,
        ILogger<TProvider> logger,
        TRepository repository)
    {
        Mapper = mapper;
        Logger = logger;
        Repository = repository;
    }

    protected IMapper Mapper { get; }

    protected TRepository Repository { get; }

    private ILogger<TProvider> Logger { get; }

    public virtual async Task<IEnumerable<TDomain>> Get(
        SieveModel? filter = default,
        bool withIncludes = false,
        IWcTransaction? transaction = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var res = await Repository.Get(filter, withIncludes, transaction, cancellationToken);

            return Mapper.Map<ICollection<TDomain>>(res);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error while receiving data: {Message}", ex.Message);
            throw;
        }
    }

    public virtual async Task<TDomain?> GetOneById(
        Guid id,
        bool withIncludes = false,
        IWcTransaction? transaction = default,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var res = await Repository.GetOneById(id, withIncludes, transaction, cancellationToken);

        if (res == null)
        {
            throw new NotFoundException($"{typeof(TDomain).Name} not found");
        }

        return Mapper.Map<TDomain>(res);
    }
}
