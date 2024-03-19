namespace DataSourceSkyTrails.Models;

public class FlightSearchParamsSkyTrails
{
    public string? Airline { get; set; }
    public DateTimeOffset? Depart { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool Refundable { get; set; } = false;
}