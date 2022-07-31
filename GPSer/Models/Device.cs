using System.ComponentModel.DataAnnotations;

namespace GPSer.Models
{
    public class Device
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string SerialNumber { get; set; }
    }
}
