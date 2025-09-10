using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplierDashboard.Data;
using SupplierDashboard.Models.Entities;

namespace SupplierDashboard.Controllers
{
    public class AgentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Agents
        public async Task<IActionResult> Index()
        {
            var agents = await _context.Agents
                .Include(a => a.Agency)
                .ToListAsync();
            return View(agents);
        }

        // GET: /Agents/AddAgent
        public async Task<IActionResult> AddAgent()
        {
            ViewBag.Agencies = await _context.Agencies
                .Where(a => a.IsActive)
                .Select(a => new { a.Id, a.AgencyName })
                .ToListAsync();
            return View(new Agent());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAgent(Agent agent)
        {
            ModelState.Remove("Agency");

            if (ModelState.IsValid)
            {
                try
                {
                    agent.CreatedAt = DateTime.UtcNow;
                    _context.Agents.Add(agent);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Agent added successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the agent: " + ex.Message);
                }
            }

            ViewBag.Agencies = await _context.Agencies
                .Where(a => a.IsActive)
                .Select(a => new { a.Id, a.AgencyName })
                .ToListAsync();

            return View(agent);
        }
    }
}