using GPSer.API.Data.UnitOfWork;
using GPSer.API.Services;
using GPSer.Models;
using MediatR;

namespace GPSer.API.Commands;

public class DeleteDeviceCommandHandler : IRequestHandler<DeleteDeviceCommand, bool>
{
    private readonly IRepository<Device> deviceRepo;
    private readonly IUserService userService;

    public DeleteDeviceCommandHandler(IRepository<Device> deviceRepo, IUserService userService)
    {
        this.deviceRepo = deviceRepo;
        this.userService = userService;
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

        return true;
    }
}
