using IAUAdmin.DTO.Entity;
using IAUBackEnd.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace IAUBackEnd.Admin
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

			config.EnableCors();
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{action}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
			 //mapperConfig = new MapperConfiguration(cfg =>
				//	cfg.CreateMap<Job, User_Permissions_Type>()
				//	.ForMember(dest => dest.User_Permissions_Type_ID, act => act.MapFrom(src => src.ID))
				//	.ForMember(dest => dest.User_Permissions_Type_Name_AR, act => act.MapFrom(src => src.Name_AR))
				//	.ForMember(dest => dest.User_Permissions_Type_Name_EN, act => act.MapFrom(src => src.Name_EN))
				//	.ForMember(dest => dest.User_Permissions, act => act.MapFrom(src => src.Permissions))
				//	.ForMember(dest => dest.Users, act => act.MapFrom(src => src.Users))
					
					
				//);
		}
	}
}
