using GPSer.API.Data.UnitOfWork;
using GPSer.API.Models;
using GPSer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GPSer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IRepository<User> userRepo;

        public AuthenticateController(IRepository<User> userRepo, IConfiguration config)
        {
            this.userRepo = userRepo;
            this.config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(User user)
        {
            await userRepo.AddAsync(user);

            return Ok("User created successfully!");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            var users = await userRepo.ListAllAsync();

            var user = users.First(x => x.UserName == userLogin.UserName && x.Password == userLogin.Password);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User Not Found!");
        }

        private string Generate(User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
            };

            var token = new JwtSecurityToken(issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: signinCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
