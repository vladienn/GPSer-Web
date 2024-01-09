using AutoMapper;
using GPSer.Core.Services;
using GPSer.Core.State;
using GPSer.Data.UnitOfWork;
using GPSer.Model;
using MediatR;

namespace GPSer.Core.Commands;

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

        deviceState.Items.Add(device.SerialNumber, new DeviceStateItem(device, DeviceStatus.Offline));

        return await deviceRepo.AddAsync(device);
    }
}