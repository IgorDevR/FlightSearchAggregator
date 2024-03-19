using DataSourceSkyTrails.Models;
using DataSourceSkyTrails.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataSourceSkyTrails.Controllers
{
    [Route("api/flights")]
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
        public IActionResult GetFlights()
        {
            _logger.LogInformationWithMethod("");
            var flights = _flightService.GetFlights();
            _logger.LogInformationWithMethod($"flights.Count: {flights.Count}");
            return Ok(flights);
        }

        [HttpGet("{id}")]
        public IActionResult GetFlight(Guid id)
        {
            _logger.LogInformationWithMethod("");
            var flight = _flightService.GetFlight(id);
            if (flight == null)
            {
                return NotFound($"Flight with Id: {id} not found");
            }
            _logger.LogInformationWithMethod($"flights id: {flight.Id}");
            return Ok(flight);
        }

        [HttpGet("search-by-filters")]
        public IActionResult SearchFlightsByFilters([FromQuery] FlightSearchParamsSkyTrails searchParams)
        {
            _logger.LogInformationWithMethod("");
            var flights = _flightService.GetFlightsByFilters(searchParams);
            _logger.LogInformationWithMethod($"flights.Count: {flights.Count}");
            return Ok(flights);
        }

        [HttpGet("book/{id}")]
        public async Task<IActionResult> GetBookingDetail(Guid id)
        {
            _logger.LogInformationWithMethod("");
            var bookingDetail = _flightService.GetBookingDetail(id);
            _logger.LogInformationWithMethod($"bookingDetail id: {bookingDetail.Id}");
            return Ok(bookingDetail);
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookFlight([FromBody] BookingRequest bookingRequest)
        {
            _logger.LogInformationWithMethod("");
            var flight = _flightService.GetFlight(bookingRequest.FlightId);
            if (flight == null)
                return NotFound($"Flight with id {bookingRequest.FlightId} not found");

            var bookingDetail = _flightService.Book(flight, bookingRequest);
            _logger.LogInformationWithMethod($"bookingDetail id: {bookingDetail.Id}");
            return Ok(bookingDetail);
        }
    }
}