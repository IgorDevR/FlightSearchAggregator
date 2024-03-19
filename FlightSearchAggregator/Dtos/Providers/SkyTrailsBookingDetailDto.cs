namespace FlightSearchAggregator.Dtos.Providers;

public class SkyTrailsBookingDetailDto
{
    public Guid Id { get; set; }
    public Guid FlightId { get; set; }
    public string PassengerName { get; set; }
    public string Email { get; set; }
    public long Seat { get; set; }
    public Guid ServiceId { get; set; }
    public decimal Price { get; set; }
    public string Result { get; set; }
}