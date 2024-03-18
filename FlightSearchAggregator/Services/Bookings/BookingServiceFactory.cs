using FlightSearchAggregator.Models;

namespace FlightSearchAggregator.Services.Bookings;

public class BookingServiceFactory
{
    private readonly Dictionary<FlightDataProvider, IBookingService> _bookingServices;
    private readonly ILogger<BookingServiceFactory> _logger;

    public BookingServiceFactory(
        IBookingService flyQuestService,
        IBookingService skyTrailsService,
        ILogger<BookingServiceFactory> logger)
    {
        _bookingServices = new Dictionary<FlightDataProvider, IBookingService>
        {
            { FlightDataProvider.FlyQuest, flyQuestService },
            { FlightDataProvider.SkyTrails, skyTrailsService }
        };
        _logger = logger;
    }

    public IBookingService GetBookingService(FlightDataProvider dataProvider)
    {
        if (!_bookingServices.TryGetValue(dataProvider, out var bookingService))
        {
            var errorMsg = $"Unsupported flight data provider: {dataProvider}.";
            _logger.LogError(errorMsg);
            throw new NotSupportedException(errorMsg);
        }

        return bookingService;
    }
}