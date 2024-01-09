using MediatR;
using System.Text.Json.Serialization;

namespace GPSer.Core.Commands;

public class EditDeviceCommand : IRequest<bool>
{
    [JsonIgnore]
    public Guid Id { get; set; }

    public string Name { get; set; }
}
