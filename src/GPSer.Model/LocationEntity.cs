using System.ComponentModel.DataAnnotations;

namespace GPSer.Model;

public class LocationEntity<T> : IEntity where T : struct
{
    public virtual T Id { get; set; }

    [Required]
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
}