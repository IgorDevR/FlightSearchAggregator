using FlightSearchAggregator.Dtos;

namespace FlightSearchAggregator.Services.Bookings;

public interface IBookingService
{
    Task<BookingDetailDto?> SubmitFlightBooking(BookingRequest bookingRequest);

    Task<BookingDetailDto?> GetBookingDetail(Guid id);
}