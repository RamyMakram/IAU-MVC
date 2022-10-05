using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TasahelAdmin.Models;

namespace TasahelAdmin.Controllers
{
    public class DomainsController : Controller
    {
        TasahelContext db;
        public DomainsController(TasahelContext _db)
        {
            db = _db;
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
        public async Task<IActionResult> Create(Domain dd)
        {
            return View();
        }
    }
}
