using GPSer.Model;

namespace GPSer.Core.Services;

public interface IUserService
{
    Task<User> GetCurrentUserAsync();
    string GenerateJWTToken(User user);
}