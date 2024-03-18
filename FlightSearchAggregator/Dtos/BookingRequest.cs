namespace FlightSearchAggregator.Dtos;

public class BookingRequest
{
    [Helpers.Validator(ErrorMessage = "FlightId must not be null")]
    public Guid FlightId { get; set; }

    [Helpers.Validator(ErrorMessage = "PassengerName must not be null or empty")]
    public string DataProvider { get; set; }

    [Helpers.Validator(ErrorMessage = "PassengerName must not be null or empty")]
    public string PassengerName { get; set; }

    [Helpers.Validator(ErrorMessage = "Email must not be null or empty")]
    public string Email { get; set; }

    [Helpers.Validator(ErrorMessage = "Email must not be null or empty")]
    public long Seat { get; set; }
}