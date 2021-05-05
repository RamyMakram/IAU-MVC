using IAUAdmin.DTO.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAUAdmin.DTO.Helper;
using System.Threading.Tasks;

namespace AdminPanel.Controllers
{
	public class JobsController : BaseController
	{
		// GET: Jobs
		public ActionResult Home()
		{
			var Data = APIHandeling.getData("Job/GetAllJobs");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<IEnumerable<JobDTO>>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Detials(int id)
		{
			var Data = APIHandeling.getData("Job/GetJob?jid=" + id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<JobDTO>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		public ActionResult Create()
		{
			var Data = APIHandeling.getData("Priviliges/GetAllAndSubPrivilges");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<JobDTO>(res.result.ToString()));
			else
				return RedirectToAction("DataError", "Error");
		}
		public ActionResult Edit(int id)
		{
			var Data = APIHandeling.getData("Job/GetJob?jid=" + id);
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<JobDTO>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}

		[HttpPost]
		public async Task<object> AddedPrivilges(string data)
		{
			var Data = JsonConvert.DeserializeObject<ICollection<Job_PermissionsDTO>>(data);
			var res = APIHandeling.Post("/Priviliges/AddPrivilgesToJob", Data);
			var resJson = res.Content.ReadAsStringAsync();
			var lst = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (lst.success)
				return JsonConvert.SerializeObject(new ResponseClass() { success = true });
			else
				return JsonConvert.SerializeObject(new ResponseClass() { success = false, result = lst.result });
		}

		[HttpPost]
		public async Task<object> RemovePrivilges(string data)
		{
			var Data = JsonConvert.DeserializeObject<ICollection<Job_PermissionsDTO>>(data);
			var res = APIHandeling.Post("/Priviliges/DeletePrivilgesFromJob", Data);
			var resJson = res.Content.ReadAsStringAsync();
			var lst = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (lst.success)
				return JsonConvert.SerializeObject(new ResponseClass() { success = true });
			else
				return JsonConvert.SerializeObject(new ResponseClass() { success = false, result = lst.result });
		}

		[HttpPost]
		public ActionResult Edit(int Id, JobDTO data)
		{
			try
			{
				data.User_Permissions_Type_ID = Id;
				var res = APIHandeling.Post("Job/EditJob", data);
				var resJson = res.Content.ReadAsStringAsync();
				var lst = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
				if (lst.success)
					return RedirectToAction("Home", "Jobs");
				else
					return RedirectToAction("Home", "Home");
			}
			catch (Exception ee)
			{
				return RedirectToAction("Saving", "Error");
			}
		}

		[HttpPost]
		public async Task<object> SaveJob(string Job)
		{
			try
			{
				var Data = JsonConvert.DeserializeObject<JobDTO>(Job);
				var res = APIHandeling.Post("Job/CreateJob", Data);
				var resJson = res.Content.ReadAsStringAsync();
				var lst = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
				if (lst.success)
					return JsonConvert.SerializeObject(new ResponseClass() { success = true });
				else
					return JsonConvert.SerializeObject(new ResponseClass() { success = false, result = lst.result });
			}
			catch (Exception ee)
			{
				return JsonConvert.SerializeObject(new ResponseClass() { success = false, result = ee });
			}
		}
		public ActionResult Deactive(int id)
		{
			return View();
		}
	}
}