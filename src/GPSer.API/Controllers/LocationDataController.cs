﻿using AutoMapper;
using GPSer.Core.Commands;
using GPSer.Core.DTOs;
using GPSer.Data.UnitOfWork;
using GPSer.Model;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GPSer.Controllers;

[Route("api/location")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class LocationDataController : ControllerBase
{
    private readonly IRepository<Device> deviceRepo;
    private readonly IRepository<LocationData> locationDataRepo;
    private readonly IMapper mapper;
    private readonly IMediator mediator;

    public LocationDataController(IRepository<Device> deviceRepo, IRepository<LocationData> locationDataRepo, IMapper mapper, IMediator mediator)
    {
        this.deviceRepo = deviceRepo;
        this.locationDataRepo = locationDataRepo;
        this.mapper = mapper;
        this.mediator = mediator;
    }

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