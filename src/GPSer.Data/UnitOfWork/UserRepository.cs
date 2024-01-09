using GPSer.Model;
using Microsoft.EntityFrameworkCore;

namespace GPSer.Data.UnitOfWork;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly GPSerDbContext dbContext;

    public UserRepository(GPSerDbContext dbContext) : base(dbContext, null)
    {
        this.dbContext = dbContext;
    }

    async public Task<User> GetById(int id) => await dbContext.Set<User>().FindAsync(id);

    async public Task<User> GetByUserName(string userName) => await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);

    async public Task<User> GetByEmail(string email) => await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
}
