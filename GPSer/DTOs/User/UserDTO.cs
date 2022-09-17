using GPSer.API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GPSer.API.DTOs;

public class UserDTO
{
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    [JsonConverter(typeof(StringEnumConverter))]
    public Roles Role { get; set; }
}