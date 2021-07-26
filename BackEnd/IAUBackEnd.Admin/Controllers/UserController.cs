//using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IAUBackEnd.Admin.Models;
using System.Data.Entity;
using IAUAdmin.DTO.Helper;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace IAUBackEnd.Admin.Controllers
{
	public class UserController : ApiController
	{
		private Admin.Models.MostafidDBEntities p = new MostafidDBEntities();
		[HttpGet]
		public async Task<IHttpActionResult> Login(string email, string pass)
		{
			try
			{
				var data = p.Users.FirstOrDefault(q => q.IS_Active == "1" && q.User_Email == email && q.User_Password == pass);
				var date = Helper.GetDate();
				var Datetime = Helper.GetHashString("" + data.User_ID + data.User_Name + date.ToString());
				data.TEMP_Login = Datetime;
				data.LoginDate = date.AddDays(1);
				p.SaveChanges();
				return Ok(new ResponseClass
				{
					success = true,
					result = new { Token = Datetime }
				});
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass
				{
					success = false,
					result = ee
				});
			}
		}

		[HttpGet]
		public async Task<IHttpActionResult> VerfiyToken(string token)
		{
			try
			{
				var date = Helper.GetDate();
				var data = p.Users.Include(q => q.Units).FirstOrDefault(q => q.IS_Active == "1" && q.TEMP_Login == token && q.LoginDate > date);
				return Ok(new ResponseClass
				{
					success = true,
					result = new { data.User_ID, EN_Top = "Hello " + data.Units.Units_Name_EN + " ،" + data.User_Name, AR_Top = "مرحبا " + data.Units.Units_Name_AR + " ،" + data.User_Name }
				});
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass
				{
					success = false,
					result = ee
				});
			}
		}

		[HttpGet]
		public async Task<IHttpActionResult> VerfiyUser(int id, string token)
		{
			try
			{
				var date = Helper.GetDate();
				var data = p.Users.Include(q => q.Job.Job_Permissions.Select(s => s.Privilage)).Include(Q => Q.Units).FirstOrDefault(q => q.User_ID == id && q.IS_Active == "1" && q.TEMP_Login == token && q.LoginDate > date);
				if (data == null)
					return Ok(new ResponseClass
					{
						success = false
					});
				else
				{
					var perm = data.Job.Job_Permissions.Select(q => q.Privilage.Name_EN).ToArray();
					return Ok(new ResponseClass
					{
						success = true,
						result = new { perm, data.Units.IS_Mostafid }
					});
				}
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass
				{
					success = false,
					result = ee
				});
			}
		}

		[HttpGet]
		public async Task<IHttpActionResult> GetAll()
		{
			try
			{
				var data = p.Users
					.Include(q => q.Job)
					.Select(q => new
					{
						q.User_ID,
						q.User_Name,
						q.User_Mobile,
						q.User_Password,
						q.User_Email,
						q.IS_Active,
						q.Job_ID,
						q.Job.User_Permissions_Type_Name_AR,
						q.Job.User_Permissions_Type_Name_EN,
					});
				return Ok(new ResponseClass
				{
					success = true,
					result = data
				});
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass
				{
					success = false,
					result = ee
				});
			}
		}

		[HttpGet]
		public async Task<IHttpActionResult> GetAllByUnit(int UID)
		{
			try
			{
				var data = p.Users.Where(q => q.UnitID == UID)
					.Include(q => q.Job)
					.Select(q => new
					{
						q.User_ID,
						q.User_Name,
						q.User_Email,
						q.Job
					});
				return Ok(new ResponseClass
				{
					success = true,
					result = data
				});
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass
				{
					success = false,
					result = ee
				});
			}
		}

		[HttpGet]
		public async Task<IHttpActionResult> GetDetails(int uid)
		{
			try
			{
				var data = p.Users
					.Where(qt => qt.User_ID == uid)
					.Select(q => new
					{
						q.User_ID,
						q.User_Name,
						q.User_Email,
						q.User_Mobile,
						q.User_Password,
						q.IS_Active,
						q.Job_ID,
						q.Job.User_Permissions_Type_Name_AR,
						q.Job.User_Permissions_Type_Name_EN,
						q.UnitID
					})
					.FirstOrDefault();
				return Ok(new ResponseClass
				{
					success = true,
					result = data
				});
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass
				{
					success = false,
					result = ee
				});
			}
		}

		[HttpPost]
		public async Task<IHttpActionResult> UpdateData([FromBody] Users users)
		{

			try
			{
				var data = p.Users.FirstOrDefault(qt => qt.User_ID == users.User_ID);
				data.User_Mobile = users.User_Mobile;
				data.User_Name = users.User_Name;
				data.User_Password = users.User_Password;
				data.Job_ID = users.Job_ID;
				data.UnitID = users.UnitID;
				p.SaveChanges();
				return Ok(new ResponseClass
				{
					success = true,
					result = data
				});
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass
				{
					success = false,
					result = ee
				});
			}
		}

		[HttpGet]
		public async Task<IHttpActionResult> Deactive(int uid)
		{

			try
			{
				var data = p.Users.FirstOrDefault(qt => qt.User_ID == uid);
				data.IS_Active = "0";
				p.SaveChanges();
				return Ok(new ResponseClass
				{
					success = true,
					result = data
				});
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass
				{
					success = false,
					result = ee
				});
			}
		}

		[HttpGet]
		public async Task<IHttpActionResult> Active(int uid)
		{

			try
			{
				var data = p.Users.FirstOrDefault(qt => qt.User_ID == uid);
				data.IS_Active = "1";
				p.SaveChanges();
				return Ok(new ResponseClass
				{
					success = true,
					result = data
				});
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass
				{
					success = false,
					result = ee
				});
			}
		}

		[HttpPost]
		public async Task<IHttpActionResult> Create([FromBody] Users users)
		{

			try
			{
				var data = p.Users.Add(users);
				if (p.SaveChanges() > 0)
					return Ok(new ResponseClass
					{
						success = true,
						result = data
					});
				else
					return Ok(new ResponseClass
					{
						success = false,
						result = "Add Faild"
					});
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass
				{
					success = false,
					result = ee
				});
			}
		}
		[HttpPost]
		public async Task<IHttpActionResult> _Delete(int id)
		{
			var user = p.Users.FirstOrDefault(q => q.User_ID == id);
			if (user == null)
				return Ok(new ResponseClass() { success = false, result = "User Is Null" });
			p.Users.Remove(user);
			await p.SaveChangesAsync();
			return Ok(new ResponseClass() { success = true });
		}
	}
}
