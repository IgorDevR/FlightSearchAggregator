using DataSourceFlyQuest.Models;

namespace DataSourceFlyQuest.Services;

public class FlightService
{
    private static readonly Random _random = new Random();

    private static readonly List<string> _aircraftTypes = new List<string>
        { "Boeing 737", "Airbus A320", "Boeing 777", "Airbus A380" };

    private static readonly Dictionary<string, string> _citiesAndCodes = new Dictionary<string, string>
    {
        { "New York", "JFK" },
        { "London", "LHR" },
        { "Paris", "CDG" },
        { "Tokyo", "HND" },
        { "Sydney", "SYD" },
        { "Los Angeles", "LAX" },
        { "Berlin", "BER" },
        { "Moscow", "SVO" }
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

    private readonly List<FlightFlyQuest> FlightFlyQuests = Generate();

    public List<FlightFlyQuest> GetFlights()
    {
        return FlightFlyQuests;
    }

    public List<FlightFlyQuest> GetFlightsByFilters(FlightSearchParamsFlyQuest searchParams)
    {
        var flights = Generate();

        return flights
            .Where(f => (string.IsNullOrEmpty(searchParams.Airline) || f.Airline == searchParams.Airline))
            .Where(f => (!searchParams.DepartureTime.HasValue ||
                         f.DepartureTime.Date == searchParams.DepartureTime.Value.Date))
            .Where(f => (!searchParams.MaxPrice.HasValue || f.Price <= searchParams.MaxPrice.Value))
            .Where(f => (!searchParams.WifiAvailable || f.WifiAvailable == searchParams.WifiAvailable))
            .ToList();
    }

    public FlightFlyQuest? GenerateFlight(Guid id)
    {
        return FlightFlyQuests.FirstOrDefault(f => f.Id == id);
    }

    private static List<FlightFlyQuest> Generate()
    {
        var flights = new List<FlightFlyQuest>();

        List<string> cityKeys = _citiesAndCodes.Keys.ToList();
        for (int i = 0; i < 10; i++)
        {
            var originCity = cityKeys[_random.Next(cityKeys.Count)];
            var destinationCity = cityKeys[_random.Next(cityKeys.Count)];

            while (destinationCity == originCity)
            {
                destinationCity = cityKeys[_random.Next(cityKeys.Count)];
            }

            flights.Add(new FlightFlyQuest
            {
                Airline = _airlines[_random.Next(_airlines.Count)],
                FlightNumber = $"A{_random.Next(100, 999)}",
                Origin = originCity,
                Destination = destinationCity,
                OriginAirportCode = _citiesAndCodes[originCity],
                DestinationAirportCode = _citiesAndCodes[destinationCity],
                DepartureTime = DateTime.Now.AddHours(_random.Next(1, 100)),
                ArrivalTime = DateTime.Now.AddHours(_random.Next(101, 200)),
                Price = _random.Next(100, 500),
                Layovers = _random.Next(0, 5),
                AircraftType = _aircraftTypes[_random.Next(_aircraftTypes.Count)],
                WifiAvailable = _random.Next(2) == 0
            });
        }

        return flights;
    }
}