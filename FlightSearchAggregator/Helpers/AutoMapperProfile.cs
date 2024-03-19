using AutoMapper;
using FlightSearchAggregator.Dtos;
using FlightSearchAggregator.Dtos.Providers;
using FlightSearchAggregator.Models;

namespace FlightSearchAggregator.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<FlyQuestDto, Flight>()
                .ForMember(dest => dest.FlightId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Airline,
                    opt => opt.MapFrom(src => EnumExtensions.ParseEnum(src.Airline, Airline.Unknown)))
                .ForMember(dest => dest.Refundable, opt => opt.Ignore())
                .AfterMap((src, dest) => { dest.Refundable ??= false; })
                .AfterMap((src, dest) => { dest.DataProvider = FlightDataProvider.FlyQuest; });

            CreateMap<SkyTrailsDto, Flight>()
                .ForMember(dest => dest.FlightId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Airline,
                    opt => opt.MapFrom(src => EnumExtensions.ParseEnum(src.Airline, Airline.Unknown)))
                .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.FlightCode))
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.FromCity))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.ToCity))
                .ForMember(dest => dest.OriginAirportCode,
                    opt => opt.MapFrom(src => SplitAirportCodes(src.AirportsCode).Item1))
                .ForMember(dest => dest.DestinationAirportCode,
                    opt => opt.MapFrom(src => SplitAirportCodes(src.AirportsCode).Item2))
                .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => new DateTimeOffset(src.Depart)))
                .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => new DateTimeOffset(src.Arrive)))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Cost))
                .ForMember(dest => dest.Layovers, opt => opt.MapFrom(src => (int?)src.Layovers))
                .ForMember(dest => dest.Refundable, opt => opt.MapFrom(src => (bool?)src.Refundable))
                .ForMember(dest => dest.AircraftType, opt => opt.Ignore())
                .ForMember(dest => dest.WifiAvailable, opt => opt.Ignore())
                .AfterMap((src, dest) => { dest.AircraftType ??= "Information not provided by the carrier"; })
                .AfterMap((src, dest) => { dest.WifiAvailable ??= false; })
                .AfterMap((src, dest) => { dest.DataProvider = FlightDataProvider.SkyTrails; });

            CreateMap<BookingRequest, FlyQuestBookingRequestDto>()
                .AfterMap((src, dest) => { dest.Created = DateTimeOffset.UtcNow; })
                .AfterMap((src, dest, context) =>
                {
                    if (context.Items.ContainsKey("ServiceId"))
                    {
                        dest.ServiceId = new Guid(context.Items["ServiceId"].ToString());
                    }
                });

            CreateMap<BookingRequest, SkyTrailsBookingRequestDto>()
                .AfterMap((src, dest) => { dest.Created = DateTimeOffset.UtcNow; })
                .AfterMap((src, dest, context) =>
                {
                    if (context.Items.ContainsKey("ServiceId"))
                    {
                        dest.ServiceId = new Guid(context.Items["ServiceId"].ToString());
                    }
                });

            CreateMap<FlyQuestBookingDetailDto, BookingDetailDto>();
            CreateMap<SkyTrailsBookingDetailDto, BookingDetailDto>();

            CreateMap<Booking, BookingDetailDto>()
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id));
        }

        private static (string?, string?) SplitAirportCodes(string airportsCode)
        {
            var codes = airportsCode.Split('-');
            return (codes.FirstOrDefault(), codes.LastOrDefault());
        }
    }
}