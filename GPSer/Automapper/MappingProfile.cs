using AutoMapper;
using GPSer.API.DTOs;
using GPSer.API.DTOs.User;
using GPSer.API.Models;
using GPSer.Models;

namespace GPSer.API.Automapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Add as many of these lines as you need to map your objects
        CreateMap<Device, DeviceDTO>().ReverseMap();
        CreateMap<User, UserDTO>().ReverseMap();
    }
}

