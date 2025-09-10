using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplierDashboard.Data;
using SupplierDashboard.Models.Entities;

namespace SupplierDashboard.Controllers
{
    public class AgenciesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgenciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Return strongly typed list
        public async Task<IActionResult> Index()
        {
            var agencies = await _context.Agencies.Include(a => a.Agents).ToListAsync();
            return View(agencies);
        }

        public IActionResult AddAgency()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAgency(Agency agency)
        {
            if (ModelState.IsValid)
            {
                _context.Agencies.Add(agency);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(agency);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var agency = await _context.Agencies.FindAsync(id);
            if (agency != null)
            {
                agency.IsActive = !agency.IsActive;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
