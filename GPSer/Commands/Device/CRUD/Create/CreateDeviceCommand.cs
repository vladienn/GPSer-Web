using GPSer.API.Models;
using GPSer.Models;
using MediatR;

namespace GPSer.API.Commands;

public class CreateDeviceCommand : IRequest<Device>
{
    public string Name { get; set; }

    public string SerialNumber { get; set; }
}
