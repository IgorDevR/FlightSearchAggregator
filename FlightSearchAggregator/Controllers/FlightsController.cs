using System.Reflection;
using FlightSearchAggregator.Dtos;
using FlightSearchAggregator.Helpers;
using FlightSearchAggregator.Models;
using FlightSearchAggregator.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(Summary = "Searches for flights",
        Description = "Retrieves a list of flights based on sorting parameters.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Flight search results returned successfully", typeof(IEnumerable<Flight>))]
    public async Task<IActionResult> SearchFlights([FromQuery] FlightSortParams sortParams)
    {
        _logger.LogInformationWithMethod($"FlightSortParams: {sortParams}");

        var flights = await _flightService.GetAggregatedFlights(sortParams);
        return Ok(flights);
    }

    [HttpGet("search")]
    [SwaggerOperation(Summary = "Searches for flights with filters",
                  Description = "Retrieves a list of flights based on search and sorting parameters.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Flight search results with filters returned successfully", typeof(IEnumerable<Flight>))]
    public async Task<IActionResult> SearchFlights([FromQuery] FlightSearchParams searchParams,
        [FromQuery] FlightSortParams sortParams)
    {
        _logger.LogInformationWithMethod($"FlightSearchParams: {searchParams},  FlightSortParams: {sortParams}");

        var flights = await _flightService.GetFlightsByFilters(searchParams, sortParams);
        return Ok(flights);
    }
}