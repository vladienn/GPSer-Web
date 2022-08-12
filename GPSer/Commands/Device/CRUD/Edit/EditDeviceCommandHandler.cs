using AutoMapper;
using GPSer.API.Data.UnitOfWork;
using GPSer.Models;
using MediatR;
using System.Security.Claims;

namespace GPSer.API.Commands;

public class EditDeviceCommandHandler : IRequestHandler<EditDeviceCommand, bool>
{
    private readonly IRepository<Device> deviceRepo;
    private readonly IUserRepository userRepo;
    private readonly IHttpContextAccessor httpContextAccessor;

    public EditDeviceCommandHandler(IRepository<Device> deviceRepo, IUserRepository userRepo, IHttpContextAccessor httpContextAccessor)
    {
        this.deviceRepo = deviceRepo;
        this.userRepo = userRepo;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(EditDeviceCommand request, CancellationToken cancellationToken)
    {
        //TODO make getting current user a method?
        var CurrentUserName = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (CurrentUserName == null)
        {
            throw new ArgumentNullException();
        }

        var user = await userRepo.GetByUserName(CurrentUserName);

        if (user == null)
        {
            throw new ArgumentNullException();
        }

        var device = await deviceRepo.GetByIdAsync(request.Id);

        if (device.UserId != user.Id)
        {
            //TODO create custom exceptions
            throw new Exception("This device doesnt belong to the user!");
        }

        device.Name = request.Name;

        device.UpdatedAt = DateTime.UtcNow;

        await deviceRepo.UpdateAsync(device);

        return true;
    }
}
