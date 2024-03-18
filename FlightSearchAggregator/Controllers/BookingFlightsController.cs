using FlightSearchAggregator.Dtos;
using FlightSearchAggregator.Helpers;
using FlightSearchAggregator.Models;
using FlightSearchAggregator.Services.Bookings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace FlightSearchAggregator.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BookingFlightsController : ControllerBase
{
    private readonly BookFlightService _bookFlightService;
    private readonly ILogger _logger;

    public BookingFlightsController(BookFlightService bookFlightService, ILogger<FlightsController> logger)
    {
        _bookFlightService = bookFlightService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Retrieves booking details by ID",
        Description = "Provides detailed information about a booking, including passenger details and flight information.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Booking details retrieved successfully", typeof(BookingDetailDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Booking with the specified ID was not found")]
    public async Task<IActionResult> GetBookingDetail(Guid id)
    {
        _logger.LogInformationWithMethod($"Guid: {id}");
        var bookingDetail = await _bookFlightService.GetBookingDetail(id);
        if (bookingDetail == null)
        {
            return NotFound($"Booking with id: {id} not found");
        }

        return Ok(bookingDetail);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new flight booking",
        Description = "Submits a booking request for a flight. The request must include passenger details and flight information.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Booking created successfully", typeof(BookingDetailDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Flight data provider not found")]
    [SwaggerRequestExample(typeof(BookingRequest), typeof(BookingRequestExample))]
    public async Task<IActionResult> BookFlight([FromBody] BookingRequest bookingRequest)
    {
        _logger.LogInformationWithMethod($"BookingRequest: {bookingRequest}");

        var dataProvider = EnumExtensions.ParseEnum(bookingRequest.DataProvider, FlightDataProvider.Unknown);
        if (dataProvider == FlightDataProvider.Unknown)
        {
            return NotFound($"Flight data provider with name: {bookingRequest.DataProvider} not found");
        }

        var bookingDetail = await _bookFlightService.BookFlight(bookingRequest, dataProvider);

        return Ok(bookingDetail);
    }
}