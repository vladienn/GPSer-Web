﻿using System.ComponentModel.DataAnnotations;

namespace GPSer.Model;

public class Device : Entity<Guid>
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string SerialNumber { get; set; }

    //[Required]
    //public Guid UserId { get; set; }
    //[ForeignKey("UserId")]
    //public IdentityUser? User { get; set; }
}