namespace FlightSearchAggregator.Models;

public class Flight
{
    public Guid FlightId { get; set; }

    public FlightDataProvider DataProvider { get; set; }

    public Airline Airline { get; set; }
    public string FlightNumber { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public string OriginAirportCode { get; set; }
    public string DestinationAirportCode { get; set; }
    public DateTimeOffset DepartureTime { get; set; }
    public DateTimeOffset ArrivalTime { get; set; }
    public decimal Price { get; set; }
    public string AircraftType { get; set; }
    public int Layovers { get; set; }
    public bool? WifiAvailable { get; set; }
    public bool? Refundable { get; set; }
}

public enum Airline
{
    Unknown,
    AmericanAirlines,
    DeltaAirLines,
    UnitedAirlines,
    SouthwestAirlines,
    AirFrance,
    Lufthansa,
    BritishAirways,
    QantasAirways,
    Emirates,
    CathayPacific,
    SingaporeAirlines,
    AllNipponAirways
}

public enum FlightDataProvider
{
    Unknown,
    FlyQuest,
    SkyTrails,
}