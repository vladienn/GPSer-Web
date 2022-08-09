using GPSer.Models;

namespace GPSer.API.DTOs;

public class LocationDataDTO
{
    public string Latitude { get; set; }

    public string Longitude { get; set; }

    public double Speed { get; set; }

    public Guid DeviceId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}