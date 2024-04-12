namespace WC.Library.Domain.Services;

public interface IDataManager<in TDomain> where TDomain : class
{
    Task Create(TDomain model, CancellationToken cancellationToken = default);
    Task Update(TDomain model, CancellationToken cancellationToken = default);
    Task Delete(Guid id, CancellationToken cancellationToken = default);
}