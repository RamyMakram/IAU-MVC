using AutoMapper;
using IAU.DTO.Entity;
using IAU_BackEnd.Models;

namespace IAU_BackEnd

{
    public class ApplicationProfile : AutoMapper.Profile
    {
        public ApplicationProfile()
        {
            var config = new MapperConfiguration(cfg =>
            {
                CreateMap<Main_Services, Main_Service_DTO>().ReverseMap();
            });
            config.AssertConfigurationIsValid();
        }
    }
}