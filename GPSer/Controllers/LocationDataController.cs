using AutoMapper;
using GPSer.API.Commands;
using GPSer.API.Data.UnitOfWork;
using GPSer.API.DTOs;
using GPSer.API.Models;
using GPSer.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GPSer.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class LocationDataController : ControllerBase
    {
        private readonly IRepository<Device> deviceRepo;
        private readonly IUserRepository userRepo;
        private readonly IRepository<LocationData> locationDataRepo;
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public LocationDataController(IRepository<Device> deviceRepo, IRepository<LocationData> locationDataRepo, IMapper mapper, IUserRepository userRepo, IMediator mediator)
        {
            this.deviceRepo = deviceRepo;
            this.locationDataRepo = locationDataRepo;
            this.mapper = mapper;
            this.userRepo = userRepo;
            this.mediator = mediator;
        }

        /// <summary>
        /// Returns all locationData by filter on current user
        /// </summary>
        /// <returns>List of Locations</returns>
        [HttpGet]
        public async Task<ActionResult<LocationDataSearchResultDTO>> GetLocations([FromQuery] SearchLocationDataCommand command)
        {
            var device = await deviceRepo.GetByIdAsync(command.DeviceId);

            if (device == null)
            {
                return StatusCode(500, "This device does not exist!");
            }

            var locationDataRecords = await mediator.Send(command);

            var searchResult = new LocationDataSearchResultDTO
            {
                Records = mapper.Map<List<LocationDataDTO>>(locationDataRecords)
            };

            return Ok(searchResult);
        }
    }
}