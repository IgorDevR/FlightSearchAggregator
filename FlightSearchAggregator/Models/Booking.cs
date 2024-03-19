namespace FlightSearchAggregator.Models;

public class Booking
{
    public Guid Id { get; set; }
    public Guid ProviderBookingId { get; set; }
    public FlightDataProvider DataProvider { get; set; }
    public Guid FlightId { get; set; }
    public string Name { get; set; }
    public string Result { get; set; }
}