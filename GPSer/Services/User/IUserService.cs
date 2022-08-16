using GPSer.API.Models;

namespace GPSer.API.Services;

public interface IUserService
{
    Task<User> GetCurrentUserAsync();
    string GenerateJWTToken(User user);
}