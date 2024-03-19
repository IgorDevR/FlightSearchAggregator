using FlightSearchAggregator.Dtos;
using FlightSearchAggregator.Helpers;
using FlightSearchAggregator.Models;
using FlightSearchAggregator.Services.Providers;
using Microsoft.Extensions.Caching.Memory;

namespace FlightSearchAggregator.Services;

public class FlightService
{
    private readonly IFlyQuestService _flyQuestService;
    private readonly ISkyTrailsService _skyTrailsService;
    private readonly IMemoryCache _cache;
    private readonly ILogger _logger;
    private List<Flight>? _cachedFlights = new List<Flight>();
    private readonly string _cacheKey = "AggregatedFlights";

    public FlightService(IFlyQuestService flyQuestService, ISkyTrailsService skyTrailsService, IMemoryCache cache,
        ILogger<FlightService> logger)
    {
        _flyQuestService = flyQuestService;
        _skyTrailsService = skyTrailsService;
        _cache = cache;
        _logger = logger;
    }

    public async Task<List<Flight>> GetAggregatedFlights(FlightSortParams sortParams)
    {
        if (!_cache.TryGetValue(_cacheKey, out _cachedFlights) || !_cachedFlights!.Any())
        {
            var tasks = new List<Task<List<Flight>>>
            {
                _flyQuestService.GetFlights(),
                _skyTrailsService.GetFlights()
            };
            var results = await Task.WhenAll(tasks);

            var aggregatedFlights = results.SelectMany(flights => flights);
            var sortedFlights = FiltersAndSort.SortFlights(aggregatedFlights, sortParams).ToList();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

            _cache.Set(_cacheKey, sortedFlights, cacheEntryOptions);

            return sortedFlights;
        }
        else
        {
            _logger.LogInformationWithMethod($"Retrieving flights from cache.");
            return _cachedFlights!;
        }
    }

    public async Task<List<Flight>> GetFlightsByFilters(FlightSearchParams searchParams,
        FlightSortParams sortParams)
    {
        if (!_cache.TryGetValue(_cacheKey, out List<Flight>? aggregatedFlights))
        {
            var tasks = new List<Task<List<Flight>>>
            {
                _flyQuestService.GetFlightsByFilters(searchParams),
                _skyTrailsService.GetFlightsByFilters(searchParams)
            };
            var results = await Task.WhenAll(tasks);
            aggregatedFlights = results.SelectMany(flights => flights).ToList();
        }

        var applyFilters = FiltersAndSort.ApplyFilters(aggregatedFlights!, searchParams);
        applyFilters = FiltersAndSort.SortFlights(applyFilters, sortParams);

        return applyFilters.ToList();
    }
}