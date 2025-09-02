using Microsoft.AspNetCore.Mvc;

namespace SupplierDashboard.Controllers
{
    public class AgentsController : Controller
    {
        private static List<dynamic> agentsList = new List<dynamic>
        {
            new {
                AgentName = "yasir ali",
                UserName = "",
                Email = "",
                AgencyName = "",
                Status = "Active"
            },
            new {
                AgentName = "mubashar",
                UserName = "",
                Email = "global123@gmail.com",
                AgencyName = "global",
                Status = "Active"
            },
            new {
                AgentName = "Mubashar Ali",
                UserName = "mubashar",
                Email = "mubashar@example.com",
                AgencyName = "Global Travels",
                Status = "Active"
            },
            new {
                AgentName = "yasir",
                UserName = "yasir294",
                Email = "yasir294@gmail.com",
                AgencyName = "Sky Travels",
                Status = "Active"
            }
        };

        public IActionResult Index()
        {
            ViewBag.Agents = agentsList;
            return View();
        }

        public IActionResult AddAgent()
        {
            ViewBag.Agencies = new List<string>
            {
                "Sky Travels",
                "Global Travels",
                "Falcon Tours",
                "Emirates Travel",
                "Gulf Wings"
            };

            return View();
        }

        [HttpPost]
        public IActionResult AddAgent(string agentName, string userName, string email, string agencyName, string password, string status)
        {
            var newAgent = new
            {
                AgentName = agentName ?? "",
                UserName = userName ?? "",
                Email = email ?? "",
                AgencyName = agencyName ?? "",
                Status = status ?? "Active"
            };

            agentsList.Add(newAgent);

            return RedirectToAction("Index");
        }
    }
}