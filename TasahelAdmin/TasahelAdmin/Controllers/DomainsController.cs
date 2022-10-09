using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
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


                string fileName = Path.GetFileName(dd.DomainStyle._Icon.FileName);


                using (FileStream stream = new FileStream(Path.Combine(Icopath, fileName), FileMode.Create))
                {
                    dd.DomainStyle._Icon.CopyTo(stream);
                    Domain.DomainStyle.Icon = Path.Combine(globalpath, "Ico", fileName).Replace("\\","/");
                }

                fileName = Path.GetFileName(dd.DomainStyle._FavIcon.FileName);
                using (FileStream stream = new FileStream(Path.Combine(Favicopath, fileName), FileMode.Create))
                {
                    dd.DomainStyle._FavIcon.CopyTo(stream);
                    Domain.DomainStyle.Favicon = Path.Combine(globalpath, "Favico", fileName).Replace("\\","/");
                }


                fileName = Path.GetFileName(dd.HomeSettings._FollowIco.FileName);
                using (FileStream stream = new FileStream(Path.Combine(FollowReqpath, fileName), FileMode.Create))
                {
                    dd.HomeSettings._FollowIco.CopyTo(stream);
                    Domain.TasahelHomeSetting.FollowIco = Path.Combine(globalpath, "FollowReq", fileName).Replace("\\","/");
                }

                fileName = Path.GetFileName(dd.HomeSettings._NewReqICo.FileName);
                using (FileStream stream = new FileStream(Path.Combine(NewReqpath, fileName), FileMode.Create))
                {
                    dd.HomeSettings._NewReqICo.CopyTo(stream);
                    Domain.TasahelHomeSetting.NewReqIco = Path.Combine(globalpath, "NewReq", fileName).Replace("\\","/");
                }
                await db.SaveChangesAsync();
                trans.Commit();
                return RedirectToAction("MangeAbout", new { id = Domain.Id });
            }
            catch (System.Exception ee)
            {
                trans.Rollback();
                return View(dd);
            }

        }
    }
}
