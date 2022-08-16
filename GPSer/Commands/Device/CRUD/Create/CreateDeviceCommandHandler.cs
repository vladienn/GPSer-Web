using AutoMapper;
using GPSer.API.Data.UnitOfWork;
using GPSer.API.Services;
using GPSer.Models;
using MediatR;
using System.Security.Claims;

namespace GPSer.API.Commands;

public class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand, Device>
{
    private readonly IRepository<Device> deviceRepo;
    private readonly IMapper mapper;
    private readonly IUserService userService;


    public CreateDeviceCommandHandler(IRepository<Device> deviceRepo, IMapper mapper, IUserService userService)
    {
        this.deviceRepo = deviceRepo;
        this.mapper = mapper;
        this.userService = userService;
    }

    public async Task<Device> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.GetCurrentUserAsync();

        Device device = mapper.Map<Device>(request);
        device.UserId = user.Id;
        device.User = user;

        return await deviceRepo.AddAsync(device);
    }
}