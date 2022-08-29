using AutoMapper;
using MustafidAppDTO.DTO;
using MustafidAppModels.Models;

namespace MustafidApp.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ServiceType, ServiceTypeDTO>()
                .ForMember(dest => dest.S_ID, src => src.MapFrom(src => src.ServiceTypeId))
                .ForMember(dest => dest.S_Name, src => src.MapFrom(src => src.ServiceTypeNameAr))
                .ForMember(dest => dest.S_Name_EN, src => src.MapFrom(src => src.ServiceTypeNameEn))
                .ReverseMap();
        }
    }
}
