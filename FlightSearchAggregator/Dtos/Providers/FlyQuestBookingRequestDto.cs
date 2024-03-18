namespace FlightSearchAggregator.Dtos.Providers;

public class FlyQuestBookingRequestDto
{
    public Guid FlightId { get; set; }
    public Guid ServiceId { get; set; }
    public string PassengerName { get; set; }
    public string Phone { get; set; }
    public long Seat { get; set; }
    public DateTimeOffset Created { get; set; }
}