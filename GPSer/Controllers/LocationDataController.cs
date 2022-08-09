using AutoMapper;
using GPSer.API.Data.UnitOfWork;
using GPSer.API.DTOs;
using GPSer.API.Models;
using GPSer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GPSer.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class LocationDataController : ControllerBase
    {
        private readonly IRepository<Device> deviceRepo;
        private readonly IUserRepository userRepo;
        private readonly IRepository<LocationData> locationDataRepo;
        private readonly IMapper mapper;

        public LocationDataController(IRepository<Device> deviceRepo, IRepository<LocationData> locationDataRepo, IMapper mapper, IUserRepository userRepo)
        {
            this.deviceRepo = deviceRepo;
            this.locationDataRepo = locationDataRepo;
            this.mapper = mapper;
            this.userRepo = userRepo;
        }

        [HttpGet]
        public async Task<ActionResult<List<LocationDataDTO>>> GetLocations(Guid deviceId)
        {
            var device = await deviceRepo.GetByIdAsync(deviceId);

            if (device == null)
            {
                return StatusCode(500, "This device does not exist!");
            }

            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userRepo.GetByUserName(userName);

            if (device.UserId != user.Id)
            {
                return StatusCode(500, "This user is not the owner of this device!");
            }

            //TODO Change db data 
            var locationData = await locationDataRepo.ListAll().Where(x => x.DeviceId == device.Id).ToListAsync();

            return mapper.Map<List<LocationDataDTO>>(locationData);
        }
    }
}
