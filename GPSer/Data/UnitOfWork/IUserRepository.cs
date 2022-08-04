using GPSer.API.Models;

namespace GPSer.API.Data.UnitOfWork
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetById(int id);

        Task<User> GetByUserName(string userName);

        Task<User> GetByEmail(string email);
    }
}
