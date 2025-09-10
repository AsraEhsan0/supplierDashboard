using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplierDashboard.Data;

namespace SupplierDashboard.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalGroups = await _context.Bookings.Select(b => b.GroupName).Distinct().CountAsync();
            ViewBag.AvailableGroups = await _context.Agencies.CountAsync(a => a.IsActive);

            ViewBag.RecentBookings = await _context.Bookings
                .Include(b => b.Agent)
                .ThenInclude(a => a.Agency)
                .OrderByDescending(b => b.BookingDate)
                .Take(5)
                .Select(b => new
                {
                    GroupName = b.GroupName,
                    AgentId = b.Agent.Id,
                    AgencyName = b.Agent.Agency.AgencyName,
                    PassengerName = b.PassengerName,
                    PNR = b.PNR,
                    Segment = b.Segment
                })
                .ToListAsync();

            ViewBag.ActiveGroups = await _context.Bookings
                .Where(b => !string.IsNullOrEmpty(b.FlightNo))
                .Take(5)
                .Select(b => new
                {
                    GroupName = b.GroupName,
                    FlightNo = b.FlightNo,
                    PNR = b.PNR,
                    SeatsSold = b.SeatsSold
                })
                .ToListAsync();

            return View();
        }
    }
}