using WC.Library.Domain.Models;

namespace WC.Library.Domain.Services;

public interface IDataProvider<TDomain> where TDomain : IModel
{
    Task<IEnumerable<TDomain>> Get(bool withIncludes = false,
        CancellationToken cancellationToken = default);

    Task<TDomain?> GetOneById(Guid id, CancellationToken cancellationToken = default);
}