namespace Library.Domain.Services;

public interface IDataProvider<TDomain>
{
    Task<ICollection<TDomain>> Get(
        CancellationToken cancellationToken = default);

    Task<TDomain?> GetOneById(Guid id, CancellationToken cancellationToken = default);
}