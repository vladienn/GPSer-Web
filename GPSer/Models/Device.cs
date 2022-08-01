using GPSer.API.Models;
using System.ComponentModel.DataAnnotations;

namespace GPSer.Models
{
    public class Device : Entity<Guid>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string SerialNumber { get; set; }
    }
}
