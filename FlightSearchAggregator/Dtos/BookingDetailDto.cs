namespace FlightSearchAggregator.Dtos;

public class BookingDetailDto
{
    public Guid BookingId { get; set; }
    public Guid FlightId { get; set; }
    public string Result { get; set; }
}