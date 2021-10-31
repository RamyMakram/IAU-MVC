using System;
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

namespace IAUBackEnd.Admin.Controllers
{
    public class UnitsController : ApiController
    {
        private MostafidDBEntities p = new MostafidDBEntities();

        public async Task<IHttpActionResult> GetUnits()
        {
            return Ok(new ResponseClass() { success = true, result = p.Units.Include(q => q.UnitServiceTypes).Include(q => q.Service_Type).Select(q => new { q.IS_Mostafid, q.Service_Type, q.Units_ID, q.Units_Name_EN, q.Units_Name_AR, q.IS_Action, UnitServiceTypes = q.UnitServiceTypes.Select(w => new { w.ID, w.ServiceTypeID, w.Service_Type }) }) });
        }

        public async Task<IHttpActionResult> GetActive()
        {
            return Ok(new ResponseClass() { success = true, result = p.Units.Where(q => q.IS_Action == true) });
        }
        public async Task<IHttpActionResult> GetActiveForEmail()
        {
            return Ok(new ResponseClass() { success = true, result = p.Units.Where(q => q.IS_Action == true && q.Users.Any(s => s.IS_Active == "1" && (!q.IS_Mostafid))) });
        }
        public async Task<IHttpActionResult> GetUniqueBuildingByLoca(int id)
        {
            return Ok(new ResponseClass() { success = true, result = p.Units.Where(q => q.IS_Action == true && q.Units_Location_ID == id).Select(q => q.Building_Number).Distinct() });
        }
        public async Task<IHttpActionResult> GetActiveUnits_by(int serviceType, int Req, int? locid, string Build)
        {
            var publider = PredicateBuilder.New<Units>(q => (q.ServiceTypeID == serviceType || q.UnitServiceTypes.Any(w => w.ServiceTypeID == serviceType)) && q.Units_Request_Type.Any(w => w.Request_Type_ID == Req) && q.Users.Any(s => s.IS_Active == "1") && (!q.IS_Mostafid));
            if (Build != "" && Build != "null" && Build != null)
                publider.And(q => q.Building_Number.Equals(Build));
            if (locid != null)
                publider.And(q => q.Units_Location_ID == locid);
            return Ok(new ResponseClass() { success = true, result = p.Units.Where(publider).Select(q => new { q.Units_Name_AR, q.Units_Name_EN, q.Units_ID, q.Building_Number, q.Units_Location_ID }) });
        }
        public async Task<IHttpActionResult> GetActiveUnits_byLevel(int id, int? uintId)
        {
            var pred = PredicateBuilder.New<Units>();
            pred.And(q => q.IS_Action == true && q.LevelID < id);
            if (uintId != null)
                pred.And(q => q.Units_ID != uintId && q.SubID != uintId);
            return Ok(new ResponseClass() { success = true, result = p.Units.Where(pred).Select(q => new { q.Units_Name_AR, q.Units_Name_EN, q.Units_ID }) });
        }

