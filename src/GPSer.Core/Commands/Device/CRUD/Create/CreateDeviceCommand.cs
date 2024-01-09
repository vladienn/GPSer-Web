using GPSer.Model;
using MediatR;

namespace GPSer.Core.Commands;

public class CreateDeviceCommand : IRequest<Device>
{
    public string Name { get; set; }

    public string SerialNumber { get; set; }
}
