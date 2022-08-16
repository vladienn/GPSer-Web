using MediatR;

namespace GPSer.API.Commands;

public class DeleteDeviceCommand : IRequest<bool>
{
    public Guid DeviceId { get; init; }
}
