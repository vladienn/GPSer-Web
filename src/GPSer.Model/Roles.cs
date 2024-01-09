using System.Runtime.Serialization;

namespace GPSer.Model;

public enum Roles
{
    [EnumMember(Value = "Admin")]
    Admin = 1,
    [EnumMember(Value = "User")]
    User = 2
}