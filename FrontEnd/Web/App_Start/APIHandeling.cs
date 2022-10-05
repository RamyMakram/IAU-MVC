using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Configuration;
using System.Net;
using System.Web;
using Web.Models;

namespace Web.App_Start
{
    public static class APIHandeling
    {
        /// <summary>
        /// Get Data From API
        /// </summary>
        /// <param name="apiName">contoller name </param>//
        /// <returns></returns>
        public static HttpResponseMessage getData(string apiName)
        {
            HttpClient h = new HttpClient();

            var db = new TasahelEntities();

            var domain = HttpContext.Current.Request.Url.Authority;

            var domainName = db.SubDomains.Where(q => q.Domain == domain).Select(q => q.Domain1.SubDomains.FirstOrDefault(s => s.Key == "BE_Mos")).FirstOrDefault();

            h.BaseAddress = new Uri(domainName.UseHttps ? "https://" + domainName.Domain : "http://" + domainName.Domain);

            h.DefaultRequestHeaders.Add("IsTwasul_OC", "true");
            h.DefaultRequestHeaders.Add("crd", "dkvkk45523g25dedks44w7ee7@k309m$.f,dkks");

            var res = h.GetAsync("/api/" + apiName);
            return res.Result;
        }
        public static HttpResponseMessage getDataAdmin(string apiName)
        {
            var db = new TasahelEntities();

            var domain = HttpContext.Current.Request.Url.Authority;

            var domainName = db.SubDomains.Where(q => q.Domain == domain).Select(q => q.Domain1.SubDomains.FirstOrDefault(s => s.Key == "BE_Admin")).FirstOrDefault();

            HttpClient h = new HttpClient();

            h.BaseAddress = new Uri(domainName.UseHttps ? "https://" + domainName.Domain : "http://" + domainName.Domain);

            h.DefaultRequestHeaders.Add("IsTwasul_OC", "true");
            h.DefaultRequestHeaders.Add("crd", "dkvkk45523g2ejieiisncbgey@jn#Wuhuhe6&&*bhjbde4w7ee7@k309m$.f,dkks");

            var res = h.GetAsync("/api/" + apiName);
            return res.Result;
        }
        /// <summary>
        /// Get Data From API with paramter
        /// </summary>
        /// <param name="apiName">contoller name </param>
        /// <param name="dic_data">dictionary data that will carry data key->varaible Name , value-> variable Value</param>
        /// <returns></returns>
        public static HttpResponseMessage getDataByParam<dt>(string apiName, Dictionary<string, dt> dic_data)
        {
            HttpClient h = new HttpClient();

            var db = new TasahelEntities();

            var domain = HttpContext.Current.Request.Url.Authority;

            var domainName = db.SubDomains.Where(q => q.Domain == domain).Select(q => q.Domain1.SubDomains.FirstOrDefault(s => s.Key == "BE_Mos")).FirstOrDefault();

            h.BaseAddress = new Uri(domainName.UseHttps ? "https://" + domainName.Domain : "http://" + domainName.Domain);

            h.DefaultRequestHeaders.Add("lang", User_Session.GetInstance.Language_IsAr.ToString());
            h.DefaultRequestHeaders.Add("crd", "dkvkk45523g25dedks44w7ee7@k309m$.f,dkks");
            dynamic res = new HttpResponseMessage();

            int pageSize = 0; int index = 0; Int64 FarmId = 0;
            if (dic_data.ContainsKey("pageSize") || dic_data.ContainsKey("FarmId"))
            {
                pageSize = int.Parse(dic_data["pageSize"].ToString());
                index = int.Parse(dic_data["index"].ToString());
                FarmId = int.Parse(dic_data["FarmId"].ToString());
                dic_data.Remove("pageSize");
                dic_data.Remove("index");
                dic_data.Remove("FarmId");

                if (dic_data.Any(pair => pair.Value != null && (pair.Value.ToString().Length != 0)))
                {
                    //Type valueType = dic_data.GetType().GetGenericArguments()[1];
                    res = h.GetAsync("/api/" + apiName + convert_DicToString(dic_data) + "&pageSize=" + pageSize + "&index=" + index + "&FarmId=" + FarmId).Result;
                }
                else
                {
                    res = h.GetAsync("/api/" + apiName + "?pageSize=" + pageSize + "&index=" + index + "&FarmId=" + FarmId).Result;
                }
            }
            else
            {
                if (dic_data.Any(pair => pair.Value != null && (pair.Value.ToString().Length != 0)))
                {
                    //Type valueType = dic_data.GetType().GetGenericArguments()[1];
                    res = h.GetAsync("/api/" + apiName + convert_DicToString(dic_data)).Result;
                }
                else
                {
                    res = h.GetAsync("/api/" + apiName).Result;
                }
            }
            return res;
        }
        /// <summary>
        /// UPDATE using API
        /// </summary>
        /// <typeparam name="dt">custom datatype to accept any datatype</typeparam>
        /// <param name="apiName">controller name</param>
        /// <param name="obj">model to be updated</param>
        /// <returns></returns>
        public static HttpResponseMessage Post<dt>(string apiName, dt obj)
        {
            var db = new TasahelEntities();

            var domain = HttpContext.Current.Request.Url.Authority;

            var domainName = db.SubDomains.Where(q => q.Domain == domain).Select(q => q.Domain1.SubDomains.FirstOrDefault(s => s.Key == "BE_Mos")).FirstOrDefault();

            //Insert
            HttpClient h = new HttpClient();
            h.BaseAddress = new Uri(domainName.UseHttps ? "https://" + domainName.Domain : "http://" + domainName.Domain);

            h.DefaultRequestHeaders.Add("lang", User_Session.GetInstance.Language_IsAr.ToString());
            h.DefaultRequestHeaders.Add("IsTwasul_OC", "true");
            h.DefaultRequestHeaders.Add("crd", "dkvkk45523g25dedks44w7ee7@k309m$.f,dkks");
            var res = h.PostAsJsonAsync("/api/" + apiName, obj).Result;
            return res;
        }
        public static HttpResponseMessage PostRequest<dt>(string apiName, dt obj)
        {
            var db = new TasahelEntities();

            var domain = HttpContext.Current.Request.Url.Authority;

            var domainName = db.SubDomains.Where(q => q.Domain == domain).Select(q => q.Domain1.SubDomains.FirstOrDefault(s => s.Key == "BE_Admin")).FirstOrDefault();

            //Insert
            HttpClient h = new HttpClient();
            h.BaseAddress = new Uri(domainName.UseHttps ? "https://" + domainName.Domain : "http://" + domainName.Domain);

            h.DefaultRequestHeaders.Add("lang", User_Session.GetInstance.Language_IsAr.ToString());
            h.DefaultRequestHeaders.Add("IsTwasul_OC", "true");
            h.DefaultRequestHeaders.Add("crd", "dkvkk45523g2ejieiisncbgey@jn#Wuhuhe6&&*bhjbde4w7ee7@k309m$.f,dkks");
            var res = h.PostAsJsonAsync("/api/" + apiName, obj).Result;
            return res;
        }

