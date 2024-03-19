using DataSourceFlyQuest.Models;
using System;

namespace DataSourceFlyQuest.Services;

public class FlightService
{
    private static readonly List<BookingDetail> _bookingDetails = new List<BookingDetail>();

    public List<FlightFlyQuest> GetFlights()
    {
        return FlightDataGenerator.FlightFlyQuests;
    }

    public List<FlightFlyQuest> GetFlightsByFilters(FlightSearchParamsFlyQuest searchParams)
    {
        var flights = FlightDataGenerator.FlightFlyQuests;

        return flights
            .Where(f => (string.IsNullOrEmpty(searchParams.Airline) || f.Airline == searchParams.Airline))
            .Where(f => (!searchParams.DepartureTime.HasValue ||
                         f.DepartureTime.Date == searchParams.DepartureTime.Value.Date))
            .Where(f => (!searchParams.MaxPrice.HasValue || f.Price <= searchParams.MaxPrice.Value))
            .Where(f => (!searchParams.WifiAvailable || f.WifiAvailable == searchParams.WifiAvailable))
            .ToList();
    }

    public FlightFlyQuest? GetFlight(Guid id)
    {
        return FlightDataGenerator.FlightFlyQuests.FirstOrDefault(f => f.Id == id);
    }

    public BookingDetail Book(FlightFlyQuest flight, BookingRequest bookingRequest)
    {
        var bookingDetail = new BookingDetail
        {
            Id = Guid.NewGuid(),
            FlightId = flight.Id,
            ServiceId = bookingRequest.ServiceId,
            PassengerName = bookingRequest.PassengerName,
            Phone = bookingRequest.Phone,
            Seat = bookingRequest.Seat,
            Price = flight.Price,
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