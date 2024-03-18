using FlightSearchAggregator.Dtos;
using FlightSearchAggregator.Models;
using FlightSearchAggregator.Services;
using FlightSearchAggregator.Services.Providers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace FlightSearchAggregator.Tests;

public class FlightServiceTests
{
    private static List<Flight> _flightsFromFlyQuest;
    private static List<Flight> _flightsFromSkyTrails;
    private static FlightSortParams _sortParams;
    private static FlightSearchParams _searchParams;

    public FlightServiceTests()
    {
        CreateFlights();
        CreateSortParams();
        CreateSearchParams();
    }

    private static void CreateFlights()
    {
        _flightsFromFlyQuest = new List<Flight>
        {
            new Flight
            {
                FlightId = Guid.NewGuid(), Airline = Airline.DeltaAirLines,
                DataProvider = FlightDataProvider.FlyQuest,
                OriginAirportCode = "qwe",
                DestinationAirportCode = "ewq",
                Price = 200,
            }
        };

        _flightsFromSkyTrails = new List<Flight>
        {
            new Flight
            {
                FlightId = Guid.NewGuid(), Airline = Airline.AllNipponAirways,
                DataProvider = FlightDataProvider.SkyTrails,
                OriginAirportCode = "zxc",
                DestinationAirportCode = "cxz",
                Price = 100,
            }
        };
    }

    private static void CreateSortParams()
    {
        _sortParams = new FlightSortParams
        {
            SortBy = "price"
        };
    }

    private static void CreateSearchParams()
    {
        _searchParams = new FlightSearchParams
        {
            Airline = Airline.DeltaAirLines,
        };
    }

    [Fact]
    public async Task GetAggregatedFlights_Returns_Combined_And_Sorted_Flights()
    {
        var flyQuestServiceMock = new Mock<IFlyQuestService>();
        var skyTrailsServiceMock = new Mock<ISkyTrailsService>();
        var cacheMock = new Mock<IMemoryCache>();

        var cacheEntryMock = Mock.Of<ICacheEntry>();

        cacheMock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock);

        var logger = new NullLogger<FlightService>();

        flyQuestServiceMock.Setup(s => s.GetFlights()).ReturnsAsync(_flightsFromFlyQuest);
        skyTrailsServiceMock.Setup(s => s.GetFlights()).ReturnsAsync(_flightsFromSkyTrails);

        var flightService = new FlightService(flyQuestServiceMock.Object, skyTrailsServiceMock.Object,
            new MemoryCache(new MemoryCacheOptions()), logger);

        // Act
        var result = await flightService.GetAggregatedFlights(_sortParams);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(_flightsFromFlyQuest.Count + _flightsFromSkyTrails.Count, result.Count);
    }

    [Fact]
    public async Task GetFlightsByFilters_Applies_Filters_And_Returns_Filtered_Flights()
    {
        var flyQuestServiceMock = new Mock<IFlyQuestService>();
        var skyTrailsServiceMock = new Mock<ISkyTrailsService>();
        var cacheMock = new Mock<IMemoryCache>();

        var cacheEntryMock = Mock.Of<ICacheEntry>();

        cacheMock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock);

        var logger = new NullLogger<FlightService>();

        flyQuestServiceMock.Setup(s => s.GetFlightsByFilters(_searchParams)).ReturnsAsync(_flightsFromFlyQuest);
        skyTrailsServiceMock.Setup(s => s.GetFlightsByFilters(_searchParams)).ReturnsAsync(_flightsFromSkyTrails);

        var flightService = new FlightService(flyQuestServiceMock.Object, skyTrailsServiceMock.Object,
            new MemoryCache(new MemoryCacheOptions()), logger);
        // Act
        var result = await flightService.GetFlightsByFilters(_searchParams, _sortParams);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(_flightsFromFlyQuest.Count, result.Count);
    }
}