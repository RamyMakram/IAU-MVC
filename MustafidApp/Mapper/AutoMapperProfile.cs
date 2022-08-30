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

            CreateMap<RequestType, RequestTypeDTO>()
                .ForMember(dest => dest.R_ID, src => src.MapFrom(src => src.RequestTypeId))
                .ForMember(dest => dest.R_Name, src => src.MapFrom(src => src.RequestTypeNameAr))
                .ForMember(dest => dest.R_Name_EN, src => src.MapFrom(src => src.RequestTypeNameEn))
                .ReverseMap();

            CreateMap<ApplicantType, ApplicantTypeDTO>()
                .ForMember(dest => dest.APP_ID, src => src.MapFrom(src => src.ApplicantTypeId))
                .ForMember(dest => dest.APP_Name, src => src.MapFrom(src => src.ApplicantTypeNameAr))
                .ForMember(dest => dest.APP_Name_EN, src => src.MapFrom(src => src.ApplicantTypeNameEn))
                .ForMember(dest => dest.APP_Affliated, src => src.MapFrom(src => src.Affliated))
                .ReverseMap();

            CreateMap<TitleMiddleName, TitleNamesDTO>()
                .ForMember(dest => dest.T_ID, src => src.MapFrom(src => src.TitleMiddleNamesId))
                .ForMember(dest => dest.T_Name, src => src.MapFrom(src => src.TitleMiddleNamesNameAr))
                .ForMember(dest => dest.T_Name_EN, src => src.MapFrom(src => src.TitleMiddleNamesNameEn))
                .ReverseMap();

            CreateMap<Country, CountryDTO>()
                .ForMember(dest => dest.C_ID, src => src.MapFrom(src => src.CountryId))
                .ForMember(dest => dest.C_Name, src => src.MapFrom(src => src.CountryNameAr))
                .ForMember(dest => dest.C_Name_EN, src => src.MapFrom(src => src.CountryNameEn))
                .ReverseMap();

            CreateMap<IdDocument, IDDocsDTO>()
                .ForMember(dest => dest.ID_ID, src => src.MapFrom(src => src.IdDocument1))
                .ForMember(dest => dest.ID_Name, src => src.MapFrom(src => src.DocumentNameAr))
                .ForMember(dest => dest.ID_Name_EN, src => src.MapFrom(src => src.DocumentNameEn))
                .ReverseMap();

            CreateMap<City, CityDTO>()
                .ForMember(dest => dest.CI_ID, src => src.MapFrom(src => src.CityId))
                .ForMember(dest => dest.CI_Name, src => src.MapFrom(src => src.CityNameAr))
                .ForMember(dest => dest.CI_Name_EN, src => src.MapFrom(src => src.CityNameEn))
                .ReverseMap();

            CreateMap<Region, RegionDTO>()
                .ForMember(dest => dest.R_ID, src => src.MapFrom(src => src.RegionId))
                .ForMember(dest => dest.R_Name, src => src.MapFrom(src => src.RegionNameAr))
                .ForMember(dest => dest.R_Name_EN, src => src.MapFrom(src => src.RegionNameEn))
                .ForMember(dest => dest.R_Cities, src => src.MapFrom(src => src.Cities))
                .ReverseMap();

            CreateMap<Unit, UnitsDTO>()
                .ForMember(dest => dest.U_ID, src => src.MapFrom(src => src.UnitsId))
                .ForMember(dest => dest.U_Name, src => src.MapFrom(src => src.UnitsNameAr))
                .ForMember(dest => dest.U_Name_EN, src => src.MapFrom(src => src.UnitsNameEn))
                .ReverseMap();

            CreateMap<MainService, MainSerivceDTO>()
                .ForMember(dest => dest.M_ID, src => src.MapFrom(src => src.MainServicesId))
                .ForMember(dest => dest.M_Name, src => src.MapFrom(src => src.MainServicesNameAr))
                .ForMember(dest => dest.M_Name_EN, src => src.MapFrom(src => src.MainServicesNameEn))
                .ReverseMap();

            CreateMap<RequiredDocument, RequiredDocsDTO>()
                .ForMember(dest => dest.RD_ID, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.RD_Name, src => src.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.RD_Name_EN, src => src.MapFrom(src => src.NameEn))
                .ReverseMap();

            CreateMap<SubService, SubSerivceDTO>()
                .ForMember(dest => dest.SS_ID, src => src.MapFrom(src => src.SubServicesId))
                .ForMember(dest => dest.SS_Name, src => src.MapFrom(src => src.SubServicesNameAr))
                .ForMember(dest => dest.SS_Name_EN, src => src.MapFrom(src => src.SubServicesNameEn))
                .ForMember(dest => dest.SS_Docs, src => src.MapFrom(src => src.RequiredDocuments))
                .ReverseMap();

        }
    }
}
