using AutoMapper;
using GPSer.Core.Commands;
using GPSer.Core.DTOs;
using GPSer.Core.State;
using GPSer.Data.UnitOfWork;
using GPSer.Model;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GPSer.Controllers;

[Route("api/devices")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class DeviceController : ControllerBase
{
    private readonly IRepository<Device> deviceRepo;
    private readonly IUserRepository userRepo;
    private readonly IMapper mapper;
    private readonly IMediator mediator;
    private readonly IDeviceState deviceState;

    public DeviceController(IRepository<Device> deviceRepo, IMapper mapper, IUserRepository userRepo, IMediator mediator, IDeviceState deviceState)
    {
        this.deviceRepo = deviceRepo;
        this.mapper = mapper;
        this.userRepo = userRepo;
        this.mediator = mediator;
        this.deviceState = deviceState;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<DeviceDTO>>> GetAll()
    {
        var devices = await deviceRepo.ListAll().ToListAsync();

        var devicesDto = mapper.Map<List<DeviceDTO>>(devices);

        foreach (var device in deviceState.Items)
        {
            devicesDto.Find(x => x.SerialNumber == device.Key)!.Status = device.Value.Status;
        }

        return devicesDto;
    }

    [HttpGet("user")]
    public async Task<ActionResult<List<DeviceDTO>>> GetUserDevices()
    {
        var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userRepo.GetByUserName(userName);

        var devices = await deviceRepo.ListAll().Where(x => x.UserId == user.Id).ToListAsync();

        var devicesDto = mapper.Map<List<DeviceDTO>>(devices);

        foreach (var device in deviceState.Items)
        {
            devicesDto.Find(x => x.SerialNumber == device.Key)!.Status = device.Value.Status;
        }

        return devicesDto;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Device>>> GetById(Guid id)
    {
        var device = await deviceRepo.GetByIdAsync(id);

        return device == null ? NotFound() : Ok(device);
    }

    [HttpPost]
    public async Task<ActionResult<Device>> Create(CreateDeviceCommand command)
    {
        var device = await mediator.Send(command);

        return CreatedAtAction(nameof(GetDeviceById), new { id = device.Id }, device);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, EditDeviceCommand command)
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deviceToDelete = await deviceRepo.GetByIdAsync(id);
        if (deviceToDelete == null)
        {
            return NotFound();
        }

        var command = new DeleteDeviceCommand { DeviceId = id };
        var result = await mediator.Send(command);
        if (!result)
        {
            return BadRequest();
        }

        return NoContent();
    }
}