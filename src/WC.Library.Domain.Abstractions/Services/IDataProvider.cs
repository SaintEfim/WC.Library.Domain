using WC.Library.Data.Services;
using WC.Library.Domain.Models;

namespace WC.Library.Domain.Services;

public interface IDataProvider<TDomain>
    where TDomain : IModel
{
    Task<IEnumerable<TDomain>> Get(
        IWcTransaction? transaction,
        bool withIncludes = false,
        CancellationToken cancellationToken = default);

    Task<TDomain?> GetOneById(
        Guid id,
        IWcTransaction? transaction,
        bool withIncludes = false,
        CancellationToken cancellationToken = default);
}
