using GPSer.Data;
using GPSer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GPSer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly ILogger<DeviceController> _logger;
        private readonly GPSerDbContext _context;
        public DeviceController(ILogger<DeviceController> logger, GPSerDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevices()
        {
            return await _context.Devices.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Device>>> GetDeviceById(int id)
        {
            var device = await _context.Devices.FindAsync(id);

            return device == null ? NotFound() : Ok(device);
        }

        [HttpPost]
        public async Task<ActionResult<Device>> AddDevice(Device device)
        {
            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDeviceById), new { id = device.Id }, device);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDevice(int id, Device device)
        {
            if (id == device.Id)
            {
                return BadRequest();
            }

            _context.Entry(device).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var deviceToDelete = await _context.Devices.FindAsync(id);
            if (deviceToDelete == null)
            { 
                return NotFound();
            }

            _context.Devices.Remove(deviceToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
