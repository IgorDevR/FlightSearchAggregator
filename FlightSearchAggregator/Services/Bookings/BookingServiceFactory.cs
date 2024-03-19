using FlightSearchAggregator.Models;
using FlightSearchAggregator.Services.Providers;

namespace FlightSearchAggregator.Services.Bookings;

public class FlightBookingFactory
{
    private readonly IFlyQuestService _flyQuestService;
    private readonly ISkyTrailsService _skyTrailsService;
    private readonly ILogger<FlightBookingFactory> _logger;

    public FlightBookingFactory(IFlyQuestService flyQuestService, ISkyTrailsService skyTrailsService,
        ILogger<FlightBookingFactory> logger)
    {
        _flyQuestService = flyQuestService;
        _skyTrailsService = skyTrailsService;
        _logger = logger;
    }

    public IBookingService SelectService(FlightDataProvider dataProvider)
    {
        IBookingService selectedService = dataProvider switch
        {
            FlightDataProvider.FlyQuest => _flyQuestService,
            FlightDataProvider.SkyTrails => _skyTrailsService,
            _ => throw new NotSupportedException($"Unsupported flight data provider: {dataProvider}")
        };

        return selectedService;
    }
}