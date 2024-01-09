using GPSer.Model;
using MediatR;

namespace GPSer.Core.Commands;

public class SearchLocationDataCommand : IRequest<List<LocationData>>
{
    public Guid DeviceId { get; set; }
    public DateTime? From { get; set; }

    public DateTime? To { get; set; }

    public ushort Limit { get; set; } = 100;
}
