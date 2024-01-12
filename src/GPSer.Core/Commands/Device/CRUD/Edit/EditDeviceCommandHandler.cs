using MediatR;

namespace GPSer.Core.Commands;

public class EditDeviceCommandHandler : IRequestHandler<EditDeviceCommand, bool>
{
    //private readonly IRepository<Device> deviceRepo;
    //private readonly IUserService userService;

    //public EditDeviceCommandHandler(IRepository<Device> deviceRepo, IUserService userService)
    //{
    //    this.deviceRepo = deviceRepo;
    //    this.userService = userService;
    //}

    //public async Task<bool> Handle(EditDeviceCommand request, CancellationToken cancellationToken)
    //{
    //    var user = await userService.GetCurrentUserAsync();

    //    var device = await deviceRepo.GetByIdAsync(request.Id);

    //    if (device.UserId != user.Id)
    //    {
    //        //TODO create custom exceptions
    //        throw new Exception("This device doesnt belong to the user!");
    //    }

    //    device.Name = request.Name;

    //    device.UpdatedAt = DateTime.UtcNow;

    //    await deviceRepo.UpdateAsync(device);

    //    return true;
    //}
    public Task<bool> Handle(EditDeviceCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
