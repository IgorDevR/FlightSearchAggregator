namespace DataSourceFlyQuest.Models;

public class FlightSearchParamsFlyQuest
{
    public string Airline { get; set; }
    public DateTimeOffset? DepartureTime { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool WifiAvailable { get; set; } = false;
}