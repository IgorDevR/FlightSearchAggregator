using DataSourceSkyTrails.Models;
using DataSourceSkyTrails.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataSourceSkyTrails.Controllers
{
    [Route("api/{controler}")]
    public class FlightsController : ControllerBase
    {
        private readonly FlightService _flightService;

        public FlightsController(FlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        public IActionResult GetFlights()
        {
            var flights = _flightService.GetFlights();
            return Ok(flights);
        }

        [HttpGet("{id}")]
        public IActionResult GetFlight(Guid id)
        {
            var flights = _flightService.GetFlight(id);
            if (flights == null)
            {
                return NotFound($"Flight with Id: {id} not found");
            }

            return Ok(flights);
        }

        [HttpGet("search-by-filters")]
        public IActionResult SearchFlightsByFilters([FromQuery] FlightSearchParamsSkyTrails searchParams)
        {
            var flights = _flightService.GetFlightsByFilters(searchParams);
            return Ok(flights);
        }

        [HttpGet("book/{id}")]
        public async Task<IActionResult> GetBookingDetail(Guid id)
        {
            var bookingDetail = _flightService.GetBookingDetail(id);
            return Ok(bookingDetail);
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookFlight([FromBody] BookingRequest bookingRequest)
        {
            var flight = _flightService.GetFlight(bookingRequest.FlightId);
            if (flight == null)
                return NotFound($"Flight with id {bookingRequest.FlightId} not found");

            var bookingDetail = _flightService.Book(flight, bookingRequest);
            return Ok(bookingDetail);
        }
    }
}