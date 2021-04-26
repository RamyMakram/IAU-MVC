using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using System.Configuration;

namespace Web.App_Start
{
	public static class APIHandeling
	{
		static string domainName = ConfigurationManager.AppSettings["DomainName"].ToString();
		static string AdminURL = ConfigurationManager.AppSettings["AdminUrl"].ToString();
		/// <summary>
		/// Get Data From API
		/// </summary>
		/// <param name="apiName">contoller name </param>//
		/// <returns></returns>
		public static HttpResponseMessage getData(string apiName, string lang)
		{
			HttpClient h = new HttpClient();

			h.BaseAddress = new Uri(domainName);

			h.DefaultRequestHeaders.Add("lang", lang);
			h.DefaultRequestHeaders.Add("IsTwasul_OC", "true");

			var res = h.GetAsync("/api/" + apiName + "?lang=" + lang).Result;
			return res;
		}
		/// <summary>
		/// Get Data From API with paramter
		/// </summary>
		/// <param name="apiName">contoller name </param>
		/// <param name="dic_data">dictionary data that will carry data key->varaible Name , value-> variable Value</param>
		/// <returns></returns>
		public static HttpResponseMessage getDataByParamter<dt>(string apiName, Dictionary<string, dt> dic_data, string lang)
		{
			HttpClient h = new HttpClient();

			h.BaseAddress = new Uri(domainName);
			h.DefaultRequestHeaders.Add("lang", lang);
			h.DefaultRequestHeaders.Add("IsTwasul_OC", "true");
			dynamic res = new HttpResponseMessage();

			int pageSize = 0; int index = 0;

			if (dic_data.ContainsKey("pageSize"))
			{
				pageSize = int.Parse(dic_data["pageSize"].ToString());
				index = int.Parse(dic_data["index"].ToString());
				dic_data.Remove("pageSize");
				dic_data.Remove("index");

				if (dic_data.Any(pair => pair.Value != null && (pair.Value.ToString().Length != 0)))
				{
					//Type valueType = dic_data.GetType().GetGenericArguments()[1];
					res = h.GetAsync("/api/" + apiName + convert_DicToString(dic_data) + "&pageSize=" + pageSize + "&index=" + index).Result;
				}
				else
				{
					res = h.GetAsync("/api/" + apiName + "?pageSize=" + pageSize + "&index=" + index).Result;
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
		public static HttpResponseMessage getDataByParam<dt>(string apiName, Dictionary<string, dt> dic_data)
		{
			HttpClient h = new HttpClient();

			h.BaseAddress = new Uri(domainName);
			h.DefaultRequestHeaders.Add("lang", User_Session.GetInstance.Language_IsAr.ToString());

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
			//Insert
			HttpClient h = new HttpClient();
			h.DefaultRequestHeaders.Add("lang", User_Session.GetInstance.Language_IsAr.ToString());
			h.DefaultRequestHeaders.Add("IsTwasul_OC", "true");
			h.BaseAddress = new Uri(domainName);

			var res = h.PostAsJsonAsync("/api/" + apiName, obj).Result;
			return res;
		}

		public static HttpResponseMessage LoginAdmin(string apiName)
		{
			//Insert
			HttpClient h = new HttpClient();
			h.BaseAddress = new Uri(AdminURL);

			var res = h.GetAsync("/api/" + apiName).Result;
			return res;
		}
		/// <summary>
		/// INSERT using API
		/// </summary>
		/// <typeparam name="dt">custom datatype to accept any datatype</typeparam>
		/// <param name="apiName">controller name</param>
		/// <param name="obj">model to be updated</param>
		/// <returns></returns>
		public static HttpResponseMessage Put<dt>(string apiName, dt obj)
		{
			//UPDATE
			HttpClient h = new HttpClient();
			h.DefaultRequestHeaders.Add("lang", User_Session.GetInstance.Language_IsAr.ToString());
			h.DefaultRequestHeaders.Add("IsTwasul_OC", "true");
			h.BaseAddress = new Uri(domainName);

			var res = h.PutAsJsonAsync("/api/" + apiName, obj).Result;
			return res;
		}

		/// <summary>
		/// DELETE using API
		/// </summary>
		/// <typeparam name="dt">custom datatype to accept any datatype</typeparam>
		/// <param name="apiName">controller name</param>
		/// <param name="obj">model to be deleted</param>
		/// <returns></returns>
		public static HttpResponseMessage Delete<dt>(string apiName, dt obj)
		{
			//DELETE
			HttpClient h = new HttpClient();
			h.DefaultRequestHeaders.Add("lang", User_Session.GetInstance.Language_IsAr.ToString());
			h.DefaultRequestHeaders.Add("IsTwasul_OC", "true");
			h.BaseAddress = new Uri(domainName);

			var res = h.PutAsJsonAsync("/api/" + apiName + "?delete=1", obj).Result;
			return res;
		}
		/// <summary>
		/// Check if Exist in DB
		/// </summary>
		/// <typeparam name="dt">custom datatype to accept any datatype</typeparam>
		/// <param name="apiName">controller name</param>
		/// <param name="obj">model to search for</param>
		/// <returns></returns>
		public static HttpResponseMessage CheckExist<dt>(string apiName, dt obj)
		{
			//DELETE
			HttpClient h = new HttpClient();
			h.DefaultRequestHeaders.Add("lang", User_Session.GetInstance.Language_IsAr.ToString());
			h.DefaultRequestHeaders.Add("IsTwasul_OC", "true");
			h.BaseAddress = new Uri(domainName);

			var res = h.PutAsJsonAsync("/api/" + apiName + "?editCheck=1", obj).Result;
			return res;
		}
		public static HttpResponseMessage Insert_Error(string pageName, string errorMessage, string functionName)
		{
			//Insert
			HttpClient h = new HttpClient();
			h.DefaultRequestHeaders.Add("lang", User_Session.GetInstance.Language_IsAr.ToString());
			h.DefaultRequestHeaders.Add("IsTwasul_OC", "true");
			h.BaseAddress = new Uri(domainName);
			Dictionary<string, string> dic_data = new Dictionary<string, string>();
			dic_data.Add("PageName", pageName);
			dic_data.Add("ErrorMessage", errorMessage);
			dic_data.Add("FunctionName", functionName);
			dic_data.Add("Date", DateTime.Now.ToLongDateString());

			var res = h.PostAsJsonAsync("/api/ErrorDisplay", dic_data).Result;

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
		////////////////// Remote procedure call /////////////////////////

		/// <summary>
		/// Get Data From API By Rpc
		/// </summary>
		/// <param name="apiName"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		//public static HttpResponseMessage getData(string apiName, string action)
		//{
		//	HttpClient h = new HttpClient();
		//	h.DefaultRequestHeaders.Add("lang", User_Session.GetInstance.Language_IsAr.ToString());
		//	h.DefaultRequestHeaders.Add("IsTwasul_OC", "true");
		//	h.BaseAddress = new Uri(domainName);
		//	h.DefaultRequestHeaders.Add("lang", User_Session.GetInstance.Language_IsAr.ToString());

		//	var res = h.GetAsync("/Rpc/" + apiName + "/" + action).Result;
		//	return res;
		//}

		//public static HttpResponseMessage getData(string apiName, string action, params object[] obj)
		//{
		//	HttpClient h = new HttpClient();
		//	h.DefaultRequestHeaders.Add("lang", User_Session.GetInstance.Language_IsAr.ToString());

		//	h.BaseAddress = new Uri(domainName);

		//	var res = h.GetAsync("/Rpc/" + apiName + "/" + action + obj).Result;
		//	return res;
		//}


	}

}