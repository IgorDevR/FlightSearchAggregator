using AutoMapper;
using FlightSearchAggregator.DbContext;
using FlightSearchAggregator.Dtos;
using FlightSearchAggregator.Dtos.Providers;
using FlightSearchAggregator.Helpers;
using FlightSearchAggregator.Models;
using FlightSearchAggregator.Services.Bookings;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace FlightSearchAggregator.Services.Providers;

public interface ISkyTrailsService : IBookingService
{
    Task<List<Flight>> GetFlights();

    Task<List<Flight>> GetFlightsByFilters(FlightSearchParams departureDate);
}

public class SkyTrailsService : ISkyTrailsService
{
    private readonly IMapper _mapper;
    private readonly AppSettings _settings;
    private readonly ILogger _logger;

    private readonly HttpService _httpService;
    private const string _clineName = "SkyTrailsClient";

    public SkyTrailsService(IMapper mapper, HttpService httpService, IOptions<AppSettings> settings,
        ILogger<SkyTrailsService> logger)
    {
        _mapper = mapper;
        _httpService = httpService;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<List<Flight>> GetFlights()
    {
        var flightsSkyTrails = await _httpService.ExecuteGetRequest<List<SkyTrailsDto>>(_clineName, "flights");
        var flights = _mapper.Map<List<Flight>>(flightsSkyTrails);

        return flights;
    }

    public async Task<List<Flight>> GetFlightsByFilters(FlightSearchParams flightSearchParams)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "airline", flightSearchParams.Airline?.GetDescription() },
            { "depart", flightSearchParams.DepartureDate?.ToString("yyyy-MM-dd") },
            { "maxPrice", flightSearchParams.MaxPrice?.ToString() },
            { "refundable", flightSearchParams.Refundable?.ToString() }
        };

        var queryString =
            QueryHelpers.AddQueryString("flights/search-by-filters",
                queryParams.Where(kvp => kvp.Value != null));

        var flightsSkyTrails = await _httpService.ExecuteGetRequest<List<SkyTrailsDto>>(_clineName, queryString);

        return _mapper.Map<List<Flight>>(flightsSkyTrails);
    }

    public async Task<BookingDetailDto?> SubmitFlightBooking(BookingRequest bookingRequest)
    {
        var flightBookingDto = _mapper.Map<SkyTrailsBookingRequestDto>(bookingRequest,
            opts => { opts.Items["ServiceId"] = _settings.ServiceId; });
        var skyTrailsBookingDetailDto =
            await _httpService.ExecutePostRequest<SkyTrailsBookingRequestDto, SkyTrailsBookingDetailDto>(_clineName,
                "flights/book",
                flightBookingDto);

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            ProviderBookingId = skyTrailsBookingDetailDto.Id,
            DataProvider = EnumExtensions.ParseEnum(bookingRequest.DataProvider, FlightDataProvider.Unknown),
            FlightId = bookingRequest.FlightId,
            Name = bookingRequest.PassengerName,
            Result = skyTrailsBookingDetailDto.Result
        };
        DbEmulation.Bookings.Add(booking.Id, booking);

        var bookingDetail = _mapper.Map<BookingDetailDto>(booking);
        return bookingDetail;
    }

    public async Task<BookingDetailDto?> GetBookingDetail(Guid id)
    {
        var skyTrailsBookingDetailDto =
            await _httpService.ExecuteGetRequest<SkyTrailsBookingDetailDto>(_clineName, $"flights/book/{id}");
        return _mapper.Map<BookingDetailDto>(skyTrailsBookingDetailDto);
    }

    public async Task<Flight?> GetFlight(Guid flightId)
    {
        try
        {
            var skyTrailsDto = await _httpService.ExecuteGetRequest<SkyTrailsDto>(_clineName, $"flights/{flightId}");
            return _mapper.Map<Flight>(skyTrailsDto);
        }
        catch (HttpRequestException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        catch (Exception e)
        {
            throw;
        }
    }
}