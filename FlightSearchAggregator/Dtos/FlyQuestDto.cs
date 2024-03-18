namespace FlightSearchAggregator.Dtos;

public class FlyQuestDto
{
    public Guid Id { get; set; }
    public string Airline { get; set; }
    public string FlightNumber { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public string OriginAirportCode { get; set; }
    public string DestinationAirportCode { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public decimal Price { get; set; }
    public int Layovers { get; set; }
    public string AircraftType { get; set; }
    public bool WifiAvailable { get; set; }
}