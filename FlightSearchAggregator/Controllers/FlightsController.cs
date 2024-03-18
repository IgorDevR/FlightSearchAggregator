using System.Reflection;
using FlightSearchAggregator.Dtos;
using FlightSearchAggregator.Helpers;
using FlightSearchAggregator.Models;
using FlightSearchAggregator.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightSearchAggregator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly FlightService _flightService;
    private readonly ILogger _logger;

    public FlightsController(FlightService flightService, ILogger<FlightsController> logger)
    {
        _flightService = flightService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> SearchFlights([FromQuery] FlightSortParams sortParams)
    {
        _logger.LogInformation($"{MethodBase.GetCurrentMethod()?.Name}. FlightSortParams: {sortParams}");

        var flights = await _flightService.GetAggregatedFlights(sortParams);
        return Ok(flights);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchFlights([FromQuery] FlightSearchParams searchParams,
        [FromQuery] FlightSortParams sortParams)
    {
        _logger.LogInformation(
            $"{MethodBase.GetCurrentMethod()?.Name}. FlightSearchParams {searchParams}, FlightSortParams: {sortParams}");

        var flights = await _flightService.GetFlightsByFilters(searchParams, sortParams);
        return Ok(flights);
    }
}