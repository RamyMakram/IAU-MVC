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
    public class LevelsController : BaseController
    {
		public ActionResult Home()
		{
			var Data = APIHandeling.getData("UnitLevels/GetUnitLevel");
			var resJson = Data.Content.ReadAsStringAsync();
			var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
			if (res.success)
				return View(JsonConvert.DeserializeObject<ICollection<UnitLevelDTO>>(res.result.ToString()));
			else
				return RedirectToAction("NotFound", "Error");
		}
		//public ActionResult Create()
		//{
		//	return View();
		//}
		//public ActionResult Edit(int id)
		//{
		//	var Data = APIHandeling.getData("UnitLevels/GetUnitLevel?id=" + id);
		//	var resJson = Data.Content.ReadAsStringAsync();
		//	var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

		//	if (res.success)
		//		return View(JsonConvert.DeserializeObject<UnitLevelDTO>(res.result.ToString()));
		//	else
		//		return RedirectToAction("NotFound", "Error");
		//}
		//[HttpPost]
		//public ActionResult Edit(int Id, UnitLevelDTO loc)
		//{
		//	loc.ID = Id;
		//	loc.Units_Type = JsonConvert.DeserializeObject<ICollection<UnitTypeDTO>>(loc.Units_Type_STR);
		//	var Req = APIHandeling.Post("UnitLevels/Update", loc);
		//	var resJson = Req.Content.ReadAsStringAsync();
		//	var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

		//	if (res.success)
		//		return RedirectToAction("Home");
		//	else
		//		return RedirectToAction("NotFound", "Error");
		//}

		//[HttpPost]
		//public ActionResult Create(UnitLevelDTO user)
		//{
		//	user.Units_Type = JsonConvert.DeserializeObject<ICollection<UnitTypeDTO>>(user.Units_Type_STR);

		//	var Req = APIHandeling.Post("UnitLevels/Create", user);
		//	var resJson = Req.Content.ReadAsStringAsync();
		//	var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

		//	if (res.success)
		//		return RedirectToAction("Home");
		//	else
		//		return RedirectToAction("NotFound", "Error");
		//}
	}
}