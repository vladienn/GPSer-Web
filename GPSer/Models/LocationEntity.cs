using System.ComponentModel.DataAnnotations;

namespace GPSer.API.Models;

public class LocationEntity<T> : IEntity where T : struct
{
    public virtual T Id { get; set; }

    [Required]
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
}