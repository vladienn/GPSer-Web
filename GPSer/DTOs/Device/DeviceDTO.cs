using GPSer.API.DTOs;

namespace GPSer.API.DTOs;

public class DeviceDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string SerialNumber { get; set; }

    public Guid UserId { get; set; }
}
