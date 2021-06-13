using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IAUBackEnd.Admin
{
	public class Helper
	{
		public static DateTime GetDate()
		{
			return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"));
		}
	}
}