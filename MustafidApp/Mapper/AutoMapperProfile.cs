﻿using AutoMapper;
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

            #region Eforms
            CreateMap<InputType, InputTypeDTO>()
                .ForMember(dest => dest.IT_ID, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.IT_Desc, src => src.MapFrom(src => src.Placeholder))
                .ForMember(dest => dest.IT_Desc_EN, src => src.MapFrom(src => src.PlaceholderEn))
                .ForMember(dest => dest.IT_Num_Only, src => src.MapFrom(src => src.IsNumber))
                .ForMember(dest => dest.IT_Date_Only, src => src.MapFrom(src => src.IsDate))
                .ReverseMap();

            CreateMap<Paragraph, ParagraphDTO>()
                .ForMember(dest => dest.Para_ID, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.Para_TXT, src => src.MapFrom(src => src.Name))
                .ForMember(dest => dest.Para_TXT_EN, src => src.MapFrom(src => src.NameEn))
                .ReverseMap();

            CreateMap<Separator, SeparatorDTO>()
                .ForMember(dest => dest.Sepa_ID, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.Sepa_Empty, src => src.MapFrom(src => src.IsEmpty))
                .ReverseMap();

            CreateMap<CheckBoxType, CheckBoxTypeDTO>()
                .ForMember(dest => dest.CB_ID, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.CB_Name, src => src.MapFrom(src => src.Name))
                .ForMember(dest => dest.CB_Name_EN, src => src.MapFrom(src => src.NameEn))
                .ReverseMap();

            CreateMap<RadioType, RadioTypeDTO>()
                .ForMember(dest => dest.RB_ID, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.RB_Name, src => src.MapFrom(src => src.Name))
                .ForMember(dest => dest.RB_Name_EN, src => src.MapFrom(src => src.NameEn))
                .ReverseMap();

            CreateMap<TableColumn, TableColumnDTO>()
                .ForMember(dest => dest.TC_ID, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.TC_Name, src => src.MapFrom(src => src.Name))
                .ForMember(dest => dest.TC_Name_EN, src => src.MapFrom(src => src.NameEn))
                .ReverseMap();

            CreateMap<Question, QuestionDTO>()
                .ForMember(dest => dest.Q_ID, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.Q_Order, src => src.MapFrom(src => src.IndexOrder))
                .ForMember(dest => dest.Q_Type, src => src.MapFrom(src => src.Type))
                .ForMember(dest => dest.Q_Lable_Name, src => src.MapFrom(src => src.LableName))
                .ForMember(dest => dest.Q_Lable_Name_En, src => src.MapFrom(src => src.LableNameEn))
                .ForMember(dest => dest.Q_Is_Req, src => src.MapFrom(src => src.Requird))
                .ForMember(dest => dest.Q_RefName, src => src.MapFrom(src => src.RefTo))
                .ForMember(dest => dest.Q_Rows_Num, src => src.MapFrom(src => src.TableRowsNum))
                .ForMember(dest => dest.Q_Input, src => src.MapFrom(src => src.InputType))
                .ForMember(dest => dest.Q_Para, src => src.MapFrom(src => src.Paragraph))
                .ForMember(dest => dest.Q_Sep, src => src.MapFrom(src => src.Separator))
                .ForMember(dest => dest.Q_Check_Box, src => src.MapFrom(src => src.CheckBoxTypes))
                .ForMember(dest => dest.Q_Radio, src => src.MapFrom(src => src.RadioTypes))
                .ForMember(dest => dest.Q_T_Columns, src => src.MapFrom(src => src.TableColumns))
                .ReverseMap();

            CreateMap<EForm, EFormDTO>()
                .ForMember(dest => dest.EF_ID, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.EF_Name, src => src.MapFrom(src => src.Name))
                .ForMember(dest => dest.EF_Name_EN, src => src.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.EF_Approved_Unit, src => src.MapFrom(src => src.UnitToApproveNavigation))
                .ForMember(dest => dest.EF_Q_List, src => src.MapFrom(src => src.Questions))
                .ReverseMap();

            #endregion

        }
    }
}