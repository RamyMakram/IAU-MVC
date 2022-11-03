using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.App_Start
{
    public class Helper
    {
        public static string GetImagePath()
        {
            var db = new TasahelEntities();

            var domain = HttpContext.Current.Request.Url.Authority;

            var domainName = db.SubDomains.Where(q => q.Domain == domain).Select(q => q.Domain1.SubDomains.FirstOrDefault(s => s.Key == "BE_Mos")).FirstOrDefault();

            var BaseAddress = domainName.UseHttps ? "https://" + domainName.Domain : "http://" + domainName.Domain;

            return BaseAddress;
        }
    }
}