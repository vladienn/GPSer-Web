namespace GPSer.Core.DTOs;

public class LocationDataDTO
{
    public string Latitude { get; set; } = null!;

    public string Longitude { get; set; } = null!;

    public double Speed { get; set; }

    public Guid DeviceId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}