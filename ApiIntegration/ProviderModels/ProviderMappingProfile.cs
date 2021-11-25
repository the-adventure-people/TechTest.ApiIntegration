using ApiIntegration.Models;
using AutoMapper;

namespace ApiIntegration.ProviderModels
{
    public class ProviderMappingProfile : Profile
    {
        public ProviderMappingProfile()
        {
            CreateMap<Availability, TourAvailability>()
                .ForMember(d => d.SellingPrice, o => o.MapFrom(s => s.Price))
                .ForMember(d => d.AvailabilityCount, o => o.MapFrom(s => s.Spaces))
                .ForMember(d => d.TourDuration, o => o.MapFrom(s => s.Nights))
                .ForMember(d => d.StartDate, o => o.MapFrom(s => s.DepartureDate));
        }
    }
}