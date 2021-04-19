using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
	public class UserController : BaseController
	{
		// GET: User
		public ActionResult Home()
		{
			var Data = APIHandeling.getData("User/GetAll");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<ICollection<UserDTO>>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Detials(int id)
		{
			var Data = APIHandeling.getData("User/GetDetails?uid=" + id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return View(JsonConvert.DeserializeObject<UserDTO>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Create()
		{
			var Data = APIHandeling.getData("Job/GetAllJobs");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			var Jobs = JsonConvert.DeserializeObject<ICollection<JobDTO>>(res.result.ToString());
			if (Request.Cookies["lang"] == null || Request.Cookies["lang"].Value == "ar")
				ViewBag.Jobs = new SelectList(Jobs, "User_Permissions_Type_ID", "User_Permissions_Type_Name_AR");
			else
				ViewBag.Jobs = new SelectList(Jobs, "User_Permissions_Type_ID", "User_Permissions_Type_Name_EN");
			return View();
		}
		public ActionResult Edit(int id)
		{
			var Data = APIHandeling.getData("Job/GetAllJobs");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			ViewBag.Permissions = JsonConvert.DeserializeObject<ICollection<JobDTO>>(res.result.ToString());

			Data = APIHandeling.getData("User/GetDetails?uid=" + id);
			resJson = Data.Content.ReadAsStringAsync();
			res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return View(JsonConvert.DeserializeObject<UserDTO>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		[HttpPost]
		public ActionResult Edit(int Id, UserDTO user)
		{
			user.User_ID = Id;
			var Req = APIHandeling.Post("User/UpdateData", user);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Deactive(int id)
		{

			return View(new UserDTO() { User_ID = id });
		}

		[HttpPost]
		public ActionResult Deactive(int id, string x)
		{
			var Req = APIHandeling.getData("User/Deactive?uid=" + id);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}
		[HttpPost]
		public ActionResult Create(UserDTO user)
		{
			var Req = APIHandeling.Post("User/Create", user);
			var resJson = Req.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

			if (res.success)
				return RedirectToAction("Home");
			else
				return RedirectToAction("NotFound", "Error");
		}
	}
}