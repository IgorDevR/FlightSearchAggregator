using DataSourceSkyTrails.Models;
using System;

namespace DataSourceSkyTrails.Services;

public static class FlightDataGenerator
{
    public static List<FlightSkyTrails> FlightSkyTrails { get; private set; } = new List<FlightSkyTrails>();
    private static readonly Random _random = new Random();
    private static readonly object _lock = new object();

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

    static FlightDataGenerator()
    {
        FlightSkyTrails = Generate();
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
                Id = i == 0 ? new Guid("985636ed-3106-4c03-aba5-645011d1db27") : Guid.NewGuid(),
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

    private static void GenerateData(object? state)
    {
        lock (_lock)
        {
            FlightSkyTrails = Generate();
        }
    }
}