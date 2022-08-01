namespace GPSer.API.Data.UnitOfWork;
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}