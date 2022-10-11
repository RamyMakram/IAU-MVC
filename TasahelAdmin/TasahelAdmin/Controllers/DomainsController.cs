using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using TasahelAdmin.Models;
using TasahelAdmin.Models.VM;

namespace TasahelAdmin.Controllers
{
    public class DomainsController : Controller
    {
        TasahelContext db;
        IMapper mapper;
        IHostingEnvironment environment;
        public DomainsController(TasahelContext _db, IMapper _mapper, IHostingEnvironment _environment)
        {
            db = _db;
            mapper = _mapper;
            environment = _environment;
        }

        public async Task<IActionResult> Index()
        {
            var data = await db.Domains.ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        public async Task<IActionResult> Delete(int id)
        {
            var data = await db.Domains.FirstOrDefaultAsync(q => q.Id == id);
            db.Domains.Remove(data);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var data = await db.Domains.Include(q => q.SubDomains).Include(q => q.DomainEmail).Include(q => q.DomainInfo).Include(q => q.DomainStyle).Include(q => q.TasahelHomeSetting)
        //                .FirstOrDefaultAsync(q => q.Id == id);
        //    var data_vm = mapper.Map<DomainCreateVM>(data);
        //    return View(data_vm);
        //}


        [HttpPost]
        public async Task<IActionResult> Create(DomainCreateVM dd)
        {
            var trans = db.Database.BeginTransaction();
            try
            {

                var Domain = mapper.Map<Domain>(dd);
                Domain.DomainStyle.Icon = "";
                Domain.DomainStyle.Favicon = "";
                Domain.TasahelHomeSetting.FollowIco = "";
                Domain.TasahelHomeSetting.NewReqIco = "";
                db.Domains.Add(Domain);
                await db.SaveChangesAsync();


                string globalpath = Path.Combine("Images", Domain.Domain1);
                Directory.CreateDirectory(Path.Combine(environment.WebRootPath, globalpath));

                var Icopath = Path.Combine(environment.WebRootPath, globalpath, "Ico", Domain.Domain1);
                Directory.CreateDirectory(Icopath);
                var Favicopath = Path.Combine(environment.WebRootPath, globalpath, "Favico", Domain.Domain1);
                Directory.CreateDirectory(Favicopath);
                var FollowReqpath = Path.Combine(environment.WebRootPath, globalpath, "FollowReq", Domain.Domain1);
                Directory.CreateDirectory(FollowReqpath);
                var NewReqpath = Path.Combine(environment.WebRootPath, globalpath, "NewReq", Domain.Domain1);
                Directory.CreateDirectory(NewReqpath);


                string fileName = "";

                if (dd.DomainStyle._Icon != null && dd.DomainStyle._Icon.ValidateImage())
                {
                    ModelState.AddModelError("DomainStyle._Icon", "There is Domain With This Url");
                    trans.Rollback();
                    return View(dd);
                }
                if (dd.DomainStyle._FavIcon != null && dd.DomainStyle._FavIcon.ValidateImage())
                {
                    ModelState.AddModelError("DomainStyle._FavIcon", "There is Domain With This Url");
                    trans.Rollback();
                    return View(dd);
                }
                if (dd.HomeSettings._NewReqICo != null && dd.HomeSettings._NewReqICo.ValidateImage())
                {
                    ModelState.AddModelError("DomainStyle._NewReqICo ", "There is Domain With This Url");
                    trans.Rollback();
                    return View(dd);
                }
                if (dd.HomeSettings._FollowIco != null && dd.HomeSettings._FollowIco.ValidateImage())
                {
                    ModelState.AddModelError("HomeSettings._FollowIco", "There is Domain With This Url");
                    trans.Rollback();
                    return View(dd);
                }




                fileName = Path.GetFileName(dd.DomainStyle._Icon.FileName);
                using (FileStream stream = new FileStream(Path.Combine(Icopath, fileName), FileMode.Create))
                {
                    dd.DomainStyle._Icon.CopyTo(stream);
                    Domain.DomainStyle.Icon = Path.Combine(globalpath, "Ico", fileName).Replace("\\", "/");
                }

                fileName = Path.GetFileName(dd.DomainStyle._FavIcon.FileName);
                using (FileStream stream = new FileStream(Path.Combine(Favicopath, fileName), FileMode.Create))
                {
                    dd.DomainStyle._FavIcon.CopyTo(stream);
                    Domain.DomainStyle.Favicon = Path.Combine(globalpath, "Favico", fileName).Replace("\\", "/");
                }


                fileName = Path.GetFileName(dd.HomeSettings._FollowIco.FileName);
                using (FileStream stream = new FileStream(Path.Combine(FollowReqpath, fileName), FileMode.Create))
                {
                    dd.HomeSettings._FollowIco.CopyTo(stream);
                    Domain.TasahelHomeSetting.FollowIco = Path.Combine(globalpath, "FollowReq", fileName).Replace("\\", "/");
                }

                fileName = Path.GetFileName(dd.HomeSettings._NewReqICo.FileName);
                using (FileStream stream = new FileStream(Path.Combine(NewReqpath, fileName), FileMode.Create))
                {
                    dd.HomeSettings._NewReqICo.CopyTo(stream);
                    Domain.TasahelHomeSetting.NewReqIco = Path.Combine(globalpath, "NewReq", fileName).Replace("\\", "/");
                }
                await db.SaveChangesAsync();
                trans.Commit();
                return RedirectToAction("Index", "About", new { id = Domain.Id, domainname = Domain.Name });
            }
            catch (System.Exception ee)
            {
                if (ee is DbUpdateException && ee.InnerException != null && ee.InnerException.Message.Contains("IX_Domain"))
                {
                    ModelState.AddModelError("Domain1", "There is Domain With This Url");
                }
                trans.Rollback();
                return View(dd);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(DomainCreateVM dd)
        {
            var trans = db.Database.BeginTransaction();
            try
            {

                var Domain = mapper.Map<Domain>(dd);
                Domain.DomainStyle.Icon = "";
                Domain.DomainStyle.Favicon = "";
                Domain.TasahelHomeSetting.FollowIco = "";
                Domain.TasahelHomeSetting.NewReqIco = "";
                db.Attach<Domain>(Domain);
                db.Entry(Domain).Reference(p => p.DomainStyle).IsModified = false;
                db.Entry(Domain).Reference(p => p.TasahelHomeSetting).IsModified = false;
                db.Entry(Domain).Collection(p => p.SubDomains).IsModified = false;
                db.Entry(Domain).Reference(p => p.DomainEmail).IsModified = false;
                db.Entry(Domain).Reference(p => p.DomainInfo).IsModified = false;
                //db.Domains.Update(Domain);
                db.Entry(Domain).State = EntityState.Modified;
                await db.SaveChangesAsync();


                string globalpath = Path.Combine("Images", Domain.Domain1);
                Directory.CreateDirectory(Path.Combine(environment.WebRootPath, globalpath));

                var Icopath = Path.Combine(environment.WebRootPath, globalpath, "Ico", Domain.Domain1);
                Directory.CreateDirectory(Icopath);
                var Favicopath = Path.Combine(environment.WebRootPath, globalpath, "Favico", Domain.Domain1);
                Directory.CreateDirectory(Favicopath);
                var FollowReqpath = Path.Combine(environment.WebRootPath, globalpath, "FollowReq", Domain.Domain1);
                Directory.CreateDirectory(FollowReqpath);
                var NewReqpath = Path.Combine(environment.WebRootPath, globalpath, "NewReq", Domain.Domain1);
                Directory.CreateDirectory(NewReqpath);
                string fileName = "";
                if (dd.DomainStyle._Icon != null)
                {
                    fileName = Path.GetFileName(dd.DomainStyle._Icon.FileName);


                    using (FileStream stream = new FileStream(Path.Combine(Icopath, fileName), FileMode.Create))
                    {
                        dd.DomainStyle._Icon.CopyTo(stream);
                        Domain.DomainStyle.Icon = Path.Combine(globalpath, "Ico", fileName).Replace("\\", "/");
                    }
                }
                if (dd.DomainStyle._FavIcon != null)
                {
                    fileName = Path.GetFileName(dd.DomainStyle._FavIcon.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(Favicopath, fileName), FileMode.Create))
                    {
                        dd.DomainStyle._FavIcon.CopyTo(stream);
                        Domain.DomainStyle.Favicon = Path.Combine(globalpath, "Favico", fileName).Replace("\\", "/");
                    }
                }
                if (dd.HomeSettings._FollowIco != null)
                {
                    fileName = Path.GetFileName(dd.HomeSettings._FollowIco.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(FollowReqpath, fileName), FileMode.Create))
                    {
                        dd.HomeSettings._FollowIco.CopyTo(stream);
                        Domain.TasahelHomeSetting.FollowIco = Path.Combine(globalpath, "FollowReq", fileName).Replace("\\", "/");
                    }
                }
                if (dd.HomeSettings._NewReqICo != null)
                {
                    fileName = Path.GetFileName(dd.HomeSettings._NewReqICo.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(NewReqpath, fileName), FileMode.Create))
                    {
                        dd.HomeSettings._NewReqICo.CopyTo(stream);
                        Domain.TasahelHomeSetting.NewReqIco = Path.Combine(globalpath, "NewReq", fileName).Replace("\\", "/");
                    }
                }

                await db.SaveChangesAsync();
                trans.Commit();
                return RedirectToAction("Index");
            }
            catch (System.Exception ee)
            {
                if (ee is DbUpdateException && ee.InnerException != null && ee.InnerException.Message.Contains("IX_Domain"))
                {
                    ModelState.AddModelError("Domain1", "There is Domain With This Url");
                }
                trans.Rollback();
                return View(dd);
            }
        }
    }
}
