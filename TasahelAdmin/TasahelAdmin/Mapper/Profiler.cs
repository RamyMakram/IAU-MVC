using AutoMapper;
using TasahelAdmin.Models;
using TasahelAdmin.Models.VM;

namespace TasahelAdmin.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DomainStyleVM, DomainStyle>().ReverseMap();
            CreateMap<HomeVM, TasahelHomeSetting>().ReverseMap();
            CreateMap<AboutVM, About>().ReverseMap();

            CreateMap<DomainCreateVM, Domain>()
                .ForMember(dest => dest.Name, src => src.MapFrom(src => src.Name))
                .ForMember(dest => dest.Domain1, src => src.MapFrom(src => src.Domain1))
                .ForMember(dest => dest.ConnectionString, src => src.MapFrom(src => src.ConnectionString))
                .ForMember(dest => dest.Enabled, src => src.MapFrom(src => src.Enabled))
                .ForMember(dest => dest.EndDate, src => src.MapFrom(src => src.EndDate))

                .ForMember(dest => dest.DomainStyle, src => src.MapFrom(src => src.DomainStyle))
                .ForMember(dest => dest.TasahelHomeSetting, src => src.MapFrom(src => src.HomeSettings))
                .ForMember(dest => dest.DomainInfo, src => src.MapFrom(src => src.DomainInfo))
                .ForMember(dest => dest.DomainEmail, src => src.MapFrom(src => src.DomainEmail))
                .ForMember(dest => dest.Abouts, src => src.MapFrom(src => src.Abouts))
                .ForMember(dest => dest.SubDomains, src => src.MapFrom(src => src.SubDomains))
                //.ForMember(dest => dest.DomainInfo.Phone, src => src.MapFrom(src => src.Phone))
                //.ForMember(dest => dest.DomainInfo.Mail, src => src.MapFrom(src => src.Mail))


                //.ForMember(dest => dest.DomainStyle.Favicon, src => src.MapFrom(src => src.Img_FavIco))
                //.ForMember(dest => dest.DomainStyle.Icon, src => src.MapFrom(src => src.Img_Ico))
                //.ForMember(dest => dest.DomainStyle.Title, src => src.MapFrom(src => src.Title))
                //.ForMember(dest => dest.DomainStyle.MetaDesc, src => src.MapFrom(src => src.MetaDesc))
                //.ForMember(dest => dest.DomainStyle.MetaKeyword, src => src.MapFrom(src => src.MetaKeyword))
                //.ForMember(dest => dest.DomainStyle.Maincolor, src => src.MapFrom(src => src.Maincolor))
                //.ForMember(dest => dest.DomainStyle.Secondcolor, src => src.MapFrom(src => src.Secondcolor))
                //.ForMember(dest => dest.DomainStyle.Thirdcolor, src => src.MapFrom(src => src.Thirdcolor))

                //.ForMember(dest => dest.DomainEmail.Name, src => src.MapFrom(src => src.Name))
                //.ForMember(dest => dest.DomainEmail.Smtp, src => src.MapFrom(src => src.Smtp))
                //.ForMember(dest => dest.DomainEmail.Port, src => src.MapFrom(src => src.Port))
                //.ForMember(dest => dest.DomainEmail.Email, src => src.MapFrom(src => src.Email))
                //.ForMember(dest => dest.DomainEmail.UserName, src => src.MapFrom(src => src.UserName))
                //.ForMember(dest => dest.DomainEmail.Password, src => src.MapFrom(src => src.Password))
                //.ForMember(dest => dest.DomainEmail.MessageAppSid, src => src.MapFrom(src => src.MessageAppSid))
                //.ForMember(dest => dest.DomainEmail.Sender, src => src.MapFrom(src => src.Sender))
                //.ForMember(dest => dest.DomainEmail.UseMessages, src => src.MapFrom(src => src.UseMessages))


                //.ForMember(dest => dest.TasahelHomeSetting.NewReqIco, src => src.MapFrom(src => src.Img_NewReqICo))
                //.ForMember(dest => dest.TasahelHomeSetting.FollowIco, src => src.MapFrom(src => src.Img_FollowIco))
                //.ForMember(dest => dest.TasahelHomeSetting.EnableAcadamic, src => src.MapFrom(src => src.EnableAcadmic))
                .ReverseMap();

        }
    }
}
