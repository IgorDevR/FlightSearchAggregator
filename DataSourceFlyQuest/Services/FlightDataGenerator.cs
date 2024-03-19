using DataSourceFlyQuest.Models;

namespace DataSourceFlyQuest.Services;

public static class FlightDataGenerator
{
    private static Timer? _timer;
    public static List<FlightFlyQuest> FlightFlyQuests { get; private set; } = new List<FlightFlyQuest>();
    private static readonly Random _random = new Random();
    private static readonly object _lock = new object();

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

    static FlightDataGenerator()
    {
        FlightFlyQuests = Generate();
        _timer = new Timer(GenerateData, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
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
                Id = i == 0 ? new Guid("ed549510-3451-4130-8cb2-b009214cfb17") : Guid.NewGuid(),
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

    private static void GenerateData(object? state)
    {
        lock (_lock)
        {
            FlightFlyQuests = Generate();
        }
    }
}