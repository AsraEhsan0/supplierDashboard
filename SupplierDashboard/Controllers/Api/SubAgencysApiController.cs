using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplierDashboard.Data;
using SupplierDashboard.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace SupplierDashboard.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubAgenciesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SubAgenciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/subagencies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubAgencyDto>>> GetSubAgencies()
        {
            return await _context.SubAgencies
                .Select(sa => new SubAgencyDto
                {
                    Id = sa.Id,
                    AgencyName = sa.AgencyName,
                    Address = sa.Address,
                    City = sa.City,
                    Email = sa.Email,
                    HandlingConsultant = sa.HandlingConsultant,
                    ContactNumber = sa.ContactNumber,
                    Status = sa.Status,
                    CreatedAt = sa.CreatedAt
                })
                .ToListAsync();
        }

        // GET: api/subagencies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubAgencyDto>> GetSubAgency(string id)
        {
            var subAgency = await _context.SubAgencies
                .Where(sa => sa.Id == id)
                .Select(sa => new SubAgencyDto
                {
                    Id = sa.Id,
                    AgencyName = sa.AgencyName,
                    Address = sa.Address,
                    City = sa.City,
                    Email = sa.Email,
                    HandlingConsultant = sa.HandlingConsultant,
                    ContactNumber = sa.ContactNumber,
                    Status = sa.Status,
                    CreatedAt = sa.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (subAgency == null)
            {
                return NotFound();
            }

            return subAgency;
        }

        // POST: api/subagencies
        [HttpPost]
        public async Task<ActionResult<SubAgencyDto>> PostSubAgency(CreateSubAgencyDto dto)
        {
            var subAgency = new SubAgency
            {
                Id = Guid.NewGuid().ToString(),
                AgencyName = dto.AgencyName,
                Address = dto.Address,
                City = dto.City,
                Email = dto.Email,
                HandlingConsultant = dto.HandlingConsultant,
                ContactNumber = dto.ContactNumber,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow
            };

            _context.SubAgencies.Add(subAgency);
            await _context.SaveChangesAsync();

            var result = new SubAgencyDto
            {
                Id = subAgency.Id,
                AgencyName = subAgency.AgencyName,
                Address = subAgency.Address,
                City = subAgency.City,
                Email = subAgency.Email,
                HandlingConsultant = subAgency.HandlingConsultant,
                ContactNumber = subAgency.ContactNumber,
                Status = subAgency.Status,
                CreatedAt = subAgency.CreatedAt
            };

            return CreatedAtAction(nameof(GetSubAgency), new { id = subAgency.Id }, result);
        }

        // PUT: api/subagencies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubAgency(string id, CreateSubAgencyDto dto)
        {
            var subAgency = await _context.SubAgencies.FindAsync(id);
            if (subAgency == null)
            {
                return NotFound();
            }

            subAgency.AgencyName = dto.AgencyName;
            subAgency.Address = dto.Address;
            subAgency.City = dto.City;
            subAgency.Email = dto.Email;
            subAgency.HandlingConsultant = dto.HandlingConsultant;
            subAgency.ContactNumber = dto.ContactNumber;
            subAgency.Status = dto.Status;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/subagencies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubAgency(string id)
        {
            var subAgency = await _context.SubAgencies.FindAsync(id);
            if (subAgency == null)
                return NotFound();

            _context.SubAgencies.Remove(subAgency);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
    public class SubAgencyDto
    {
        public string Id { get; set; }
        public string AgencyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string HandlingConsultant { get; set; }
        public string ContactNumber { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class CreateSubAgencyDto
    {
        public string AgencyName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public string HandlingConsultant { get; set; }

        public string ContactNumber { get; set; }

        public bool Status { get; set; } = true;
    }
}