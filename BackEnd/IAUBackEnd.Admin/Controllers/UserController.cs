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
				var Datetime = Encoding.ASCII.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(new DateTime().AddMinutes(3).Millisecond.ToString())));
				data.TEMP_Login = Datetime;
				p.SaveChanges();
				return Ok(new ResponseClass
				{
					success = true,
					result = JsonConvert.SerializeObject(new { Token = Datetime })
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
				var data = p.Users.FirstOrDefault(q => q.IS_Active == "1" && q.TEMP_Login == token);
				return Ok(new ResponseClass
				{
					success = true,
					result = data.User_ID
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
		public async Task<IHttpActionResult> VerfiyUser(int id)
		{
			try
			{
				var data = p.Users.FirstOrDefault(q => q.User_ID == id);
				if (data.IS_Active == "1")
					return Ok(new ResponseClass
					{
						success = true,
						result = p.Job_Permissions.Where(q => q.Job_ID == data.Job_ID).Select(q => q.Privilage.Name_EN).ToArray()
					});
				else
					return Ok(new ResponseClass
					{
						success = false
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
						q.Job.User_Permissions_Type_Name_EN
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

	}
}
