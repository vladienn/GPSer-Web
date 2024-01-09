using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPSer.Model;

public class LocationData : LocationEntity<Guid>
{
    [Required]
    public string Latitude { get; set; }

    [Required]
    public string Longitude { get; set; }

    [Required]
    public double Speed { get; set; }

    [Required]
    public Guid DeviceId { get; set; }
    [ForeignKey("DeviceId")]
    public Device Device { get; set; }
}