using AutoMapper;
using GPSer.API.Data.UnitOfWork;
using GPSer.API.Services;
using GPSer.API.State;
using GPSer.Models;
using MediatR;
using System.Security.Claims;

namespace GPSer.API.Commands;

public class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand, Device>
{
    private readonly IRepository<Device> deviceRepo;
    private readonly IMapper mapper;
    private readonly IUserService userService;
    private readonly IDeviceState deviceState;


    public CreateDeviceCommandHandler(IRepository<Device> deviceRepo, IMapper mapper, IUserService userService, IDeviceState deviceState)
    {
        this.deviceRepo = deviceRepo;
        this.mapper = mapper;
        this.userService = userService;
        this.deviceState = deviceState;
    }

    public async Task<Device> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.GetCurrentUserAsync();

        Device device = mapper.Map<Device>(request);
        device.UserId = user.Id;
        device.User = user;

        deviceState.Items.Add(device.SerialNumber, new DeviceStateItem (device, DeviceStatus.Offline));

        return await deviceRepo.AddAsync(device);
    }
}