using FlightSearchAggregator.Dtos;
using FlightSearchAggregator.Helpers;
using FlightSearchAggregator.Models;
using FlightSearchAggregator.Services.Bookings;
using Microsoft.AspNetCore.Mvc;

namespace FlightSearchAggregator.Controllers;

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