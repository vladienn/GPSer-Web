using GPSer.Model;

namespace GPSer.Data.UnitOfWork;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetById(int id);

    Task<User?> GetByUserName(string userName);

    Task<User?> GetByEmail(string email);
}
