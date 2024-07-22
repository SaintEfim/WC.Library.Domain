using WC.Library.Domain.Models;

namespace WC.Library.Domain.Services;

public interface IDataManager<TDomain>
    where TDomain : IModel
{
    Task<TDomain> Create(
        TDomain model,
        CancellationToken cancellationToken = default);

    Task<TDomain> Update(
        TDomain model,
        CancellationToken cancellationToken = default);

    Task<TDomain> Delete(
        Guid id,
        CancellationToken cancellationToken = default);
}
