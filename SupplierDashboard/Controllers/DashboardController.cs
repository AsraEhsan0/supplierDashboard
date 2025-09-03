using Microsoft.AspNetCore.Mvc;

namespace SupplierDashboard.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.TotalGroups = 12;
            ViewBag.AvailableGroups = 85;

            ViewBag.RecentBookings = new object[]
            {
                new { GroupName = "Group A", AgentId = "AGT001", AgencyName = "Sky Travels", PassengerName = "Ali Khan", PNR = "PNR12345", Segment = "LHE - BAH" },
                new { GroupName = "Group B", AgentId = "AGT002", AgencyName = "Falcon Tours", PassengerName = "Fatima Zahra", PNR = "PNR67890", Segment = "LHE - BAH" },
                new { GroupName = "Group C", AgentId = "AGT003", AgencyName = "Global Wings", PassengerName = "Omar Faisal", PNR = "PNR11122", Segment = "LHE - BAH" }
            };

            ViewBag.ActiveGroups = new object[]
            {
                new { GroupName = "Group D", FlightNo = "FL001", PNR = "PNR44444", SeatsSold = 25 },
                new { GroupName = "Group E", FlightNo = "FL002", PNR = "PNR55555", SeatsSold = 18 }
            };

            return View();
        }
    }
}