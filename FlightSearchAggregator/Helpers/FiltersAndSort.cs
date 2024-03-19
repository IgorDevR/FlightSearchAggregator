using FlightSearchAggregator.Dtos;
using FlightSearchAggregator.Models;

namespace FlightSearchAggregator.Helpers;

public class FiltersAndSort
{
    public static IEnumerable<Flight> ApplyFilters(IEnumerable<Flight> flights, FlightSearchParams searchParams)
    {
        return flights
            .Where(f => (string.IsNullOrEmpty(searchParams.Airline.ToString()) || f.Airline.GetDescription() == searchParams.Airline!.GetDescription()))
            .Where(f => (!searchParams.DepartureDate.HasValue ||
                         f.DepartureTime.Date == searchParams.DepartureDate.Value.Date))
            .Where(f => (!searchParams.MaxPrice.HasValue || f.Price <= searchParams.MaxPrice.Value))
            .Where(f => (!searchParams.MaxLayovers.HasValue || f.Layovers <= searchParams.MaxLayovers.Value))
            .Where(f => (!searchParams.WifiAvailable.HasValue || f.WifiAvailable == searchParams.WifiAvailable))
            .Where(f => (!searchParams.Refundable.HasValue || f.Refundable == searchParams.Refundable));
    }

    public static IEnumerable<Flight> SortFlights(IEnumerable<Flight> flights, FlightSortParams flightSortParams)
    {
        return flightSortParams.SortBy.ToLower() switch
        {
            "price" => flightSortParams.SortDescending.Value
                ? flights.OrderByDescending(f => f.Price)
                : flights.OrderBy(f => f.Price),
            "departuretime" => flightSortParams.SortDescending.Value
                ? flights.OrderByDescending(f => f.DepartureTime)
                : flights.OrderBy(f => f.DepartureTime),
            _ => flights.OrderBy(f => f.Price)
        };
    }
}