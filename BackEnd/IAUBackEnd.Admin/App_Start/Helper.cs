using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace IAUBackEnd.Admin
{
	public class Helper
	{
		public static DateTime GetDate()
		{
			return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"));
		}

		public static byte[] GetHash(string inputString)
		{
			using (HashAlgorithm algorithm = SHA256.Create())
				return algorithm.ComputeHash(Encoding.ASCII.GetBytes(inputString));
		}

		public static string GetHashString(string inputString)
		{
			StringBuilder sb = new StringBuilder();
			var str = GetHash(inputString);
			foreach (byte b in str)
				sb.Append(b.ToString("X2"));

			return sb.ToString();
		}
	}
}