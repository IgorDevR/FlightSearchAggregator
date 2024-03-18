using AutoMapper;
using FlightSearchAggregator.DbContext;
using FlightSearchAggregator.Dtos;
using FlightSearchAggregator.Dtos.Providers;
using FlightSearchAggregator.Helpers;
using FlightSearchAggregator.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Reflection;
using FlightSearchAggregator.Services.Bookings;

namespace FlightSearchAggregator.Services.Providers;

public interface IFlyQuestService
{
    Task<List<Flight>> GetFlights();

    Task<List<Flight>> GetFlightsByFilters(FlightSearchParams departureDate);
}

public class FlyQuestService : IFlyQuestService, IBookingService
{
    private readonly IMapper _mapper;
    private readonly AppSettings _settings;
    private readonly ILogger _logger;

    private readonly HttpService _httpService;

    private const string _clineName = "FlyQuestClient";

    public FlyQuestService(IMapper mapper, HttpService httpService, IOptions<AppSettings> settings,
        ILogger<FlyQuestService> logger)
    {
        _settings = settings.Value;
        _httpService = httpService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<Flight>> GetFlights()
    {
        var flightsFlyQuest = await _httpService.ExecuteGetRequest<List<FlyQuestDto>>(_clineName,"flights");
        var flights = _mapper.Map<List<Flight>>(flightsFlyQuest);

        return flights;
    }

    public async Task<List<Flight>> GetFlightsByFilters(FlightSearchParams flightSearchParams)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "airline", flightSearchParams.Airline?.GetDescription() },
            { "departureTime", flightSearchParams.DepartureDate?.ToString("yyyy-MM-dd") },
            { "maxPrice", flightSearchParams.MaxPrice?.ToString() },
            { "wifiAvailable", flightSearchParams.WifiAvailable?.ToString() }
        };

        var queryString =
            QueryHelpers.AddQueryString("flights/search",
                queryParams.Where(kvp => kvp.Value != null));

        var flightsFlyQuest = await _httpService.ExecuteGetRequest<List<FlyQuestDto>>(_clineName, queryString);

        return _mapper.Map<List<Flight>>(flightsFlyQuest);
    }

    public async Task<BookingDetailDto?> SubmitFlightBooking(BookingRequest bookingRequest)
    {
        var flightBookingDto = _mapper.Map<FlyQuestBookingRequestDto>(bookingRequest,
            opts => { opts.Items["ServiceId"] = _settings.ServiceId; });
        var FlyQuestBookingDetailDto =
            await _httpService.ExecutePostRequest<FlyQuestBookingRequestDto, FlyQuestBookingDetailDto>(_clineName,
                "flights/book",
                flightBookingDto);

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            ProviderBookingId = FlyQuestBookingDetailDto.Id,
            DataProvider = EnumExtensions.ParseEnum(bookingRequest.DataProvider, FlightDataProvider.Unknown),
            FlightId = bookingRequest.FlightId,
            Name = bookingRequest.PassengerName,
            Result = FlyQuestBookingDetailDto.Result
        };
        DbEmulation.Bookings.Add(booking.Id, booking);

        var bookingDetail = _mapper.Map<BookingDetailDto>(booking);
        return bookingDetail;
    }

    public async Task<BookingDetailDto?> GetBookingDetail(Guid id)
    {
        var flyQuestBookingDetailDto =
            await _httpService.ExecuteGetRequest<FlyQuestBookingDetailDto>(_clineName,$"flights/book/{id}");
        return _mapper.Map<BookingDetailDto>(flyQuestBookingDetailDto);
    }
}