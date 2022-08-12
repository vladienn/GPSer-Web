using AutoMapper;
using AutoMapper.QueryableExtensions;
using GPSer.API.Commands;
using GPSer.API.Data.UnitOfWork;
using GPSer.API.DTOs;
using GPSer.API.Models;
using GPSer.Data;
using GPSer.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GPSer.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IRepository<Device> deviceRepo;
        private readonly IUserRepository userRepo;
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public DeviceController(IRepository<Device> deviceRepo, IMapper mapper, IUserRepository userRepo, IMediator mediator)
        {
            this.deviceRepo = deviceRepo;
            this.mapper = mapper;
            this.userRepo = userRepo;
            this.mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<DeviceDTO>>> GetDevices()
        {
            var devices = await deviceRepo.ListAll().ToListAsync();

            return mapper.Map<List<DeviceDTO>>(devices);
        }
        
        [HttpGet("byUser")]
        public async Task<ActionResult<List<DeviceDTO>>> GetUserDevices()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userRepo.GetByUserName(userName);

            var devices = await deviceRepo.ListAll().Where(x => x.UserId == user.Id).ToListAsync();

            return mapper.Map<List<DeviceDTO>>(devices);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Device>>> GetDeviceById(Guid id)
        {
            var device = await deviceRepo.GetByIdAsync(id);

            return device == null ? NotFound() : Ok(device);
        }

        [HttpPost]
        public async Task<ActionResult<Device>> AddDevice(CreateDeviceCommand command)
        {
            var device = await mediator.Send(command);

            return CreatedAtAction(nameof(GetDeviceById), new { id = device.Id }, device);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDevice(Guid id, EditDeviceCommand command)
        {
            Device oldDevice = await deviceRepo.GetByIdAsync(id);
            if (oldDevice == null)
            {
                return NotFound();
            }

            command.Id = id;
            var result = await mediator.Send(command);

            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(Guid id)
        {
            var deviceToDelete = await deviceRepo.GetByIdAsync(id);
            if (deviceToDelete == null)
            { 
                return NotFound();
            }

            deviceRepo.Delete(deviceToDelete);

            return NoContent();
        }
    }
}
