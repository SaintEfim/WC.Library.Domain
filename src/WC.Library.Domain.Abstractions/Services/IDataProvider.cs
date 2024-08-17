using WC.Library.Data.Services;
using WC.Library.Domain.Models;

namespace WC.Library.Domain.Services;

public interface IDataProvider<TDomain>
    where TDomain : IModel
{
    Task<IEnumerable<TDomain>> Get(
        bool withIncludes = false,
        IWcTransaction? transaction = default,
        CancellationToken cancellationToken = default);

    Task<TDomain?> GetOneById(
        Guid id,
        bool withIncludes = false,
        IWcTransaction? transaction = default,
        CancellationToken cancellationToken = default);
}
