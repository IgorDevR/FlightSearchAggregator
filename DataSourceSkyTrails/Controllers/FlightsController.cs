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
            var flights = _flightService.GenerateFlights();
            return Ok(flights);
        }

        [HttpGet("{id}")]
        public IActionResult GetFlight(Guid id)
        {
            var flights = _flightService.GenerateFlight(id);
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
    }
}