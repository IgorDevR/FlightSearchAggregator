namespace FlightSearchAggregator.Dtos;

public class FlightSortParams
{
    public string? SortBy { get; set; } = "price";
    public bool? SortDescending { get; set; } = true;

    public override string ToString()
    {
        return $"Sort by: {SortBy}\n" +
               $"Sort descending: {SortDescending}";
    }
}