using GPSer.Core.Services;
using GPSer.Core.State;
using GPSer.Data.UnitOfWork;
using GPSer.Model;
using MediatR;

namespace GPSer.Core.Commands;

public class DeleteDeviceCommandHandler : IRequestHandler<DeleteDeviceCommand, bool>
{
    private readonly IRepository<Device> deviceRepo;
    private readonly IUserService userService;
    private readonly IDeviceState deviceState;

    public DeleteDeviceCommandHandler(IRepository<Device> deviceRepo, IUserService userService, IDeviceState deviceState)
    {
        this.deviceRepo = deviceRepo;
        this.userService = userService;
        this.deviceState = deviceState;
    }

    public async Task<bool> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.GetCurrentUserAsync();

        var device = await deviceRepo.GetByIdAsync(request.DeviceId);

        if (device.UserId != user.Id)
        {
            //TODO create custom exceptions
            throw new Exception("This device doesnt belong to the user!");
        }

        await deviceRepo.DeleteAsync(device);

        deviceState.Items.Remove(device.SerialNumber);

        return true;
    }
}