        public async Task<IHttpActionResult> GetUnits(int id)
        {
            var units = await p.Units.Where(q => q.Units_ID == id).Select(q => new { q.Units_Type, q.ServiceTypeID, q.UnitLevel, q.Code, q.Units_ID, q.Units_Name_AR, q.Units_Name_EN, q.Units_Location_ID, q.Units_Type_ID, q.Ref_Number, q.Building_Number, q.LevelID, q.SubID, q.IS_Action, q.IS_Mostafid, q.Units_Location, CanChangeLevel = q.Units1.Count == 0, Request_Type = q.Units_Request_Type.Select(w => new { w.Request_Type.Image_Path, w.Request_Type.Request_Type_Name_AR, w.Request_Type.Request_Type_Name_EN }), ServiceTypes = q.UnitServiceTypes.Select(w => new { Service_Type_ID = w.ServiceTypeID, w.Service_Type.Service_Type_Name_AR, w.Service_Type.Service_Type_Name_EN, w.Service_Type.Image_Path }), Units_Request_Type = q.Units_Request_Type.Select(s => new { s.Request_Type_ID, s.Units_ID, s.Units_Request_Type_ID }), MainServices = q.UnitMainServices.Select(w => new { w.Main_Services.Main_Services_ID, w.Main_Services.Main_Services_Name_AR, w.Main_Services.Main_Services_Name_EN }) }).FirstOrDefaultAsync();
            if (units == null)
                return Ok(new ResponseClass() { success = false, result = "Unit Is NULL" });

            return Ok(new ResponseClass()
            {
                success = true,
                result = new
                {
                    Unit = units,
                    Units_Types = p.Units_Type.Where(q => q.LevelID == units.LevelID).Select(q => new { q.Units_Type_ID, q.Units_Type_Name_AR, q.Units_Type_Name_EN }),
                    SubUnits = p.Units.Where(q => q.LevelID < units.LevelID && q.Units_ID != id).Select(q => new { q.Units_Name_AR, q.Units_Name_EN, q.Units_ID })
                }
            });
        }
        public async Task<IHttpActionResult> GetUnitsByID(int id)
        {
            var units = await p.Units.Where(q => q.Units_ID == id).Select(q => new { q.ServiceTypeID, q.Code, q.Units_ID, q.Units_Name_AR, q.Units_Name_EN, q.Units_Location_ID, q.Units_Type_ID, q.Ref_Number, q.Building_Number, q.LevelID, q.SubID, q.IS_Action, q.IS_Mostafid, q.Units_Type, q.Units_Location, Request_Type = q.Units_Request_Type.Select(w => new { w.Request_Type.Image_Path, w.Request_Type.Request_Type_Name_AR, w.Request_Type.Request_Type_Name_EN }), ServiceTypes = q.UnitServiceTypes.Select(w => new { Service_Type_ID = w.ServiceTypeID, w.Service_Type.Service_Type_Name_AR, w.Service_Type.Service_Type_Name_EN, w.Service_Type.Image_Path }), Units_Request_Type = q.Units_Request_Type.Select(s => new { s.Request_Type_ID, s.Units_ID, s.Units_Request_Type_ID }), MainServices = q.UnitMainServices.Select(w => new { w.Main_Services.Main_Services_ID, w.Main_Services.Main_Services_Name_AR, w.Main_Services.Main_Services_Name_EN }) }).FirstOrDefaultAsync();
            if (units == null)
                return Ok(new ResponseClass() { success = false, result = "Unit Is NULL" });

            return Ok(new ResponseClass() { success = true, result = units });
        }
        [HttpGet]
        public async Task<IHttpActionResult> ThereIsNoMostafid()
        {
            return Ok(new ResponseClass() { success = true, result = p.Units.Count(q => q.IS_Mostafid) == 0 });
        }
        public async Task<IHttpActionResult> Update(Units units)
        {
            var db = new MostafidDBEntities();
            var data = db.Units.Include(q => q.Units_Type).Include(q => q.Units_Location).Include(q => q.Units_Request_Type).Include(q => q.UnitServiceTypes).FirstOrDefault(q => q.Units_ID == units.Units_ID);
            if (!ModelState.IsValid || data == null)
                return Ok(new ResponseClass() { success = false, result = ModelState });
            var trans = db.Database.BeginTransaction();
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
                        data.Units_Location_ID = units.Units_Location_ID;
                        LocationID = units.Units_Location_ID;
                    }
                    else
                        LocationID = data.Units_Location_ID;
                    char[] GenrateCode = units.Ref_Number.ToCharArray();
                    GetCodeInternal(ref GenrateCode, ParentUnit, units.Code, UnitTypeCode[0], data.LevelID.Value, LocationID.Value, data.Units_ID);
                    var code = string.Join("", GenrateCode).Replace('x', '0');
                    data.Ref_Number = code;
                    await db.SaveChangesAsync();
                    if (CheckCodeAvab(units.Code, data.Units_ID, data.LevelID.Value, db) && !ReArrange(data.Units_ID))
                        throw new Exception("REA");
                }
                await db.SaveChangesAsync();

