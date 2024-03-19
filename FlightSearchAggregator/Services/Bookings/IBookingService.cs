using FlightSearchAggregator.Dtos;
using FlightSearchAggregator.Models;

namespace FlightSearchAggregator.Services.Bookings;

public interface IBookingService
{
    Task<BookingDetailDto?> SubmitFlightBooking(BookingRequest bookingRequest);

    Task<BookingDetailDto?> GetBookingDetail(Guid id);

    Task<Flight?> GetFlight(Guid flightId);
}