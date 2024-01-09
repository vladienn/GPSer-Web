using System.ComponentModel.DataAnnotations;

namespace GPSer.Model;

public class Entity<T> : IEntity where T : struct
{
    public virtual T Id { get; set; }

    [Required]
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTimeOffset UpdatedAt { get; set; } = DateTime.UtcNow;
}