using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace IAU.DTO.Helper
{
	public static class API_HelperFunctions
	{
		public static HttpStatusCode getStatusCode(int state_code)
		{
			switch (state_code)
			{
				case -1:
					return HttpStatusCode.InternalServerError;
				case 13:
					//multiple simultaneous updates.
					return HttpStatusCode.Conflict; //409 
				default:
					return HttpStatusCode.Accepted;
			}
		}

		public static List<string> Get_DeviceInfo()
		{
			#region fz


			string ipAddress = HttpContext.Current.Request.UserHostAddress;

			var request = HttpContext.Current.Request;
			string UserIp = ipAddress;
			string IsTwasul_OC = (request.UserAgent != null) ? (!(request.UserAgent.IndexOf("IsTwasul_OC", StringComparison.OrdinalIgnoreCase) >= 0)).ToString() : "true";
			var ss = request.Headers.GetValues("lang");
			string lang = request.Headers.GetValues("lang").FirstOrDefault() ?? "1";
			#endregion
			return new List<string> { UserIp, IsTwasul_OC, lang };
		}
	}
}