using FlightSearchAggregator.DbContext;
using FlightSearchAggregator.Dtos;
using FlightSearchAggregator.Models;

namespace FlightSearchAggregator.Services.Bookings;

public class BookFlightService
{
    private readonly ILogger _logger;
    private readonly FlightBookingFactory _flightBookingFactory;

    public BookFlightService(FlightBookingFactory flightBookingFactory, ILogger<BookFlightService> logger)
    {
        _flightBookingFactory = flightBookingFactory;
        _logger = logger;
    }

    public async Task<BookingDetailDto> BookFlight(BookingRequest bookingRequest, FlightDataProvider dataProvider)
    {
        var bookingService = _flightBookingFactory.SelectService(dataProvider);
        var bookingDetail = await bookingService.SubmitFlightBooking(bookingRequest);
        return bookingDetail;
    }
    public async Task<Flight?> GetFlight(Guid flightId, FlightDataProvider dataProvider)
    {
        var bookingService = _flightBookingFactory.SelectService(dataProvider);
        var bookingDetail = await bookingService.GetFlight(flightId);
        return bookingDetail;
    }
    public async Task<BookingDetailDto?> GetBookingDetail(Guid id)
    {
        if (!DbEmulation.Bookings.TryGetValue(id, out var booking))
        {
            return null;
        }

        var bookingService = _flightBookingFactory.SelectService(booking.DataProvider);
        var bookingDetailDto = await bookingService.GetBookingDetail(booking.ProviderBookingId);
        bookingDetailDto.BookingId = id;

        return bookingDetailDto;
    }
}