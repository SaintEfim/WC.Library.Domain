using WC.Library.Data.Services;
using WC.Library.Domain.Models;

namespace WC.Library.Domain.Services;

public interface IDataManager<TDomain>
    where TDomain : IModel
{
    Task<TDomain> Create(
        TDomain model,
        IWcTransaction? transaction = default,
        CancellationToken cancellationToken = default);

    Task<TDomain> Update(
        TDomain model,
        IWcTransaction? transaction = default,
        CancellationToken cancellationToken = default);

    Task<TDomain> Delete(
        Guid id,
        IWcTransaction? transaction = default,
        CancellationToken cancellationToken = default);
}
