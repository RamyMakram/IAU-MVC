using AutoMapper;
using Microsoft.Extensions.Configuration;
using MustafidAppDTO.DTO;
using MustafidAppModels.Models;
using MustafidApp.Helpers.SaveRequest;

namespace MustafidApp.Mapper
{
    public class AutoMapperProfile : Profile
    {
        IConfiguration configuration;
        public AutoMapperProfile(IConfiguration _configuration)
        {
            this.configuration = _configuration;

            CreateMap<ServiceType, ServiceTypeDTO>()
                .ForMember(dest => dest.S_ID, src => src.MapFrom(src => src.ServiceTypeId))
                .ForMember(dest => dest.S_Name, src => src.MapFrom(src => src.ServiceTypeNameAr))
                .ForMember(dest => dest.S_Name_EN, src => src.MapFrom(src => src.ServiceTypeNameEn))
                .ForMember(dest => dest.S_Img, src => src.MapFrom(src => configuration["ImagePath"] + "/" + src.ImagePath))
                .ReverseMap();

            CreateMap<RequestType, RequestTypeDTO>()
                .ForMember(dest => dest.R_ID, src => src.MapFrom(src => src.RequestTypeId))
                .ForMember(dest => dest.R_Name, src => src.MapFrom(src => src.RequestTypeNameAr))
                .ForMember(dest => dest.R_Name_EN, src => src.MapFrom(src => src.RequestTypeNameEn))
                .ForMember(dest => dest.R_IsReq, src => src.MapFrom(src => src.IsRequestType))
                .ForMember(dest => dest.R_Img, src => src.MapFrom(src => configuration["ImagePath"] + "/" + src.ImagePath))
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

            CreateMap<PhoneNumberNotification, NotificationsDTO>()
                .ForMember(dest => dest.Noti_TXT, src => src.MapFrom(src => src.Message))
                .ForMember(dest => dest.Noti_TXT_EN, src => src.MapFrom(src => src.MessageEn))
                .ForMember(dest => dest.Noti_Date, src => src.MapFrom(src => src.NotiDate))
                .ReverseMap();

            CreateMap<PersonelDatum, PersonalDataDTO>()
            .ForMember(dest => dest.PD_ID, src => src.MapFrom(src => src.PersonelDataId))
            .ForMember(dest => dest.PD_IAUNumber, src => src.MapFrom(src => src.IauIdNumber))
            .ForMember(dest => dest.PD_TitleNames_ID, src => src.MapFrom(src => src.TitleMiddleNamesId))
            .ForMember(dest => dest.PD_F_Name, src => src.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.PD_M_Name, src => src.MapFrom(src => src.MiddleName))
            .ForMember(dest => dest.PD_L_Name, src => src.MapFrom(src => src.LastName))
            .ForMember(dest => dest.PD_National_ID, src => src.MapFrom(src => src.NationalityId))
            .ForMember(dest => dest.PD_ID_Doc_ID, src => src.MapFrom(src => src.IdDocument))
            .ForMember(dest => dest.PD_ID_Number, src => src.MapFrom(src => src.IdNumber))
            .ForMember(dest => dest.PD_C_ID, src => src.MapFrom(src => src.CountryId))
            .ForMember(dest => dest.PD_Address_C_ID, src => src.MapFrom(src => src.AddressCountryId))
            .ForMember(dest => dest.PD_Address_City_ID, src => src.MapFrom(src => src.AddressCityId))
            .ForMember(dest => dest.PD_Adress_R_ID, src => src.MapFrom(src => src.AdressRegionId))
            .ForMember(dest => dest.PD_Address_City, src => src.MapFrom(src => src.AddressCity))
            .ForMember(dest => dest.PD_Adress_Region, src => src.MapFrom(src => src.AdressRegion))
            .ForMember(dest => dest.PD_Postal, src => src.MapFrom(src => src.PostalCode))
            .ForMember(dest => dest.PD_mail, src => src.MapFrom(src => src.Email))
            .ForMember(dest => dest.PD_Phone, src => src.MapFrom(src => src.Mobile))
            //.ForMember(dest => dest.PD_EFormAnswer, src => src.MapFrom(src => src.))
            .ReverseMap();

            CreateMap<RequestTransaction, RequestTransactionDTO>()
                .ForMember(dest => dest.ReqTrans_DateWillEnd, src => src.MapFrom(src => src.ExpireDays))
                .ForMember(dest => dest.ReqTrans_Unit, src => src.MapFrom(src => src.ToUnit))
                .ReverseMap();


            CreateMap<RequestDatum, RequestDTO>()
                .ForMember(dest => dest.Req_ID, src => src.MapFrom(src => src.RequestDataId))
                .ForMember(dest => dest.Req_U_ID, src => src.MapFrom(src => src.UnitId))
                .ForMember(dest => dest.Req_SS_ID, src => src.MapFrom(src => src.SubServicesId))
                .ForMember(dest => dest.Req_Notes, src => src.MapFrom(src => src.RequiredFieldsNotes))
                .ForMember(dest => dest.Req_S_ID, src => src.MapFrom(src => src.ServiceTypeId))
                .ForMember(dest => dest.Req_R_ID, src => src.MapFrom(src => src.RequestTypeId))
                .ForMember(dest => dest.Req_Is_Mos, src => src.MapFrom(src => src.IsTwasulOc))
                .ForMember(dest => dest.Req_Status, src => src.MapFrom(src => src.RequestStateId))
                .ForMember(dest => dest.Req_ApplicantData, src => src.MapFrom(src => src.PersonelData))
                .ForMember(dest => dest.Req_Code, src => src.MapFrom(src => src.CodeGenerate))

                //.ForMember(dest => dest.Req_Trans, src => src.MapFrom(src => src.RequestTransactions))
                .ReverseMap();


            #region EFrom Answares
            CreateMap<EFormsAnswer, EformAnsDTO>()
                   .ForMember(dest => dest.EFAns_Value, src => src.MapFrom(src => src.Value))
                   .ForMember(dest => dest.EFAns_Value_EN, src => src.MapFrom(src => src.ValueEn))
                   .ForMember(dest => dest.EFAns_Q_ID, src => src.MapFrom(src => src.QuestionId))
                   .ForMember(dest => dest.EFAns_EF_ID, src => src.MapFrom(src => src.EformId))
                   .ForMember(dest => dest.EFAns_TableCol, src => src.MapFrom(src => src.PreviewTableCols))
                   .ReverseMap();

            CreateMap<PreviewTableCol, Preview_TableColsDTO>()
                //.ForMember(dest => dest.TC_ID, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.TC_Answare, src => src.MapFrom(src => src.TablesAnswares))
                .ReverseMap();


            CreateMap<TablesAnsware, Tables_AnswareDTO>()
                .ForMember(dest => dest.TAns_Row, src => src.MapFrom(src => src.Row))
                .ForMember(dest => dest.TAns_Row, src => src.MapFrom(src => src.Value))
                .ReverseMap();
            #endregion

            #region SaveReq
            CreateMap<SaveReq_PersonalDataDTO, PersonalDataDTO>()
                .ForMember(dest => dest.PD_ID, src => src.MapFrom(src => src.Personel_Data_ID))
                .ForMember(dest => dest.PD_IAUNumber, src => src.MapFrom(src => src.IAU_ID_Number))
                .ForMember(dest => dest.PD_TitleNames_ID, src => src.MapFrom(src => src.Title_Middle_Names_ID))
                .ForMember(dest => dest.PD_F_Name, src => src.MapFrom(src => src.First_Name))
                .ForMember(dest => dest.PD_M_Name, src => src.MapFrom(src => src.Middle_Name))
                .ForMember(dest => dest.PD_L_Name, src => src.MapFrom(src => src.Last_Name))
                .ForMember(dest => dest.PD_National_ID, src => src.MapFrom(src => src.Nationality_ID))
                .ForMember(dest => dest.PD_ID_Doc_ID, src => src.MapFrom(src => src.ID_Document))
                .ForMember(dest => dest.PD_ID_Number, src => src.MapFrom(src => src.ID_Number))
                .ForMember(dest => dest.PD_C_ID, src => src.MapFrom(src => src.Country_ID))
                .ForMember(dest => dest.PD_Address_C_ID, src => src.MapFrom(src => src.Address_CountryID))
                .ForMember(dest => dest.PD_Address_City_ID, src => src.MapFrom(src => src.Address_CityID))
                .ForMember(dest => dest.PD_Adress_R_ID, src => src.MapFrom(src => src.Adress_RegionID))
                .ForMember(dest => dest.PD_Address_City, src => src.MapFrom(src => src.Address_City))
                .ForMember(dest => dest.PD_Adress_Region, src => src.MapFrom(src => src.Adress_Region))
                .ForMember(dest => dest.PD_Postal, src => src.MapFrom(src => src.Postal_Code))
                .ForMember(dest => dest.PD_mail, src => src.MapFrom(src => src.Email))
                .ForMember(dest => dest.PD_Phone, src => src.MapFrom(src => src.Mobile))
                //.ForMember(dest => dest.PD_EFormAnswer, src => src.MapFrom(src => src.))
                .ReverseMap();

            CreateMap<SaveReq_E_Forms_Answer, EformAnsDTO>()
                   .ForMember(dest => dest.EFAns_Value, src => src.MapFrom(src => src.Value))
                   .ForMember(dest => dest.EFAns_Value_EN, src => src.MapFrom(src => src.Value_En))
                   .ForMember(dest => dest.EFAns_Q_ID, src => src.MapFrom(src => src.Question_ID))
                   .ForMember(dest => dest.EFAns_EF_ID, src => src.MapFrom(src => src.EForm_ID))
                   .ForMember(dest => dest.EFAns_TableCol, src => src.MapFrom(src => src.Preview_TableCols))
                   .ReverseMap();

            CreateMap<SaveReq_Preview_TableCols, Preview_TableColsDTO>()
                //.ForMember(dest => dest.TC_ID, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.TC_Answare, src => src.MapFrom(src => src.Tables_Answare))
                .ReverseMap();


            CreateMap<SaveReq_Tables_Answare, Tables_AnswareDTO>()
                .ForMember(dest => dest.TAns_Row, src => src.MapFrom(src => src.Row))
                .ForMember(dest => dest.TAns_Row, src => src.MapFrom(src => src.Value))
                .ReverseMap();


            CreateMap<SaveReq_RequestDTO, RequestDTO>()
                .ForMember(dest => dest.Req_ID, src => src.MapFrom(src => src.Request_Data_ID))
                .ForMember(dest => dest.Req_U_ID, src => src.MapFrom(src => src.Unit_ID))
                .ForMember(dest => dest.Req_SS_ID, src => src.MapFrom(src => src.Sub_Services_ID))
                .ForMember(dest => dest.Req_Notes, src => src.MapFrom(src => src.Required_Fields_Notes))
                .ForMember(dest => dest.Req_S_ID, src => src.MapFrom(src => src.Service_Type_ID))
                .ForMember(dest => dest.Req_R_ID, src => src.MapFrom(src => src.Request_Type_ID))
                .ForMember(dest => dest.Req_Is_Mos, src => src.MapFrom(src => src.IsTwasul_OC))
                //.ForMember(dest => dest.Req_Status, src => src.MapFrom(src => src.Request_State_ID))
                .ForMember(dest => dest.Req_ApplicantData, src => src.MapFrom(src => src.Personel_Data))
                .ForMember(dest => dest.Req_Code, src => src.MapFrom(src => src.Code_Generate))
                //.ForMember(dest => dest.Req_Trans, src => src.MapFrom(src => src.RequestTransactions))
                .ReverseMap();
            #endregion
        }
    }
}
