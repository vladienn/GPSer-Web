using GPSer.API.State;

namespace GPSer.API.DTOs;

public class DeviceDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string SerialNumber { get; set; } = null!;

    public DeviceStatus Status { get; set; }

    public Guid UserId { get; set; }
}
