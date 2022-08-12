using AutoMapper;
using GPSer.API.Data.UnitOfWork;
using GPSer.Models;
using MediatR;
using System.Security.Claims;

namespace GPSer.API.Commands;

public class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand, Device>
{
    private readonly IRepository<Device> deviceRepo;
    private readonly IUserRepository userRepo;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IMapper mapper;

    public CreateDeviceCommandHandler(IRepository<Device> deviceRepo, IUserRepository userRepo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        this.deviceRepo = deviceRepo;
        this.userRepo = userRepo;
        this.mapper = mapper;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<Device> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
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

        Device device = mapper.Map<Device>(request);
        device.UserId = user.Id;
        device.User = user;

        return await deviceRepo.AddAsync(device);
    }
}