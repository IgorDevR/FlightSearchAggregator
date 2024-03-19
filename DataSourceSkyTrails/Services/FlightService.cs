using DataSourceSkyTrails.Models;

namespace DataSourceSkyTrails.Services;

public class FlightService
{
    public List<FlightSkyTrails> GetFlights()
    {
        return FlightDataGenerator.FlightSkyTrails;
    }

    private static readonly List<BookingDetail> _bookingDetails = new List<BookingDetail>();

    public List<FlightSkyTrails> GetFlightsByFilters(FlightSearchParamsSkyTrails searchParams)
    {
        var flights = FlightDataGenerator.FlightSkyTrails;

        return flights
            .Where(f => (string.IsNullOrEmpty(searchParams.Airline) || f.Airline == searchParams.Airline))
            .Where(f => (!searchParams.Depart.HasValue ||
                         f.Depart.Date == searchParams.Depart.Value.Date))
            .Where(f => (!searchParams.MaxPrice.HasValue || f.Cost <= searchParams.MaxPrice.Value))
            .Where(f => (!searchParams.Refundable || f.Refundable == searchParams.Refundable))
            .ToList();
    }

    public FlightSkyTrails? GetFlight(Guid id)
    {
        return FlightDataGenerator.FlightSkyTrails.FirstOrDefault(f => f.Id == id);
    }

    public BookingDetail Book(FlightSkyTrails flight, BookingRequest bookingRequest)
    {
        var bookingDetail = new BookingDetail
        {
            Id = Guid.NewGuid(),
            FlightId = flight.Id,
            ServiceId = bookingRequest.ServiceId,
            PassengerName = bookingRequest.PassengerName,
            Email = bookingRequest.Email,
            Seat = bookingRequest.Seat,
            Price = flight.Cost,
            Result = "Successfully"
        };

        _bookingDetails.Add(bookingDetail);
        return bookingDetail;
    }

    public BookingDetail? GetBookingDetail(Guid id)
    {
        var bookingDetail = _bookingDetails.FirstOrDefault(_ => _.Id == id);
        return bookingDetail;
    }
}