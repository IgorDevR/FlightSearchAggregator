using System.Reflection;
using AutoMapper;
using FlightSearchAggregator.Dtos;
using FlightSearchAggregator.Helpers;
using FlightSearchAggregator.Models;
using Microsoft.Extensions.Options;

namespace FlightSearchAggregator.Services.Providers;

public interface ISkyTrailsService
{
    Task<List<Flight>> GetFlights();

    Task<List<Flight>> GetFlightsByFilters(FlightSearchParams flightSearchParams);
}

public class SkyTrailsService : ISkyTrailsService
{
    private readonly IMapper _mapper;
    private readonly AppSettings _settings;
    private readonly ILogger _logger;

    private readonly string _baseUrl;

    public SkyTrailsService(IMapper mapper, IOptions<AppSettings> settings, ILogger<SkyTrailsService> logger)
    {
        _mapper = mapper;
        _settings = settings.Value;
        _logger = logger;

        _baseUrl = _settings.SkyTrailsBaseUrl;
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
                var flightsSkyTrails = await response.Content.ReadFromJsonAsync<List<SkyTrailsDto>>();
                flights = _mapper.Map<List<Flight>>(flightsSkyTrails);
            }

            return flights;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception(
                $"{MethodBase.GetCurrentMethod()?.Name}. Error while retrieving flight data from SkyTrails.", ex);
        }
    }

    public async Task<List<Flight>> GetFlightsByFilters(FlightSearchParams flightSearchParams)
    {
        var searchParams = new
        {
            Airline = flightSearchParams.Airline?.GetDescription(),
            Depart = flightSearchParams.DepartureDate,
            MaxPrice = flightSearchParams.MaxPrice,
            Refundable = flightSearchParams.Refundable,
        };

        try
        {
            List<Flight> flights = new List<Flight>();

            using var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(60),
            };
            var response = await httpClient.GetAsync($"{_baseUrl}/flights/search-by-filters");
            if (response.IsSuccessStatusCode)
            {
                var flightsSkyTrails = await response.Content.ReadFromJsonAsync<List<SkyTrailsDto>>();
                flights = _mapper.Map<List<Flight>>(flightsSkyTrails);
            }

            return flights;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception(
                $"{MethodBase.GetCurrentMethod()?.Name}. Error while retrieving flight data from SkyTrails.", ex);
        }
    }

}