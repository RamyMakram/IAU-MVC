using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    public class EmailController : BaseController
    {
        // GET: Email
        public ActionResult Home()
        {
            var isar = Request.Cookies["lang"] == null || Request.Cookies["lang"].Value == "ar";

            var Data = APIHandeling.getData("Service_Type/GetActive");
            var resJson = Data.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
            var Services = JsonConvert.DeserializeObject<ICollection<ServiceTypeDTO>>(res.result.ToString());
            ViewBag.ServiceType = Services;

            Data = APIHandeling.getData("Request_Type/GetActive");
            resJson = Data.Content.ReadAsStringAsync();
            res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
            var ReqTypes = JsonConvert.DeserializeObject<ICollection<RequestTypeDTO>>(res.result.ToString());
            ViewBag.ReqTypes = ReqTypes;


            Data = APIHandeling.getData("Request/GetRequests_Data?UserID=" + Request.Cookies["u"].Value);
            resJson = Data.Content.ReadAsStringAsync();
            res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);

            if (res.success)
                return View(JsonConvert.DeserializeObject<IEnumerable<ReqestDTO>>(res.result.ToString()));
            else
                return RedirectToAction("NotFound", "Error");
        }
        public ActionResult Preview(int id)
        {
            var isar = Request.Cookies["lang"].Value == "ar";
            var Data = APIHandeling.getData($"Request/GetRequest_Data?id={id}&UserID={Request.Cookies["u"].Value}");
            var resJson = Data.Content.ReadAsStringAsync();
            var Request_res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
            var IsMostafid = (TempData["IsMostafid"] as Nullable<bool>);
            if (Request_res.success)
            {
                Data = APIHandeling.getData($"Request/GetRequestTranscation?id={id}&UserID={Request.Cookies["u"].Value}");
                resJson = Data.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
                ViewBag.RequestTransaction = JsonConvert.DeserializeObject<ICollection<Request_TransactionDTO>>(res.result.ToString());
                var Req = JObject.Parse(Request_res.result.ToString());
                var RequestData = JsonConvert.DeserializeObject<ReqestDTO>(Req["request_Data"].ToString());
                //RequestData.Units = new UnitsDTO() { Building_Number = Req["Building_Number"].Value<string>(), Units_Location_ID = Req["Units_Location_ID"].Value<int>() };
                Data = APIHandeling.getData("Units/GetActiveForEmail");
                resJson = Data.Content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
                var Units = JsonConvert.DeserializeObject<ICollection<UnitsDTO>>(res.result.ToString());
                ViewBag.Units = Units;

                if (IsMostafid.HasValue && IsMostafid.Value)
                {
                    if ((RequestData.TempCode == "" || RequestData.TempCode == null))
                    {
                        bool count = true;
                        if (isar)
                            ViewBag.Source = new List<string> { "مستفيد", "تواصل" }.ConvertAll(e => { count = !count; return new SelectListItem() { Text = e, Value = count.ToString() }; });
                        else
                            ViewBag.Source = new List<string> { "Mostafid", "Twasol" }.ConvertAll(e => { count = !count; return new SelectListItem() { Text = e, Value = count.ToString() }; });

                        Data = APIHandeling.getData("UnitsLocation/GetActive");
                        resJson = Data.Content.ReadAsStringAsync();
                        res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
                        var Locations = JsonConvert.DeserializeObject<ICollection<UnitsLocDTO>>(res.result.ToString());
                        if (isar)
                            ViewBag.Locations = new SelectList(Locations, "Units_Location_ID", "Units_Location_Name_AR");
                        else
                            ViewBag.Locations = new SelectList(Locations, "Units_Location_ID", "Units_Location_Name_EN");

                        Data = APIHandeling.getData("Service_Type/GetActive");
                        resJson = Data.Content.ReadAsStringAsync();
                        res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
                        var Services = JsonConvert.DeserializeObject<ICollection<ServiceTypeDTO>>(res.result.ToString());
                        if (isar)
                            ViewBag.ServiceType = new SelectList(Services, "Service_Type_ID", "Service_Type_Name_AR");
                        else
                            ViewBag.ServiceType = new SelectList(Services, "Service_Type_ID", "Service_Type_Name_EN");
                        //= new List<string> { "REQUEST DISPATCHING", "REQUEST SORTING", "REQUEST PROCESSING", "REQUEST PROCESSING", "DELIVERED REQUEST PROCESSED" };

                        Data = APIHandeling.getData("Request_State/GetActive");
                        resJson = Data.Content.ReadAsStringAsync();
                        res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
                        ViewBag.Status = JsonConvert.DeserializeObject<ICollection<RequestStatusDTO>>(res.result.ToString());

                        Data = APIHandeling.getData("Request_Type/GetActive");
                        resJson = Data.Content.ReadAsStringAsync();
                        res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
                        var ReqTypes = JsonConvert.DeserializeObject<ICollection<RequestTypeDTO>>(res.result.ToString());
                        if (isar)
                            ViewBag.ReqTypes = new SelectList(ReqTypes, "Request_Type_ID", "Request_Type_Name_AR");
                        else
                            ViewBag.ReqTypes = new SelectList(ReqTypes, "Request_Type_ID", "Request_Type_Name_EN");
                    }
                    return View("Preview", RequestData);
                }
                else
                    return View("PreviewForUnit", RequestData);
            }
            else
                return RedirectToAction("NotFound", "Error");
        }
        public JsonResult GetRequesType(int ID)
        {
            var Data = APIHandeling.getData("Request_Type/GetActiveByMainService?SID=" + ID);
            var resJson = Data.Content.ReadAsStringAsync();
            return Json(resJson.Result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBuildings(int ID)
        {
            var Data = APIHandeling.getData("Units/GetUniqueBuildingByLoca?id=" + ID);
            var resJson = Data.Content.ReadAsStringAsync();
            return Json(resJson.Result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUnits(int SID, int RID, int? Loc, string Building)
        {
            var Data = APIHandeling.getData($"Units/GetActiveUnits_by?serviceType={SID}&Req={RID}&locid={Loc}&Build={Building}");
            var resJson = Data.Content.ReadAsStringAsync();
            return Json(resJson.Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Codeing(int RequestIID, bool IsTwasul_OC, int Service_Type_ID, int Request_Type_ID, int? locations, string BuildingSelect, int Unit_ID, string type)
        {
            var Data = APIHandeling.getData($"Request/Coding?RequestIID={RequestIID}&IsTwasul_OC={IsTwasul_OC}&Service_Type_ID={Service_Type_ID}&Request_Type_ID={Request_Type_ID}&locations={locations}&BuildingSelect={BuildingSelect}&Unit_ID={Unit_ID}&type={type}");
            var resJson = Data.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
            if (res.success)
                return RedirectToAction("Preview", new { id = RequestIID });
            else
                return RedirectToAction("Home");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Forward(int RequestIID, int Unit_ID, Nullable<DateTime> Expected, string MosComment)
        {
            var Data = APIHandeling.Post($"Request/Forward?RequestIID={RequestIID}&Unit_ID={Unit_ID}&Expected={Expected}", MosComment);
            var resJson = Data.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
            if (res.success)
                return RedirectToAction("Home");
            else
                return RedirectToAction("Preview", new { id = RequestIID });
        }
        public JsonResult GetCode(int RequestIID, bool IsTwasul_OC, int Service_Type_ID, int Request_Type_ID, int? locations, string BuildingSelect, int Unit_ID, string type)
        {
            var Data = APIHandeling.getData($"Request/GenrateCode?RequestIID={RequestIID}&IsTwasul_OC={IsTwasul_OC}&Service_Type_ID={Service_Type_ID}&Request_Type_ID={Request_Type_ID}&locations={locations}&BuildingSelect={BuildingSelect}&Unit_ID={Unit_ID}&type={type}");
            var resJson = Data.Content.ReadAsStringAsync();
            return Json(resJson.Result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult AddComment(int RequestIID, string Comment)
        {
            var Data = APIHandeling.Post($"Request/AddComment?UserID={Request.Cookies["u"].Value}&RequestID={RequestIID}&CommentType=1", Comment);
            var resJson = Data.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
            if (res.success)
                return RedirectToAction("Home");
            else
                return RedirectToAction("Preview", new { id = RequestIID });
        }
        [HttpPost]
        public ActionResult CloseComment(int RequestIID, string Comment)
        {
            var Data = APIHandeling.Post($"Request/AddComment?UserID={Request.Cookies["u"].Value}&RequestID={RequestIID}&CommentType=0", Comment);
            var resJson = Data.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
            if (res.success)
                return RedirectToAction("Home");
            else
                return RedirectToAction("Preview", new { id = RequestIID });
        }

        [HttpPost]
        public ActionResult CloseRequest(int RequestIID)
        {
            var Data = APIHandeling.Post($"Request/CloseRequest?UserID={Request.Cookies["u"].Value}&RequestID={RequestIID}", "");
            var resJson = Data.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
            if (res.success)
                return RedirectToAction("Home");
            else
                return RedirectToAction("Preview", new { id = RequestIID });
        }

        [HttpPost]
        public ActionResult Archive(string requests)
        {
            var Data = APIHandeling.Post($"Request/ArchiveRequests?UserID={Request.Cookies["u"].Value}", requests);
            var resJson = Data.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
            return RedirectToAction("Home");
        }

        [HttpPost]
        public JsonResult Filter(int? ST, int? RT, int? MT, DateTime? DT, DateTime? DF)
        {
            var Data = APIHandeling.getData($"Request/GetFilterdRequests_Data?RT={RT}&ST={ST}&MT={MT}&DF={DF}&DT={DT}&UserID=" + Request.Cookies["u"].Value);
            var resJson = Data.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
            if (res.success)
                return Json(res.result.ToString());
            else
                return Json("");
        }
        [HttpGet]
        public ActionResult GetEformRequest(int efid, int req)
        {
            try
            {
                var Data = APIHandeling.getData($"E_Forms/GetE_FormsFoRequest?id={efid}&RequestID={req}");
                var resJson = Data.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<ResponseClass>(resJson.Result);
                var model = JsonConvert.DeserializeObject<PersonEfDTO>(res.result.ToString());
                return PartialView("~/Views/Email/_Eform.cshtml", model);
            }
            catch (Exception ee)
            {
                return PartialView("~/Views/Email/_Eform.cshtml", null);

            }
        }
    }
}