using IAUAdmin.DTO.Entity;
using IAUBackEnd.Admin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web.Http;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Text;
using System.Web.Hosting;
using Newtonsoft.Json;
using System.Threading.Tasks;

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
            //HostingEnvironment.QueueBackgroundWorkItem(async _ => { await InvokeMethod(); });
        }
        
    }
}
