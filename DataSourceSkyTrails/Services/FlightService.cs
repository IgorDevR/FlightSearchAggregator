using DataSourceSkyTrails.Models;

namespace DataSourceSkyTrails.Services;

public class FlightService
{
    private static readonly Random _random = new Random();

    private static readonly Dictionary<string, string> _citiesAndCodes = new Dictionary<string, string>
    {
        { "New York", "JFK" },
        { "London", "LHR" },
        { "Seoul", "ICN" },
        { "Amsterdam", "AMS" },
        { "Tokyo", "HND" },
        { "Dubai", "DXB" },
        { "Istanbul", "IST" },
        { "Bangkok", "BKK" },
    };

    private static readonly List<string> _airlines = new List<string>
    {
        "American Airlines",
        "Delta Air Lines",
        "United Airlines",
        "Southwest Airlines",
        "Air France",
        "Lufthansa",
        "British Airways",
        "Qantas Airways",
        "Emirates",
        "Cathay Pacific",
        "Singapore Airlines",
        "All Nippon Airways"
    };

    public readonly List<FlightSkyTrails> FlightSkyTrails = Generate();

    public List<FlightSkyTrails> GenerateFlights()
    {
        return FlightSkyTrails;
    }

    public List<FlightSkyTrails> GetFlightsByFilters(FlightSearchParamsSkyTrails searchParams)
    {
        var flights = FlightSkyTrails;

        return flights
            .Where(f => (string.IsNullOrEmpty(searchParams.Airline) || f.Airline == searchParams.Airline))
            .Where(f => (!searchParams.Depart.HasValue ||
                         f.Depart.Date == searchParams.Depart.Value.Date))
            .Where(f => (!searchParams.MaxPrice.HasValue || f.Cost <= searchParams.MaxPrice.Value))
            .Where(f => (!searchParams.Refundable || f.Refundable == searchParams.Refundable))
            .ToList();
    }

    public FlightSkyTrails? GenerateFlight(Guid id)
    {
        return FlightSkyTrails.FirstOrDefault(f => f.Id == id);
    }

    private static List<FlightSkyTrails> Generate()
    {
        var flights = new List<FlightSkyTrails>();
        List<string> cityKeys = _citiesAndCodes.Keys.ToList();

        for (int i = 0; i < 10; i++)
        {
            var originCity = cityKeys[_random.Next(cityKeys.Count)];
            var destinationCity = cityKeys[_random.Next(cityKeys.Count)];

            while (destinationCity == originCity)
            {
                destinationCity = cityKeys[_random.Next(cityKeys.Count)];
            }

            flights.Add(new FlightSkyTrails
            {
                Id = new Guid(),
                Airline = _airlines[_random.Next(_airlines.Count)],
                FlightCode = $"A{_random.Next(100, 999)}",
                FromCity = originCity,
                ToCity = destinationCity,
                AirportsCode = $"{_citiesAndCodes[originCity]}-{_citiesAndCodes[destinationCity]}",
                Depart = DateTime.Now.AddHours(_random.Next(1, 100)),
                Arrive = DateTime.Now.AddHours(_random.Next(101, 200)),
                Cost = _random.Next(100, 500),
                Layovers = _random.Next(0, 4),
                Refundable = _random.Next(2) == 0
            });
        }

        return flights;
    }
}