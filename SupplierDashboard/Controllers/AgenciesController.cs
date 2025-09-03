using Microsoft.AspNetCore.Mvc;

namespace SupplierDashboard.Controllers
{
    public class AgenciesController : Controller
    {
        private static List<dynamic> agenciesList = new List<dynamic>
        {
            new {
                AgencyName = "global",
                Email = "global123@gmail.com",
                Phone = "90274012",
                Address = "bahrain manma",
                Status = "Inactive"
            },
            new {
                AgencyName = "sky",
                Email = "sky123@gmail.com",
                Phone = "48764665",
                Address = "manama",
                Status = "Inactive"
            },
            new {
                AgencyName = "Global Travels",
                Email = "global@travel.com",
                Phone = "1234567890",
                Address = "Manama, Bahrain",
                Status = "Inactive"
            },
            new {
                AgencyName = "easy travels",
                Email = "easy123@gmail.com",
                Phone = "48764665",
                Address = "dubai",
                Status = "Inactive"
            },
            new {
                AgencyName = "Sky Travels",
                Email = "skytravel123@gmail.com",
                Phone = "9078675",
                Address = "pakistan,toba tek singh",
                Status = "Inactive"
            }
        };

        public IActionResult Index()
        {
            ViewBag.Agencies = agenciesList;
            return View();
        }

        public IActionResult AddAgency()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAgency(string agencyName, string email, string phone, string address, bool active = false)
        {
            // Create new agency object
            var newAgency = new
            {
                AgencyName = agencyName ?? "",
                Email = email ?? "",
                Phone = phone ?? "",
                Address = address ?? "",
                Status = active ? "Active" : "Inactive"
            };

            agenciesList.Add(newAgency);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ToggleStatus(int index)
        {
            if (index >= 0 && index < agenciesList.Count)
            {
                var agency = agenciesList[index];
                var newStatus = agency.Status == "Active" ? "Inactive" : "Active";

                var updatedAgency = new
                {
                    AgencyName = agency.AgencyName,
                    Email = agency.Email,
                    Phone = agency.Phone,
                    Address = agency.Address,
                    Status = newStatus
                };

                agenciesList[index] = updatedAgency;
            }

            return RedirectToAction("Index");
        }
    }
}