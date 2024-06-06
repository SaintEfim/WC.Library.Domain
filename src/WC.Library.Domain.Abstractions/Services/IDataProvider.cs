namespace WC.Library.Domain.Services;

public interface IDataProvider<TDomain>
{
    Task<IEnumerable<TDomain>> Get(
        CancellationToken cancellationToken = default);

    Task<TDomain?> GetOneById(Guid id, CancellationToken cancellationToken = default);
}