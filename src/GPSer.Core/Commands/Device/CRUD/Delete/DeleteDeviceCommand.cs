using MediatR;

namespace GPSer.Core.Commands;

public class DeleteDeviceCommand : IRequest<bool>
{
    public Guid DeviceId { get; init; }
}
