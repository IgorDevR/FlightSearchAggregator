using FlightSearchAggregator.Models;

namespace FlightSearchAggregator.Dtos;

public class FlightSearchParams
{
    /// <summary> Enum type of carriers for ease of selection in the swager </summary>
    public Airline? Airline { get; set; }
    public DateTimeOffset? DepartureDate { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MaxLayovers { get; set; }
    public bool? WifiAvailable { get; set; }
    public bool? Refundable { get; set; }

    public override string ToString()
    {
        return $"Airline: {Airline.ToString() ?? "(any)"}\n" +
               $"Departure date: {DepartureDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? "(not set)"}\n" +
               $"Max price: {MaxPrice?.ToString("C") ?? "(none)"}\n" +
               $"Max layovers: {MaxLayovers?.ToString("C") ?? "(none)"}\n" +
               $"Wifi Available: {WifiAvailable}\n" +
               $"Refundable: {Refundable}\n";
    }
}