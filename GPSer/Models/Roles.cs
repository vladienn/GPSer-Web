using System.Runtime.Serialization;

namespace GPSer.API.Models;

public enum Roles
{
    [EnumMember(Value = "Admin")]
    Admin,
    [EnumMember(Value = "User")]
    User
}

