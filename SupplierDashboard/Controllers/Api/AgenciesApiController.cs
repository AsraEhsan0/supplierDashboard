using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplierDashboard.Data;
using SupplierDashboard.Models;
using SupplierDashboard.Models.Entities;

namespace SupplierDashboard.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgenciesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AgenciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgencyDto>>> GetAgencies()
        {
            return await _context.Agencies
                .Select(a => new AgencyDto
                {
                    Id = a.Id,
                    AgencyName = a.AgencyName,
                    Email = a.Email,
                    Phone = a.Phone,
                    Address = a.Address,
                    IsActive = a.IsActive,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();
        }

        // GET: api/agencies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AgencyDto>> GetAgency(string id)
        {
            var agency = await _context.Agencies
                .Where(a => a.Id == id)
                .Select(a => new AgencyDto
                {
                    Id = a.Id,
                    AgencyName = a.AgencyName,
                    Email = a.Email,
                    Phone = a.Phone,
                    Address = a.Address,
                    IsActive = a.IsActive,
                    CreatedAt = a.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (agency == null)
            {
                return NotFound();
            }

            return agency;
        }

        // POST: api/agencies
        [HttpPost]
        public async Task<ActionResult<AgencyDto>> PostAgency(CreateAgencyDto dto)
        {
            var agency = new Agency
            {
                Id = Guid.NewGuid().ToString(),
                AgencyName = dto.AgencyName,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Agencies.Add(agency);
            await _context.SaveChangesAsync();

            var result = new AgencyDto
            {
                Id = agency.Id,
                AgencyName = agency.AgencyName,
                Email = agency.Email,
                Phone = agency.Phone,
                Address = agency.Address,
                IsActive = agency.IsActive,
                CreatedAt = agency.CreatedAt
            };

            return CreatedAtAction(nameof(GetAgency), new { id = agency.Id }, result);
        }

        // PUT: api/agencies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgency(string id, CreateAgencyDto dto)
        {
            var agency = await _context.Agencies.FindAsync(id);
            if (agency == null)
            {
                return NotFound();
            }

            agency.AgencyName = dto.AgencyName;
            agency.Email = dto.Email;
            agency.Phone = dto.Phone;
            agency.Address = dto.Address;
            agency.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgency(string id)
        {
            var agency = await _context.Agencies
                .Include(a => a.Agents)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agency == null)
                return NotFound();

            if (agency.Agents.Any())
                return BadRequest("Cannot delete agency with active agents. Please delete agents first.");

            _context.Agencies.Remove(agency);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
    public class CreateAgencyDto
    {
        public string AgencyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }
    public class AgencyDto
    {
        public string Id { get; set; }
        public string AgencyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}