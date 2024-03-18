using FlightSearchAggregator.DbContext;
using FlightSearchAggregator.Dtos;
using FlightSearchAggregator.Models;

namespace FlightSearchAggregator.Services.Bookings;

public class BookFlightService
{
    private readonly ILogger _logger;
    private readonly BookingServiceFactory _bookingServiceFactory;

    public BookFlightService(BookingServiceFactory bookingServiceFactory, ILogger<BookFlightService> logger)
    {
        _bookingServiceFactory = bookingServiceFactory;
        _logger = logger;
    }

    public async Task<BookingDetailDto> BookFlight(BookingRequest bookingRequest, FlightDataProvider dataProvider)
    {
        var bookingService = _bookingServiceFactory.GetBookingService(dataProvider);
        var bookingDetail = await bookingService.SubmitFlightBooking(bookingRequest);
        return bookingDetail;
    }

    public async Task<BookingDetailDto?> GetBookingDetail(Guid id)
    {
        if (!DbEmulation.Bookings.TryGetValue(id, out var booking))
        {
            return null;
        }

        var bookingService = _bookingServiceFactory.GetBookingService(booking.DataProvider);
        var bookingDetailDto = await bookingService.GetBookingDetail(booking.ProviderBookingId);
        bookingDetailDto.BookingId = id;

        return bookingDetailDto;
    }
}