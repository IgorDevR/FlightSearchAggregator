using FlightSearchAggregator.Auth;
using FlightSearchAggregator.Models;
using FlightSearchAggregator.Services.Bookings;

namespace FlightSearchAggregator.DbContext;

public static class DbEmulation
{
    public static Dictionary <Guid, Booking> Bookings  = new Dictionary<Guid, Booking> ();
    public static Dictionary<string, User> Users = new Dictionary<string, User>();
}