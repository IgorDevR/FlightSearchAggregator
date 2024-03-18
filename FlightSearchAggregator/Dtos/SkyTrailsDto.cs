namespace FlightSearchAggregator.Dtos;

public class SkyTrailsDto
{
    public Guid Id { get; set; }
    public string Airline { get; set; }
    public string FlightCode { get; set; }
    public string FromCity { get; set; }
    public string ToCity { get; set; }
    public string AirportsCode { get; set; }
    public DateTime Depart { get; set; }
    public DateTime Arrive { get; set; }
    public decimal Cost { get; set; }
    public int Layovers { get; set; }
    public bool Refundable { get; set; }
}