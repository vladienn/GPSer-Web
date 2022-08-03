using GPSer.API.Data.UnitOfWork;
using GPSer.Data;
using GPSer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GPSer.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IRepository<Device> deviceRepo;

        public DeviceController(IRepository<Device> deviceRepo)
        {
            this.deviceRepo = deviceRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            return await deviceRepo.ListAll().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Device>>> GetDeviceById(Guid id)
        {
            var device = await deviceRepo.GetByIdAsync(id);

            return device == null ? NotFound() : Ok(device);
        }

        [HttpPost]
        public async Task<ActionResult<Device>> AddDevice(Device device)
        {
            await deviceRepo.AddAsync(device);

            return CreatedAtAction(nameof(GetDeviceById), new { id = device.Id }, device);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDevice(Guid id, Device device)
        {
            if (id == device.Id)
            {
                return BadRequest();
            }

            deviceRepo.Update(device);

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
