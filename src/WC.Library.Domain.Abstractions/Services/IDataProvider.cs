using WC.Library.Domain.Models;

namespace WC.Library.Domain.Services;

public interface IDataProvider<TDomain> where TDomain : IModel
{
    Task<IEnumerable<TDomain>> Get(
        CancellationToken cancellationToken = default);

    Task<TDomain?> GetOneById(Guid id, CancellationToken cancellationToken = default);
}