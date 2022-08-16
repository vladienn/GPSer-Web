using GPSer.API.Data.UnitOfWork;
using GPSer.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GPSer.API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository userRepo;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IConfiguration config;

    public UserService(IUserRepository userRepo, IHttpContextAccessor httpContextAccessor, IConfiguration config)
    {
        this.userRepo = userRepo;
        this.httpContextAccessor = httpContextAccessor;
        this.config = config;
    }
    public async Task<User> GetCurrentUserAsync()
    {
        var CurrentUserName = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (CurrentUserName == null)
        {
            throw new ArgumentNullException();
        }

        var user = await userRepo.GetByUserName(CurrentUserName);

        if (user == null)
        {
            throw new ArgumentNullException();
        }

        return user;
    }
    public string GenerateJWTToken(User user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

        var token = new JwtSecurityToken(issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(5),
            signingCredentials: signinCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}