                trans.Commit();
                return Ok(new ResponseClass() { success = true });
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
                    var UnitLevel = p.UnitLevel.FirstOrDefault(q => q.ID == i.LevelID.Value);
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
                if (SubUnitID == null)
                {
                    var firstpart = "043" + LocationID.ToString() + UnittypeCode + unitCode;
                    _code = (firstpart + string.Join("", Enumerable.Range(0, 15 - firstpart.Length).Select(q => "0").ToArray())).ToCharArray();
                    return;
                }
                var UnitLevel = db.UnitLevel.FirstOrDefault(q => q.ID == Level);
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
                    if (unitCode.Length < 2)
                        unitCode = "00";
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
            try
            {
                var data = p.Units.Include(q => q.UnitMainServices).FirstOrDefault(q => q.Units_ID == id);
                if (data == null)
                    return Ok(new ResponseClass() { success = false, result = "Unit Is NULL" });
                foreach (var i in main.Added)
                    data.UnitMainServices.Add(new UnitMainServices() { MainServiceID = i.MainServiceID });
                foreach (var i in main.Deleted)
                    p.UnitMainServices.Remove(data.UnitMainServices.First(q => q.MainServiceID == i.MainServiceID));
                await p.SaveChangesAsync();
                return Ok(new ResponseClass() { success = true });
            }
            catch (Exception ee)
            {
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
                units.IS_Action = true;
                db.Units.Add(units);
                await db.SaveChangesAsync();
                char[] GenrateCode = units.Ref_Number.ToCharArray();
                //if (units.SubID != 0 && units.SubID != null)
                var UnitTypecode = db.Units_Type.First(q => q.Units_Type_ID == units.Units_Type_ID).Code;
                GetCode(ref GenrateCode, units.SubID, units.Code, UnitTypecode[0], units.LevelID.Value, units.Units_Location_ID.Value, units.Units_ID, db);
                units.Ref_Number = string.Join("", GenrateCode).Replace('x', '0');

                await db.SaveChangesAsync();
                trans.Commit();
                return Ok(new ResponseClass() { success = true });
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
            char[] GenrateCode = Ref_Number.ToCharArray();
            if (SubID != 0 && SubID != null)
                GetCode(ref GenrateCode, SubID.Value, UCode, typecode[0], Level, loc);
            var code = string.Join("", GenrateCode);
            return Ok(new ResponseClass() { success = true, result = new { Code = code } });
        }
        private bool CheckCodeAvab(string code, int? unitid, int level, MostafidDBEntities db = null)
        {
            if (level > 2)
                return true;

            var data = (db ?? new MostafidDBEntities()).Units.Any(q => q.Units_ID != (unitid ?? 0) && q.Code == code && q.LevelID == level);
            return !data;
        }
        private bool CanChangeLevel(int unitid, int newLevel)
        {
            var data = p.Units.Count(q => q.SubID == unitid && q.LevelID < newLevel);//if dont have any sub units and can increase yount ex. from level 1 to level 2
            return data == 0;
        }

        [HttpPost]
        public async Task<IHttpActionResult> _CheckEnteredCode(string code, int? unitid, int level) => Ok(new ResponseClass() { success = CheckCodeAvab(code, unitid, level) });

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
                if (unitCode == null || unitCode.Length < 2)
                    unitCode = "00";
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
            Units units = await p.Units.FindAsync(id);
            if (units == null)
                return Ok(new ResponseClass() { success = false, result = "Unit Is NULL" });

            units.IS_Action = true;
            await p.SaveChangesAsync();

            return Ok(new ResponseClass() { success = true });
        }
        [HttpGet]
        public async Task<IHttpActionResult> Deactive(int id)
        {
            Units units = await p.Units.FindAsync(id);
            if (units == null)
                return Ok(new ResponseClass() { success = false, result = "Unit Is NULL" });

            units.IS_Action = false;
            await p.SaveChangesAsync();

            return Ok(new ResponseClass() { success = true });
        }

        [HttpPost]
        public async Task<IHttpActionResult> _Delete(int id)
        {
            Units units = p.Units.Include(q => q.Request_Data).Include(q => q.RequestTransaction1).Include(q => q.RequestTransaction).Include(q => q.Users).Include(q => q.Units1).Include(q => q.UnitMainServices).FirstOrDefault(q => q.Units_ID == id);
            if (units == null)
                return Ok(new ResponseClass() { success = false, result = "Unit Is NULL" });
            if (units.Request_Data.Count == 0 && units.RequestTransaction1.Count == 0 && units.RequestTransaction.Count == 0 && units.Users.Count == 0 && units.Units1.Count == 0 && units.UnitMainServices.Count == 0)
            {
                p.Units_Request_Type.RemoveRange(p.Units_Request_Type.Where(q => q.Units_ID == id).ToList());
                p.UnitServiceTypes.RemoveRange(p.UnitServiceTypes.Where(q => q.UnitID == id).ToList());
                p.Units.Remove(units);
                await p.SaveChangesAsync();
                return Ok(new ResponseClass() { success = true });
            }
            return Ok(new ResponseClass() { success = false, result = "CantRemove" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                p.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}