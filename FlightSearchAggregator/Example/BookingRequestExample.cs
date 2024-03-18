using FlightSearchAggregator.Dtos;
using FlightSearchAggregator.Models;
using Swashbuckle.AspNetCore.Filters;

public class BookingRequestExample : IExamplesProvider<BookingRequest>
{
    public BookingRequest GetExamples()
    {
        return new BookingRequest
        {
            FlightId = Guid.Parse("3235e614-62fa-46f7-95f7-94e06bf49fd6"),
            DataProvider = "SkyTrails",
            PassengerName = "John Doe",
            Phone = "+123456789",
            Email = "john.doe@example.com",
            Seat = 1
        };
    }
}
