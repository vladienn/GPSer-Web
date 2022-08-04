using GPSer.API.DTOs.User;

namespace GPSer.API.DTOs;

public class DeviceDTO
{
    public string Name { get; set; }

    public string SerialNumber { get; set; }

    public Guid UserId { get; set; }
}
