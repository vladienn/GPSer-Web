using AutoMapper;
using GPSer.API.Commands;
using GPSer.API.DTOs;
using GPSer.API.Models;
using GPSer.Models;

namespace GPSer.API.Automapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Add as many of these lines as you need to map your objects
        CreateMap<Device, DeviceDTO>().ReverseMap();
        CreateMap<Device, CreateDeviceCommand>().ReverseMap();

        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<LocationData, LocationDataDTO>();
    }
}