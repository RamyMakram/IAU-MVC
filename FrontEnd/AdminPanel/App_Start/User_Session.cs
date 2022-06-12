using IAUAdmin.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Script.Serialization;
using IAUAdmin.DTO.Helper;
namespace AdminPanel
{

    public sealed class User_Session
    {
        //use lazy keyword
        private static readonly Lazy<User_Session> Current = new Lazy<User_Session>(() => new User_Session());
        #region User
        public int UserId { get; set; }
 
        public int GroupId { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
        #endregion

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

        public static int LogIn(CustomUserLogin user)
        {
            try
            {
                #region From API
                var res = APIHandeling.Post("Login_out", user);
                var lst = res.Content.ReadAsAsync<Dictionary<string, object>>().Result;
                int StatusCode = int.Parse(lst["state_Code"].ToString());
                switch (StatusCode)
                {
                    case 1://Valid
                        {
                            JavaScriptSerializer ser = new JavaScriptSerializer();
                            var UserdataPlace = ser.Deserialize<CustomUserLogin>(lst["obj"].ToString());

                            User_Session Current = User_Session.GetInstance;
                            HttpContext.Current.Session["UserId"] = UserdataPlace.Emp_ID;
                            Current.UserId = UserdataPlace.Emp_ID;
                            Current.FullName = UserdataPlace.FullName;
                            Current.Token = UserdataPlace.EmpToken;
                            Set_Language(true);
                        }
                        break;
                    case 2://InvalidCredential
                        break;
                    case 5://LoginFromAnotherDevice

                        break;
                }
                return StatusCode;
                #endregion
            }
            catch //(Exception ex)
            {

                return (int)GlobalEnum.User_Login.ErrorHappened;
            }
        }

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

        public void LogOut()
        {
            try
            {
                User_Session Current = User_Session.GetInstance;

                #region From API
                Dictionary<string, string> dt = new Dictionary<string, string>();
                dt["Token"] = Current.Token;
                var res = APIHandeling.Put("Login_out", dt);
                #endregion
                Current = null;
            }
            catch
            {
                //plant_db.Sp_plant_Error_Insert("User_Session", ex.Message, "logout");
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
                //plant_db.Sp_plant_Error_Insert("User_Session", ex.Message, "clearsession");
            }

        }
    }
}