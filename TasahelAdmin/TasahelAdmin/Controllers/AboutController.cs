using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TasahelAdmin.Models;
using TasahelAdmin.Models.VM;

namespace TasahelAdmin.Controllers
{
    public class AboutController : Controller
    {
        TasahelContext db;
        IMapper mapper;
        IHostingEnvironment environment;
        public AboutController(TasahelContext _db, IMapper _mapper, IHostingEnvironment _environment)
        {
            db = _db;
            mapper = _mapper;
            environment = _environment;
        }
        public async Task<IActionResult> Index(string domainname, int domainid)
        {
            var data = await db.Abouts.Where(q => q.DomainId == domainid).ToListAsync();

            return View(data);
        }
        public async Task<IActionResult> CreateAbout(string domainname, int domainid)
        {
            AboutVM data = new AboutVM { domainname = domainname, DomainId = domainid, domainid = domainid };
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAbout(AboutVM data)
        {
            var about_data = mapper.Map<About>(data);

            data.Enabled = true;

            await db.Abouts.AddAsync(about_data);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", new { domainname = data.domainname, domainid = data.DomainId });
        }
        public async Task<IActionResult> EditAbout(string domainname, int domainid, int id)
        {
            var edit_data = db.Abouts.FirstOrDefault(q => q.Id == id && q.DomainId == domainid);
            var edit_dataVM = mapper.Map<AboutVM>(edit_data); ;
            edit_dataVM.domainid = domainid;
            edit_dataVM.domainname = domainname;

            return View(edit_dataVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditAbout(string domainname, int domainid, int id, AboutVM data)
        {
            var about_data = mapper.Map<About>(data);

            db.Abouts.Update(about_data);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", new { domainname = data.domainname, domainid = data.DomainId });
        }
        public async Task<IActionResult> ActiveAbout(string domainname, int domainid, int id)
        {
            var edit_data = db.Abouts.FirstOrDefault(q => q.Id == id);
            edit_data.Enabled = true;
            await db.SaveChangesAsync();

            return RedirectToAction("Index", new { domainname = domainname, domainid = domainid });
        }
        public async Task<IActionResult> DeactiveAbout(string domainname, int domainid, int id)
        {
            var edit_data = db.Abouts.FirstOrDefault(q => q.Id == id);
            edit_data.Enabled = false;
            await db.SaveChangesAsync();

            return RedirectToAction("Index", new { domainname = domainname, domainid = domainid });
        }
    }
}
