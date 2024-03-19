using FlightSearchAggregator.Auth;
using FlightSearchAggregator.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace FlightSearchAggregator.Example;

public class BookingRequestExample : IExamplesProvider<BookingRequest>
{
    public BookingRequest GetExamples()
    {
        return new BookingRequest
        {
            FlightId = Guid.Parse("985636ed-3106-4c03-aba5-645011d1db27"),
            DataProvider = "SkyTrails",
            PassengerName = "John Doe",
            Phone = "+123456789",
            Email = "john.doe@example.com",
            Seat = 1
        };
    }
}

public class LoginExample : IExamplesProvider<LoginModel>
{
    public LoginModel GetExamples()
    {
        return new LoginModel
        {
           Email = "testUser@gmail.com",
           Password = "test123"
        };
    }
}