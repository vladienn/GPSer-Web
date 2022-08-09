using GPSer.API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GPSer.API.DTOs;

public class UserDTO
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public Roles Role { get; set; }
}