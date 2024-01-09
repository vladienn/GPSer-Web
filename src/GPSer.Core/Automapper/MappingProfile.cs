using AutoMapper;
using GPSer.Core.Commands;
using GPSer.Core.DTOs;
using GPSer.Model;

namespace GPSer.Core.Automapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Add as many of these lines as you need to map your objects
        CreateMap<Device, DeviceDTO>().ReverseMap();
        CreateMap<Device, CreateDeviceCommand>().ReverseMap();

        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<LocationDataDTO, LocationData>().ReverseMap();
    }
}