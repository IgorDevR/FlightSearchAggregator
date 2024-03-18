using DataSourceFlyQuest.Models;
using DataSourceFlyQuest.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataSourceFlyQuest.Controllers
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
            var flights = _flightService.GenerateFlight(id);
            if (flights == null)
            {
                return NotFound($"Flight with Id: {id} not found");
            }

            return Ok(flights);
        }

        [HttpGet("search")]
        public IActionResult SearchFlights([FromQuery] FlightSearchParamsFlyQuest searchParams)
        {
            var flights = _flightService.GetFlightsByFilters(searchParams);
            return Ok(flights);
        }
    }
}