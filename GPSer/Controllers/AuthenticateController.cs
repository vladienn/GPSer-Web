using AutoMapper;
using GPSer.API.Data.UnitOfWork;
using GPSer.API.DTOs;
using GPSer.API.Models;
using GPSer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GPSer.API.Services;

namespace GPSer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserRepository userRepo;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public AuthenticateController(IUserRepository userRepo, IUserService userService, IMapper mapper)
        {
            this.userRepo = userRepo;
            this.userService = userService;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            var userExists = await userRepo.GetByUserName(userDTO.UserName);
            if (userExists != null)
            {
                return StatusCode(500, "User already exists!");
            }

            User user = mapper.Map<User>(userDTO);

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
                var token = userService.GenerateJWTToken(user);
                return Ok(token);
            }

            return Unauthorized();
        }
    }
}