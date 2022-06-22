﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using IAUBackEnd.Admin.Models;
using LinqKit;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace IAUBackEnd.Admin.Controllers
{
    public class UnitsController : ApiController
    {
        private MostafidDBEntities db = new MostafidDBEntities();

        public async Task<IHttpActionResult> GetDeleted()
        {
            return Ok(new ResponseClass() { success = true, result = db.Units.Where(q => q.Deleted).Include(q => q.UnitServiceTypes).Include(q => q.Service_Type).Select(q => new { q.IS_Mostafid, q.DeletedAt, q.Service_Type, q.Units_ID, q.Units_Name_EN, q.Units_Name_AR, q.IS_Action, UnitServiceTypes = q.UnitServiceTypes.Select(w => new { w.ID, w.ServiceTypeID, w.Service_Type }) }) });
        }
        public async Task<IHttpActionResult> GetUnits()
        {
            return Ok(new ResponseClass() { success = true, result = db.Units.Where(q => !q.Deleted).Include(q => q.UnitServiceTypes).Include(q => q.Service_Type).Select(q => new { q.IS_Mostafid, q.Service_Type, q.Units_ID, q.Units_Name_EN, q.Units_Name_AR, q.IS_Action, UnitServiceTypes = q.UnitServiceTypes.Select(w => new { w.ID, w.ServiceTypeID, w.Service_Type }) }) });
        }

        public async Task<IHttpActionResult> GetUnitsByLevel(int lvlid)
        {
            return Ok(new ResponseClass() { success = true, result = db.Units.Where(q => !q.Deleted && q.LevelID == lvlid).Include(q => q.UnitServiceTypes).Include(q => q.Service_Type).Select(q => new { q.IS_Mostafid, q.Service_Type, q.Units_ID, q.Units_Name_EN, q.Units_Name_AR, q.IS_Action, UnitServiceTypes = q.UnitServiceTypes.Select(w => new { w.ID, w.ServiceTypeID, w.Service_Type }) }) });
        }
        
        public async Task<IHttpActionResult> GetUnitsByLocation(int locid)
        {
            return Ok(new ResponseClass() { success = true, result = db.Units.Where(q => !q.Deleted && q.Units_Location_ID == locid).Include(q => q.UnitServiceTypes).Include(q => q.Service_Type).Select(q => new { q.IS_Mostafid, q.Service_Type, q.Units_ID, q.Units_Name_EN, q.Units_Name_AR, q.IS_Action, UnitServiceTypes = q.UnitServiceTypes.Select(w => new { w.ID, w.ServiceTypeID, w.Service_Type }) }) });
        }
        public async Task<IHttpActionResult> GetUnitsByServiceType(int serviceType)
        {
            return Ok(new ResponseClass() { success = true, result = db.Units.Where(q => !q.Deleted && (q.ServiceTypeID == serviceType || q.UnitServiceTypes.Any(w => w.ServiceTypeID == serviceType))).Include(q => q.UnitServiceTypes).Include(q => q.Service_Type).Select(q => new { q.IS_Mostafid, q.Service_Type, q.Units_ID, q.Units_Name_EN, q.Units_Name_AR, q.IS_Action, UnitServiceTypes = q.UnitServiceTypes.Select(w => new { w.ID, w.ServiceTypeID, w.Service_Type }) }) });
        }

        public async Task<IHttpActionResult> GetActive()
        {
            return Ok(new ResponseClass() { success = true, result = db.Units.Where(q => q.IS_Action == true && !q.Deleted) });
        }
        public async Task<IHttpActionResult> GetActiveForEmail()
        {
            return Ok(new ResponseClass() { success = true, result = db.Units.Where(q => q.IS_Action == true && !q.Deleted && q.Users.Any(s => s.IS_Active == "1" && !s.Deleted && (!q.IS_Mostafid))) });
        }
        public async Task<IHttpActionResult> GetUniqueBuildingByLoca(int id)
        {
            return Ok(new ResponseClass() { success = true, result = db.Units.Where(q => q.IS_Action == true && q.Units_Location_ID == id && !q.Deleted).Select(q => q.Building_Number).Distinct() });
        }
        public async Task<IHttpActionResult> GetUnitSeginature(int id)
        {
            return Ok(new ResponseClass() { success = true, result = await db.Unit_Signature.FirstOrDefaultAsync(q => q.UnitID == id && !q.Deleted) });
        }
        [HttpPost]
        public async Task<IHttpActionResult> SaveUnitSeginature(IAUAdmin.DTO.Entity.Unit_Signature signature)
        {
            if (signature == null || signature.UnitID == 0 || signature.Base64 == "")
                return Ok(new ResponseClass() { success = false, result = "CheckEnter" });

            var path = HttpContext.Current.Server.MapPath("~");
            var FilePath = Path.Combine("Images", "UnitSignature", signature.UnitID + "_" + DateTime.Now.Ticks + ".png");

            var trans = db.Database.BeginTransaction();

            try
            {
                var Singature = await db.Unit_Signature.Include(q => q.Units).FirstOrDefaultAsync(q => q.UnitID == signature.UnitID && !q.Deleted);

                var OldVals = JsonConvert.SerializeObject(Singature, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                File.WriteAllBytes(Path.Combine(path, FilePath), Convert.FromBase64String(signature.Base64));

                if (db.Units.Find(signature.UnitID).Deleted)
                    return Ok(new ResponseClass() { success = false, result = "del" });

                var isCreateOperation = false;
                if (Singature == null)
                {
                    isCreateOperation = true;
                    Singature = new IAUBackEnd.Admin.Models.Unit_Signature { Date = Helper.GetDate(), Path = FilePath.Replace("\\", "/"), UnitID = signature.UnitID };
                    db.Unit_Signature.Add(Singature);
                }
                else
                    Singature.Path = FilePath.Replace("\\", "/");


                await db.SaveChangesAsync();
                var logstate = Logger.AddLog(db: db, logClass: LogClassType.UnitSignature, Method: isCreateOperation ? "Create" : "Update", Oldval: isCreateOperation ? null : OldVals, Newval: Singature, es: out _, syslog: out _, ID: Singature.UnitID, notes: "Update Unit Signature");
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    trans.Commit();
                    return Ok(new ResponseClass()
                    {
                        success = true
                    });
                }
                else
                {
                    trans.Rollback();
                    return Ok(new ResponseClass() { success = false });
                }
            }
            catch (Exception ee)
            {
                trans.Rollback();
                try
                {
                    if (File.Exists(Path.Combine(path, FilePath)))
                        File.Delete(Path.Combine(path, FilePath));
                }
                catch (Exception)
                {
                }
                return Ok(new ResponseClass() { success = false });
            }
        }
        public async Task<IHttpActionResult> GetActiveUnits_by(int serviceType, int Req, int? locid, string Build)
        {
            var publider = PredicateBuilder.New<Units>(q => !q.Deleted && (q.ServiceTypeID == serviceType || q.UnitServiceTypes.Any(w => w.ServiceTypeID == serviceType)) && q.Units_Request_Type.Any(w => w.Request_Type_ID == Req) && q.Users.Any(s => s.IS_Active == "1" && !q.Deleted) && (!q.IS_Mostafid));
            if (Build != "" && Build != "null" && Build != null)
                publider.And(q => q.Building_Number.Equals(Build));
            if (locid != null)
                publider.And(q => q.Units_Location_ID == locid);
            return Ok(new ResponseClass() { success = true, result = db.Units.Where(publider).Select(q => new { q.Units_Name_AR, q.Units_Name_EN, q.Units_ID, q.Building_Number, q.Units_Location_ID }) });
        }
        public async Task<IHttpActionResult> GetActiveUnits_byLevel(int id, int? uintId)
        {
            var pred = PredicateBuilder.New<Units>();
            pred.And(q => q.IS_Action == true && !q.Deleted && q.LevelID < id);
            if (uintId != null)
                pred.And(q => q.Units_ID != uintId && q.SubID != uintId);
            return Ok(new ResponseClass() { success = true, result = db.Units.Where(pred).Select(q => new { q.Units_Name_AR, q.Units_Name_EN, q.Units_ID }) });
        }

        public async Task<IHttpActionResult> GetUnits(int id)
        {
            var units = await db.Units.Where(q => q.Units_ID == id && !q.Deleted).Select(q => new { q.Units_Type, q.ServiceTypeID, q.UnitLevel, q.Code, q.Units_ID, q.Units_Name_AR, q.Units_Name_EN, q.Units_Location_ID, q.Units_Type_ID, q.Ref_Number, q.Building_Number, q.LevelID, q.SubID, q.IS_Action, q.IS_Mostafid, q.Units_Location, CanChangeLevel = q.Units1.Count == 0, Request_Type = q.Units_Request_Type.Select(w => new { w.Request_Type.Image_Path, w.Request_Type.Request_Type_Name_AR, w.Request_Type.Request_Type_Name_EN }), ServiceTypes = q.UnitServiceTypes.Select(w => new { Service_Type_ID = w.ServiceTypeID, w.Service_Type.Service_Type_Name_AR, w.Service_Type.Service_Type_Name_EN, w.Service_Type.Image_Path }), Units_Request_Type = q.Units_Request_Type.Select(s => new { s.Request_Type_ID, s.Units_ID, s.Units_Request_Type_ID }), MainServices = q.UnitMainServices.Select(w => new { w.Main_Services.Main_Services_ID, w.Main_Services.Main_Services_Name_AR, w.Main_Services.Main_Services_Name_EN }) }).FirstOrDefaultAsync();
            if (units == null)
                return Ok(new ResponseClass() { success = false, result = "Unit Is NULL" });

            return Ok(new ResponseClass()
            {
                success = true,
                result = new
                {
                    Unit = units,
                    Units_Types = db.Units_Type.Where(q => q.LevelID == units.LevelID).Select(q => new { q.Units_Type_ID, q.Units_Type_Name_AR, q.Units_Type_Name_EN }),
                    SubUnits = db.Units.Where(q => q.LevelID < units.LevelID && q.Units_ID != id).Select(q => new { q.Units_Name_AR, q.Units_Name_EN, q.Units_ID })
                }
            });
        }
        public async Task<IHttpActionResult> GetUnitsByID(int id)
        {
            var units = await db.Units.Where(q => q.Units_ID == id && !q.Deleted).Select(q => new { q.ServiceTypeID, q.Code, q.Units_ID, q.Units_Name_AR, q.Units_Name_EN, q.Units_Location_ID, q.Units_Type_ID, q.Ref_Number, q.Building_Number, q.LevelID, q.SubID, q.IS_Action, q.IS_Mostafid, q.Units_Type, q.Units_Location, Request_Type = q.Units_Request_Type.Select(w => new { w.Request_Type.Image_Path, w.Request_Type.Request_Type_Name_AR, w.Request_Type.Request_Type_Name_EN }), ServiceTypes = q.UnitServiceTypes.Select(w => new { Service_Type_ID = w.ServiceTypeID, w.Service_Type.Service_Type_Name_AR, w.Service_Type.Service_Type_Name_EN, w.Service_Type.Image_Path }), Units_Request_Type = q.Units_Request_Type.Select(s => new { s.Request_Type_ID, s.Units_ID, s.Units_Request_Type_ID }), MainServices = q.UnitMainServices.Select(w => new { w.Main_Services.Main_Services_ID, w.Main_Services.Main_Services_Name_AR, w.Main_Services.Main_Services_Name_EN }) }).FirstOrDefaultAsync();
            if (units == null)
                return Ok(new ResponseClass() { success = false, result = "Unit Is NULL" });

            return Ok(new ResponseClass() { success = true, result = units });
        }
        [HttpGet]
        public async Task<IHttpActionResult> ThereIsNoMostafid()
        {
            return Ok(new ResponseClass() { success = true, result = db.Units.Count(q => q.IS_Mostafid) == 0 });
        }
        public async Task<IHttpActionResult> Update(Units units)
        {
            var db = new MostafidDBEntities();
            var data = db.Units.Include(q => q.Units_Type).Include(q => q.Units_Location).Include(q => q.Units_Request_Type).Include(q => q.UnitServiceTypes).FirstOrDefault(q => q.Units_ID == units.Units_ID && !q.Deleted);
            if (!ModelState.IsValid || data == null)
                return Ok(new ResponseClass() { success = false, result = ModelState });
            var trans = db.Database.BeginTransaction();
            var OldVals = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            try
            {
                data.Units_Name_AR = units.Units_Name_AR;
                data.Units_Name_EN = units.Units_Name_EN;

                data.Building_Number = units.Building_Number;
                data.Ref_Number = units.Ref_Number;
                if (data.LevelID != units.LevelID && CanChangeLevel(data.Units_ID, units.LevelID.Value))
                    data.LevelID = units.LevelID;

                data.IS_Mostafid = units.IS_Mostafid;
                db.Units_Request_Type.RemoveRange(data.Units_Request_Type);
                data.Units_Request_Type = units.Units_Request_Type;
                db.UnitServiceTypes.RemoveRange(data.UnitServiceTypes);
                data.UnitServiceTypes = units.UnitServiceTypes;
                data.ServiceTypeID = units.ServiceTypeID;
                if (data.LevelID != 1/*check saved level if not level one and code has two digit*/ && units.Code.Length != 2)
                    units.Code = "0" + units.Code;
                await db.SaveChangesAsync();

                if (units.Code != data.Code || data.Units_Type_ID != units.Units_Type_ID || data.SubID != units.SubID || data.Units_Location_ID != units.Units_Location_ID)
                {
                    //if code,unittype,subuint,location change
                    data.Code = units.Code;
                    string UnitTypeCode = "";
                    int? ParentUnit = null;
                    int? LocationID = null;
                    if (data.Units_Type_ID != units.Units_Type_ID)
                    {
                        data.Units_Type_ID = units.Units_Type_ID;
                        UnitTypeCode = db.Units_Type.First(q => q.Units_Type_ID == units.Units_Type_ID).Code;
                    }
                    else
                        UnitTypeCode = data.Units_Type.Code;
                    if (data.SubID != units.SubID)
                    {
                        data.SubID = units.SubID;
                        ParentUnit = units.SubID;
                    }
                    else
                        ParentUnit = data.SubID;

                    if (data.Units_Location_ID != units.Units_Location_ID)
                    {
                        if (db.Units_Location.Find(units.Units_Location_ID).Deleted)//check if unit location deleted
                        {
                            trans.Rollback();
                            return Ok(new ResponseClass() { success = false, result = "Del UT" });
                        }

                        data.Units_Location_ID = units.Units_Location_ID;
                        LocationID = units.Units_Location_ID;
                    }
                    else
                        LocationID = data.Units_Location_ID;
                    char[] GenrateCode = units.Ref_Number.ToCharArray();
                    GetCodeInternal(ref GenrateCode, ParentUnit, units.Code, UnitTypeCode[0], data.LevelID.Value, LocationID.Value, data.Units_ID);//get unit code
                    var code = string.Join("", GenrateCode).Replace('x', '0');
                    data.Ref_Number = code;
                    await db.SaveChangesAsync();
                    if (!CheckCodeAvab(units.Code, data.Units_ID, data.LevelID.Value, data.Units_Type_ID.Value, db) || !ReArrange(data.Units_ID))//reorder units by recursive
                        throw new Exception("REA");
                }

                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.Unit, Method: "Update", Oldval: OldVals, Newval: data, es: out _, syslog: out _, ID: data.Units_ID, notes: null);
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    trans.Commit();
                    return Ok(new ResponseClass()
                    {
                        success = true
                    });
                }
                else
                {
                    trans.Rollback();
                    return Ok(new ResponseClass() { success = false });
                }
            }
            catch (Exception ee)
            {
                trans.Rollback();
                return Ok(new ResponseClass() { success = false });
            }
            bool ReArrange(int UnitID)
            {
                var _SubUnits = db.Units.Include(q => q.Units1).Include(q => q.Units_Type).Where(q => q.SubID == UnitID);
                foreach (var i in _SubUnits)
                {
                    var UnitLevel = this.db.UnitLevel.FirstOrDefault(q => q.ID == i.LevelID.Value);
                    var unitCode = i.Code;
                    var _code = i.Units2.Ref_Number.ToCharArray();
                    int levelCode = Convert.ToInt32(UnitLevel.Code) - 1;
                    var index = 3 + (levelCode == 0 ? 1 : levelCode * 3);
                    if (levelCode == 0)
                    {
                        if (unitCode.Length > 1)
                            unitCode = "0";
                        _code[index] = i.Units_Type.Code[0];
                        _code[index + 1] = unitCode[0];
                    }
                    else
                    {
                        if (unitCode.Length < 2)
                            unitCode = "00";
                        _code[index] = i.Units_Type.Code[0];
                        _code[index + 1] = unitCode[0];
                        _code[index + 2] = unitCode[1];
                    }
                    i.Ref_Number = string.Join("", _code);
                    db.SaveChanges();
                    ReArrange(i.Units_ID);
                }
                return true;
            }
            void GetCodeInternal(ref char[] _code, int? SubUnitID, string unitCode, char UnittypeCode, int Level, int LocationID, int? unitID = null)
            {
                var queryID = (SubUnitID ?? unitID).Value;
                Units Unit = db.Units.Include(q => q.Units_Location).FirstOrDefault(q => q.Units_ID == queryID);
                if (SubUnitID == null)// if first level
                {
                    var firstpart = "043" + LocationID.ToString() + UnittypeCode + unitCode;
                    _code = (firstpart + string.Join("", Enumerable.Range(0, 15 - firstpart.Length).Select(q => "0").ToArray())).ToCharArray();
                    return;
                }
                var UnitLevel = db.UnitLevel.FirstOrDefault(q => q.ID == Level);
                _code = Unit.Ref_Number.ToCharArray();
                int levelCode = Convert.ToInt32(UnitLevel.Code) - 1;
                var index = 3 + (levelCode == 0 ? 1 : levelCode * 3);
                if (levelCode == 0)// if first level
                {
                    if (unitCode.Length > 1)
                        unitCode = "0";
                    _code[index] = UnittypeCode;
                    _code[index + 1] = unitCode[0];
                    return;
                }
                else
                {
                    if (unitCode.Length < 2)
                        unitCode = "0" + unitCode;
                    _code[3] = LocationID.ToString()[0];
                    _code[index] = UnittypeCode;
                    _code[index + 1] = unitCode[0];
                    _code[index + 2] = unitCode[1];
                    return;
                }
            }
        }
        public async Task<IHttpActionResult> UpdateMainService(int id, [FromBody] Unit_MainServiceEditDTO main)
        {
            var trans = db.Database.BeginTransaction();

            try
            {
                var data = db.Units.Include(q => q.UnitMainServices).FirstOrDefault(q => q.Units_ID == id && !q.Deleted);
                if (data == null)
                    return Ok(new ResponseClass() { success = false, result = "Unit Is NULL" });

                var OldVals = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                foreach (var i in main.Added)
                {
                    if (db.Main_Services.Find(i.MainServiceID).Deleted)
                        return Ok(new ResponseClass() { success = false, result = "Del MS" });
                    if (!data.UnitMainServices.Any(s => s.MainServiceID == i.MainServiceID))//Check if mainservice not exist
                        data.UnitMainServices.Add(new UnitMainServices() { MainServiceID = i.MainServiceID });
                }
                foreach (var i in main.Deleted)
                {
                    var mainser = data.UnitMainServices.First(q => q.MainServiceID == i.MainServiceID);
                    db.UnitMainServices.Remove(mainser);
                    data.UnitMainServices.Remove(mainser);
                }

                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.Unit, Method: "Update", Oldval: OldVals, Newval: data, es: out _, syslog: out _, ID: data.Units_ID, notes: "Update Unit MainServices");
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    trans.Commit();
                    return Ok(new ResponseClass()
                    {
                        success = true
                    });
                }
                else
                {
                    trans.Rollback();
                    return Ok(new ResponseClass() { success = false });
                }

            }
            catch (Exception ee)
            {
                trans.Rollback();
                return Ok(new ResponseClass() { success = false });
            }
        }

        public async Task<IHttpActionResult> Create(Units units)
        {
            var db = new MostafidDBEntities();
            var trans = db.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new ResponseClass() { success = false, result = ModelState });

                ///check if there Deleted RequestType in entred reuest types
                #region Check Deleted RequestType 
                foreach (var i in units.Units_Request_Type)
                    if (this.db.Request_Type.Find(i.Request_Type_ID).Deleted)
                    {
                        trans.Rollback();
                        return base.Ok(new ResponseClass() { success = false, result = "Del RT" });
                    }
                #endregion

                #region Check Deleted ServiceType 
                foreach (var i in units.UnitServiceTypes)
                    if (this.db.Service_Type.Find(i.ServiceTypeID).Deleted)
                    {
                        trans.Rollback();
                        return base.Ok(new ResponseClass() { success = false, result = "Del ST" });
                    }

                if (this.db.Service_Type.Find(units.ServiceTypeID).Deleted)
                {
                    trans.Rollback();
                    return base.Ok(new ResponseClass() { success = false, result = "Del ST" });
                }

                #endregion

                units.Deleted = false;
                units.IS_Action = true;
                if (units.LevelID != 1 && units.Code.Length != 2)
                    units.Code = "0" + units.Code;
                db.Units.Add(units);
                await db.SaveChangesAsync();
                char[] GenrateCode = units.Ref_Number.ToCharArray();
                //if (units.SubID != 0 && units.SubID != null)
                var UnitTypecode = db.Units_Type.First(q => q.Units_Type_ID == units.Units_Type_ID).Code;
                GetCode(ref GenrateCode, units.SubID, units.Code, UnitTypecode[0], units.LevelID.Value, units.Units_Location_ID.Value, units.Units_ID, db);
                units.Ref_Number = string.Join("", GenrateCode).Replace('x', '0');

                await db.SaveChangesAsync();
                if (!CheckCodeAvab(units.Code, units.Units_ID, units.LevelID.Value, units.Units_Type_ID.Value, db))
                    throw new Exception("Code Not Avaible");

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.Unit, Method: "Create", Oldval: null, Newval: units, es: out _, syslog: out _, ID: units.Units_ID, notes: null);
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    trans.Commit();
                    return Ok(new ResponseClass()
                    {
                        success = true
                    });
                }
                else
                {
                    trans.Rollback();
                    return Ok(new ResponseClass() { success = false });
                }
            }
            catch (Exception ee)
            {
                trans.Rollback();
                return Ok(new ResponseClass() { success = false });
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GenrateCode(string Ref_Number, int? SubID, string UCode, string typecode, int loc, int Level)
        {
            char[] GenrateCode = Ref_Number?.ToCharArray() ?? new char[13];
            if (SubID != 0 && SubID != null)
                GetCode(ref GenrateCode, SubID.Value, UCode, typecode[0], Level, loc);
            var code = string.Join("", GenrateCode);
            return Ok(new ResponseClass() { success = true, result = new { Code = code } });
        }
        private bool CheckCodeAvab(string code, int? unitid, int level, int unittype, MostafidDBEntities db = null)
        {
            if (level > 2)/*code can duplicated in level 3,4*/
                return true;
            /*
             رقم الفئة ونوع الفئة مبيتكرروش في المستوي الاول والتاني
            */
            var there_is_only_one_digit_in_code = level == 2 && code.Length == 1;//if level equal two and code is one digit only

            var data = (db ?? new MostafidDBEntities()).Units.Any(q => q.Units_ID != (unitid ?? 0) && (level == 1 /*المستوي الاول*/? q.Code == code : (there_is_only_one_digit_in_code ? q.Code == "0" + code/*if code has one char*/ : q.Code == code)) && q.LevelID == level && q.Units_Type_ID == unittype);
            return !data;
        }
        private bool CanChangeLevel(int unitid, int newLevel)
        {
            var data = db.Units.Count(q => q.SubID == unitid && q.LevelID < newLevel);//if dont have any sub units and can increase yount ex. from level 1 to level 2
            return data == 0;
        }

        [HttpPost]
        public async Task<IHttpActionResult> _CheckEnteredCode(string code, int? unitid, int level, int unittype) => Ok(new ResponseClass() { success = CheckCodeAvab(code, unitid, level, unittype) });

        private void GetCode(ref char[] _code, int? SubUnitID, string unitCode, char UnittypeCode, int Level, int LocationID, int? unitID = null, MostafidDBEntities p = null)
        {
            if (p == null)
                p = new MostafidDBEntities();
            var queryID = (SubUnitID ?? unitID).Value;
            Units Unit = p.Units.Include(q => q.Units_Location).FirstOrDefault(q => q.Units_ID == queryID);
            if (SubUnitID == null)
            {
                var firstpart = "043" + LocationID.ToString()[0] + UnittypeCode + unitCode;
                _code = (firstpart + string.Join("", Enumerable.Range(0, 15 - firstpart.Length).Select(q => "0").ToArray())).ToCharArray();
                return;
            }
            var UnitLevel = p.UnitLevel.FirstOrDefault(q => q.ID == Level);
            _code = Unit.Ref_Number.ToCharArray();
            int levelCode = Convert.ToInt32(UnitLevel.Code) - 1;
            var index = 3 + (levelCode == 0 ? 1 : levelCode * 3);
            if (levelCode == 0)
            {
                if (unitCode.Length > 1)
                    unitCode = "0";
                _code[index] = UnittypeCode;
                _code[index + 1] = unitCode[0];
                return;
            }
            else
            {
                unitCode = unitCode ?? "00";
                if (unitCode.Length < 2)
                    unitCode = "0" + unitCode;
                _code[3] = LocationID.ToString()[0];
                _code[index] = UnittypeCode;
                _code[index + 1] = unitCode[0];
                _code[index + 2] = unitCode[1];
                return;
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Active(int id)
        {
            Units units = await db.Units.FindAsync(id);
            if (units == null || units.Deleted)
                return Ok(new ResponseClass() { success = false, result = "Unit Is NULL" });

            var trans = db.Database.BeginTransaction();

            var OldVals = JsonConvert.SerializeObject(units, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            units.IS_Action = true;
            await db.SaveChangesAsync();


            var logstate = Logger.AddLog(db: db, logClass: LogClassType.Unit, Method: "Active", Oldval: OldVals, Newval: units, es: out _, syslog: out _, ID: units.Units_ID, notes: null);
            if (logstate)
            {
                await db.SaveChangesAsync();
                trans.Commit();
                return Ok(new ResponseClass()
                {
                    success = true
                });
            }
            else
            {
                trans.Rollback();
                return Ok(new ResponseClass() { success = false });
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> Deactive(int id)
        {
            Units units = await db.Units.FindAsync(id);
            if (units == null || units.Deleted)
                return Ok(new ResponseClass() { success = false, result = "Unit Is NULL" });

            var trans = db.Database.BeginTransaction();

            var OldVals = JsonConvert.SerializeObject(units, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            units.IS_Action = false;
            await db.SaveChangesAsync();


            var logstate = Logger.AddLog(db: db, logClass: LogClassType.Unit, Method: "Deactive", Oldval: OldVals, Newval: units, es: out _, syslog: out _, ID: units.Units_ID, notes: null);
            if (logstate)
            {
                await db.SaveChangesAsync();
                trans.Commit();
                return Ok(new ResponseClass()
                {
                    success = true
                });
            }
            else
            {
                trans.Rollback();
                return Ok(new ResponseClass() { success = false });
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> _Delete(int id)
        {
            Units units = db.Units.Include(q => q.Request_Data).Include(q => q.Units_Request_Type).Include(q => q.Unit_Signature).Include(q => q.RequestTransaction1).Include(q => q.RequestTransaction).Include(q => q.Users).Include(q => q.Units1).Include(q => q.UnitMainServices).Include(q => q.E_Forms).Include(q => q.Unit_Signature).Include(q => q.Users).FirstOrDefault(q => q.Units_ID == id && !q.Deleted);
            if (units == null)
                return Ok(new ResponseClass() { success = false, result = "Unit Is NULL" });

            var trans = db.Database.BeginTransaction();

            var OldVals = JsonConvert.SerializeObject(db.Units.Include(q => q.Units_Request_Type).Include(q => q.Service_Type).Include(q => q.Unit_Signature).FirstOrDefault(q => q.Units_ID == id), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            if (units.Request_Data.Count == 0 && units.RequestTransaction1.Count == 0 && units.RequestTransaction.Count == 0 && units.Users.Count == 0 && units.Units1.Count == 0 && units.UnitMainServices.Count == 0 && units.E_Forms.Count == 0/*Eform Approval*/&& units.Users.Count == 0/*Users Jobs*/)
            {
                #region Delete UnitRequestType
                var UnitReqType = units.Units_Request_Type;
                foreach (var UnitReq in UnitReqType)
                {
                    UnitReq.Deleted = true;
                    UnitReq.DeletedAt = DateTime.Now;
                }
                //p.Units_Request_Type.RemoveRange(p.Units_Request_Type.Where(q => q.Units_ID == id).ToList());


                #endregion

                #region Delete UnitServiceType
                var UnitServiceType = units.UnitServiceTypes;
                foreach (var i in UnitServiceType)
                {
                    i.Deleted = true;
                    i.DeletedAt = DateTime.Now;
                }
                //p.UnitServiceTypes.RemoveRange(p.UnitServiceTypes.Where(q => q.UnitID == id).ToList());

                #endregion

                #region Delete UnitSignature
                if (units.Unit_Signature != null)
                {
                    units.Unit_Signature.Deleted = true;
                    units.Unit_Signature.DeletedAt = DateTime.Now;
                }
                #endregion

                units.Deleted = true;
                units.DeletedAt = DateTime.Now;
                //p.Units.Remove(units);
                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.Unit, Method: "Delete", Oldval: OldVals, Newval: db.Units.Include(q => q.Units_Request_Type).Include(q => q.Service_Type).Include(q => q.Unit_Signature).FirstOrDefault(q => q.Units_ID == id), es: out _, syslog: out _, ID: units.Units_ID, notes: null);
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    trans.Commit();
                    return Ok(new ResponseClass()
                    {
                        success = true
                    });
                }
                else
                {
                    trans.Rollback();
                    return Ok(new ResponseClass() { success = false });
                }
            }
            return Ok(new ResponseClass() { success = false, result = "CantRemove" });
        }

        [HttpPost]
        public async Task<IHttpActionResult> _Restore(int id)
        {
            Units units = db.Units.Include(q => q.Units_Request_Type.Select(s => s.Request_Type)).Include(s => s.UnitServiceTypes.Select(q => q.Service_Type)).Include(q => q.Unit_Signature).Include(q => q.Unit_Signature).FirstOrDefault(q => q.Units_ID == id && q.Deleted);
            if (units == null)
                return Ok(new ResponseClass() { success = false, result = "Unit Is NULL" });
            var trans = db.Database.BeginTransaction();

            var OldVals = JsonConvert.SerializeObject(units, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            #region Delete UnitRequestType
            var UnitReqType = units.Units_Request_Type;
            foreach (var UnitReq in UnitReqType)
            {
                if (!UnitReq.Request_Type.Deleted)
                {
                    UnitReq.Deleted = false;
                }
            }
            //p.Units_Request_Type.RemoveRange(p.Units_Request_Type.Where(q => q.Units_ID == id).ToList());


            #endregion

            #region Delete UnitServiceType
            var UnitServiceType = units.UnitServiceTypes;
            foreach (var i in UnitServiceType)
            {
                if (!i.Service_Type.Deleted)
                {
                    i.Deleted = false;
                }
            }
            //p.UnitServiceTypes.RemoveRange(p.UnitServiceTypes.Where(q => q.UnitID == id).ToList());

            #endregion

            #region Delete UnitSignature
            if (units.Unit_Signature != null)
            {
                units.Unit_Signature.Deleted = false;
            }
            #endregion

            units.Deleted = false;
            //p.Units.Remove(units);
            await db.SaveChangesAsync();

            var logstate = Logger.AddLog(db: db, logClass: LogClassType.Unit, Method: "Restore", Oldval: OldVals, Newval: units, es: out _, syslog: out _, ID: units.Units_ID, notes: null);
            if (logstate)
            {
                await db.SaveChangesAsync();
                trans.Commit();
                return Ok(new ResponseClass()
                {
                    success = true
                });
            }
            else
            {
                trans.Rollback();
                return Ok(new ResponseClass() { success = false });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}