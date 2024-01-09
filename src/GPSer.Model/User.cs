using System.ComponentModel.DataAnnotations;

namespace GPSer.Model;

public class User : Entity<Guid>
{
    [Required]
    public string? UserName { get; set; }

    [Required]
    public string? Password { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public Roles Role { get; set; }
}