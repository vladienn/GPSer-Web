using GPSer.Model;
using MediatR;

namespace GPSer.Core.Commands;

public class SearchLocationDataCommandHandler : IRequestHandler<SearchLocationDataCommand, List<LocationData>>
{
    //private readonly IRepository<LocationData> locationDataRepo;
    //private readonly IRepository<Device> deviceRepo;

    //public SearchLocationDataCommandHandler(IRepository<LocationData> locationDataRepo, IRepository<Device> deviceRepo)
    //{
    //    this.locationDataRepo = locationDataRepo;
    //    this.deviceRepo = deviceRepo;
    //}

    //public async Task<List<LocationData>> Handle(SearchLocationDataCommand request, CancellationToken cancellationToken)
    //{
    //    //TODO maybe make checking ownership a service
    //    var user = await userService.GetCurrentUserAsync();

    //    var device = await deviceRepo.GetByIdAsync(request.DeviceId);

    //    if (device.UserId != user.Id)
    //    {
    //        //TODO create custom exceptions
    //        throw new Exception("This device doesnt belong to the user!");
    //    }

    //    var result = await locationDataRepo.ListAsync(new FilterLocationDatasSpec(request.DeviceId,
    //        request.From, request.To));

    //    return result.ToList();
    //}
    public Task<List<LocationData>> Handle(SearchLocationDataCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}