        public static async System.Threading.Tasks.Task<HttpResponseMessage> LoginAdminAsync(string apiName)
        {

            var db = new TasahelEntities();

            var domain = HttpContext.Current.Request.Url.Authority;

            var domainName = db.SubDomains.Where(q => q.Domain == domain).Select(q => q.Domain1.SubDomains.FirstOrDefault(s => s.Key == "BE_Admin")).FirstOrDefault();

            //Insert
            HttpClient h = new HttpClient();
            h.BaseAddress = new Uri(domainName.UseHttps ? "https://" + domainName.Domain : "http://" + domainName.Domain);
            h.DefaultRequestHeaders.Add("crd", "dkvkk45523g2ejieiisncbgey@jn#Wuhuhe6&&*bhjbde4w7ee7@k309m$.f,dkks");
            h.DefaultRequestHeaders.Add("IsTwasul_OC", "true");
            System.Net.ServicePointManager.SecurityProtocol |=
                SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var res = await h.GetAsync("/api/" + apiName);
            return res;
        }
        //*********************************************//
        private static string convert_DicToString<dt>(Dictionary<string, dt> dic_data)
        {
            StringBuilder str = new StringBuilder();
            str.Append("?");
            foreach (var item in dic_data)
            {
                str.Append(item.Key + "=" + (string.IsNullOrEmpty(Convert.ToString(item.Value)) ? "%7F" : Convert.ToString(item.Value)) + "&");
            }
            str.Remove(str.Length - 1, 1);
            return str.ToString();
        }

        internal static string AdminURL()
        {
            var db = new TasahelEntities();

            var domain = HttpContext.Current.Request.Url.Authority;

            var domainName = db.SubDomains.Where(q => q.Domain == domain).Select(q => q.Domain1.SubDomains.FirstOrDefault(s => s.Key == "BE_Admin")).FirstOrDefault();
            return domainName.UseHttps ? "https://" + domainName.Domain : "http://" + domainName.Domain;
        }
    }

}