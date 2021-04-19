
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Script.Serialization;

namespace Web.App_Start
{

	public sealed class User_Session
	{
		//use lazy keyword
		private static readonly Lazy<User_Session> Current = new Lazy<User_Session>(() => new User_Session());


		public static User_Session GetInstance
		{
			get
			{
				#region Lazy
				return Current.Value;
				#endregion
			}
		}

		/// <summary>
		/// For Language ar-Eg->Ar , En-us->English
		/// </summary>
		public string Language { get; set; }

		public byte Language_IsAr { get; set; }


		public static void Set_Language(bool is_Arabic = true)
		{
			User_Session Current = User_Session.GetInstance;
			if (is_Arabic)
			{
				Current.Language = "ar-Eg";
				Current.Language_IsAr = 1;
			}
			else
			{
				Current.Language = "en-Us";
				Current.Language_IsAr = 0;
			}
		}

		private void ClearSession()
		{
			try
			{
				User_Session Current = User_Session.GetInstance;
			}
			catch
			{
			}
		}
	}
}