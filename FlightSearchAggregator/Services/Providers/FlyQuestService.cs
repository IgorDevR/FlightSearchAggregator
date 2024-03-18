using System.Reflection;
using AutoMapper;
using FlightSearchAggregator.Dtos;
using FlightSearchAggregator.Helpers;
using FlightSearchAggregator.Models;
using Microsoft.Extensions.Options;

namespace FlightSearchAggregator.Services.Providers;

public interface IFlyQuestService
{
    Task<List<Flight>> GetFlights();

    Task<List<Flight>> GetFlightsByFilters(FlightSearchParams departureDate);
}

public class FlyQuestService : IFlyQuestService
{
    private readonly IMapper _mapper;
    private readonly AppSettings _settings;
    private readonly ILogger _logger;

    private readonly string _baseUrl;

    public FlyQuestService(IMapper mapper, IOptions<AppSettings> settings, ILogger<FlyQuestService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
        _mapper = mapper;

        _baseUrl = _settings.FlyQuestBaseUrl;
    }

    public async Task<List<Flight>> GetFlights()
    {
        try
        {
            List<Flight> flights = new List<Flight>();

            using var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(60)
            };
            var response = await httpClient.GetAsync($"{_baseUrl}/flights");
            if (response.IsSuccessStatusCode)
            {
                var flightsFlyQuest = await response.Content.ReadFromJsonAsync<List<FlyQuestDto>>();
                flights = _mapper.Map<List<Flight>>(flightsFlyQuest);
            }

            return flights;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception(
                $"{MethodBase.GetCurrentMethod()?.Name}. Error while retrieving flight data from FlyQuest.", ex);
        }
    }

    public async Task<List<Flight>> GetFlightsByFilters(FlightSearchParams flightSearchParams)
    {
        var searchParams = new
        {
            Airline = flightSearchParams.Airline?.GetDescription(),
            DepartureTime = flightSearchParams.DepartureDate,
            MaxPrice = flightSearchParams.MaxPrice,
            WifiAvailable = flightSearchParams.WifiAvailable,
        };

        try
        {
            List<Flight> flights = new List<Flight>();

            using var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(60)
            };
            var response = await httpClient.GetAsync($"{_baseUrl}/flights/search");
            if (response.IsSuccessStatusCode)
            {
                var flightsFlyQuest = await response.Content.ReadFromJsonAsync<List<FlyQuestDto>>();
                flights = _mapper.Map<List<Flight>>(flightsFlyQuest);
            }

            return flights;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception(
                $"{MethodBase.GetCurrentMethod()?.Name}. Error while retrieving flight data from FlyQuest.",
                ex);
        }
    }
}