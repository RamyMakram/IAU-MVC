using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using IAUBackEnd.Admin.Models;

namespace IAUBackEnd.Admin.Controllers
{
	public class UnitsController : ApiController
	{
		private MostafidDBEntities p = new MostafidDBEntities();

		public async Task<IHttpActionResult> GetUnits()
		{
			return Ok(new ResponseClass() { success = true, result = p.Units.Include(q => q.UnitServiceTypes).Select(q => new { q.Units_ID, q.Units_Name_EN, q.Units_Name_AR, q.IS_Action, UnitServiceTypes = q.UnitServiceTypes.Select(w => new { w.ID, w.ServiceTypeID, w.Service_Type }) }) });
		}

		public async Task<IHttpActionResult> GetActive()
		{
			return Ok(new ResponseClass() { success = true, result = p.Units.Where(q => q.IS_Action == true) });
		}
		public async Task<IHttpActionResult> GetActiveUnits_byLevel(int id)
		{
			return Ok(new ResponseClass() { success = true, result = p.Units.Where(q => q.IS_Action == true && q.LevelID < id).Select(q => new { q.Units_Name_AR, q.Units_Name_EN, q.Units_ID }) });
		}

		public async Task<IHttpActionResult> GetUnits(int id)
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
			var data = p.Units.Include(q => q.Units_Request_Type).Include(q => q.UnitServiceTypes).FirstOrDefault(q => q.Units_ID == units.Units_ID);
			if (!ModelState.IsValid || data == null)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			try
			{
				data.Units_Name_AR = units.Units_Name_AR;
				data.Units_Name_EN = units.Units_Name_EN;
				data.Units_Location_ID = units.Units_Location_ID;
				data.Units_Type_ID = units.Units_Type_ID;
				data.Building_Number = units.Building_Number;
				data.Ref_Number = units.Ref_Number;
				data.LevelID = units.LevelID;
				data.SubID = units.SubID;
				data.IS_Mostafid = units.IS_Mostafid;
				p.Units_Request_Type.RemoveRange(data.Units_Request_Type);
				data.Units_Request_Type = units.Units_Request_Type;
				p.UnitServiceTypes.RemoveRange(data.UnitServiceTypes);
				data.UnitServiceTypes = units.UnitServiceTypes;
				data.ServiceTypeID = units.ServiceTypeID;
				char[] GenrateCode = units.Ref_Number.ToCharArray();
				if (units.SubID != 0 && units.SubID != null)
					GetCode(ref GenrateCode, units.SubID.Value);
				var code = string.Join("", GenrateCode).Replace('x', '0');
				data.Ref_Number = code;

				await p.SaveChangesAsync();
				return Ok(new ResponseClass() { success = true });
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false });
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
			if (!ModelState.IsValid)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			units.IS_Action = true;

			char[] GenrateCode = units.Ref_Number.ToCharArray();
			if (units.SubID != 0 && units.SubID != null)
				GetCode(ref GenrateCode, units.SubID.Value);
			units.Ref_Number = string.Join("", GenrateCode).Replace('x', '0');

			p.Units.Add(units);
			await p.SaveChangesAsync();

			return Ok(new ResponseClass() { success = true });
		}

		[HttpGet]
		public async Task<IHttpActionResult> GenrateCode(string Ref_Number, int? SubID)
		{
			char[] GenrateCode = Ref_Number.ToCharArray();
			if (SubID != 0 && SubID != null)
				GetCode(ref GenrateCode, SubID.Value);
			var code = string.Join("", GenrateCode);
			return Ok(new ResponseClass() { success = true, result = new { Code = code } });
		}
		private void GetCode(ref char[] _code, int UnitID)
		{
			try
			{
				var Unit = p.Units.Include(q => q.UnitLevel).Include(q => q.Units_Type).FirstOrDefault(q => q.Units_ID == UnitID);
				int levelCode = Convert.ToInt32(Unit.UnitLevel.Code) - 1;
				var index = 3 + (levelCode == 0 ? 1 : levelCode * 3);
				if (levelCode == 0)
				{
					_code[index] = Unit.Units_Type.Code[0];
					_code[index + 1] = Unit.Code[0];
					return;
				}
				else
				{
					_code[index] = Unit.Units_Type.Code[0];
					_code[index + 1] = Unit.Code[0];
					_code[index + 2] = Unit.Code[1];
					GetCode(ref _code, Unit.SubID.Value);
				}
			}
			catch (Exception ee)
			{
				_code = "".ToCharArray();
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