using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplierDashboard.Data;
using SupplierDashboard.Models;
using SupplierDashboard.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace SupplierDashboard.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarkupsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MarkupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/markups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarkupSummaryDto>>> GetMarkups()
        {
            return await _context.Markups
                .Include(m => m.MarkupAgencies)
                    .ThenInclude(ma => ma.Agency)
                .Select(m => new MarkupSummaryDto
                {
                    Id = m.Id,
                    GroupName = m.GroupName,
                    MarkupFee = m.MarkupFee,
                    MarkupType = m.MarkupType,
                    IsActive = m.IsActive,
                    CreatedAt = m.CreatedAt,
                    AgencyCount = m.MarkupAgencies.Count,
                    AgencyNames = m.MarkupAgencies.Select(ma => ma.Agency.AgencyName).ToList()
                })
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        // GET: api/markups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MarkupDto>> GetMarkup(string id)
        {
            var markup = await _context.Markups
                .Include(m => m.MarkupAgencies)
                    .ThenInclude(ma => ma.Agency)
                .Where(m => m.Id == id)
                .Select(m => new MarkupDto
                {
                    Id = m.Id,
                    GroupName = m.GroupName,
                    MarkupFee = m.MarkupFee,
                    MarkupType = m.MarkupType,
                    IsActive = m.IsActive,
                    CreatedAt = m.CreatedAt,
                    Agencies = m.MarkupAgencies.Select(ma => new AgencyDto
                    {
                        Id = ma.Agency.Id,
                        AgencyName = ma.Agency.AgencyName,
                        Email = ma.Agency.Email,
                        Phone = ma.Agency.Phone,
                        Address = ma.Agency.Address,
                        IsActive = ma.Agency.IsActive,
                        CreatedAt = ma.Agency.CreatedAt
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (markup == null)
            {
                return NotFound();
            }

            return markup;
        }

        // POST: api/markups
        [HttpPost]
        public async Task<ActionResult<MarkupDto>> PostMarkup(CreateUpdateMarkupDto dto)
        {
            // Validate markup type
            if (dto.MarkupType != "Amount" && dto.MarkupType != "Percentage")
            {
                return BadRequest("MarkupType must be either 'Amount' or 'Percentage'");
            }

            // Validate agencies exist
            var existingAgencies = await _context.Agencies
                .Where(a => dto.AgencyIds.Contains(a.Id) && a.IsActive)
                .ToListAsync();

            if (existingAgencies.Count != dto.AgencyIds.Count)
            {
                var invalidIds = dto.AgencyIds.Except(existingAgencies.Select(a => a.Id)).ToList();
                return BadRequest($"Invalid or inactive agency IDs: {string.Join(", ", invalidIds)}");
            }

            // Check for duplicate group name
            var existingMarkup = await _context.Markups
                .FirstOrDefaultAsync(m => m.GroupName.ToLower() == dto.GroupName.ToLower());

            if (existingMarkup != null)
            {
                return BadRequest("A markup group with this name already exists");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var markup = new Markup
                {
                    Id = Guid.NewGuid().ToString(),
                    GroupName = dto.GroupName.Trim(),
                    MarkupFee = dto.MarkupFee,
                    MarkupType = dto.MarkupType,
                    IsActive = dto.IsActive,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Markups.Add(markup);
                await _context.SaveChangesAsync();

                // Create markup-agency relationships
                var markupAgencies = dto.AgencyIds.Select(agencyId => new MarkupAgency
                {
                    Id = Guid.NewGuid().ToString(),
                    MarkupId = markup.Id,
                    AgencyId = agencyId,
                    AssignedAt = DateTime.UtcNow
                }).ToList();

                _context.MarkupAgencies.AddRange(markupAgencies);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                // Return the created markup with agencies
                var result = new MarkupDto
                {
                    Id = markup.Id,
                    GroupName = markup.GroupName,
                    MarkupFee = markup.MarkupFee,
                    MarkupType = markup.MarkupType,
                    IsActive = markup.IsActive,
                    CreatedAt = markup.CreatedAt,
                    Agencies = existingAgencies.Select(a => new AgencyDto
                    {
                        Id = a.Id,
                        AgencyName = a.AgencyName,
                        Email = a.Email,
                        Phone = a.Phone,
                        Address = a.Address,
                        IsActive = a.IsActive,
                        CreatedAt = a.CreatedAt
                    }).ToList()
                };

                return CreatedAtAction(nameof(GetMarkup), new { id = markup.Id }, result);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // PUT: api/markups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarkup(string id, CreateUpdateMarkupDto dto)
        {
            // Validate markup type
            if (dto.MarkupType != "Amount" && dto.MarkupType != "Percentage")
            {
                return BadRequest("MarkupType must be either 'Amount' or 'Percentage'");
            }

            var markup = await _context.Markups
                .Include(m => m.MarkupAgencies)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (markup == null)
            {
                return NotFound();
            }

            // Check for duplicate group name (excluding current markup)
            var existingMarkup = await _context.Markups
                .FirstOrDefaultAsync(m => m.GroupName.ToLower() == dto.GroupName.ToLower() && m.Id != id);

            if (existingMarkup != null)
            {
                return BadRequest("A markup group with this name already exists");
            }

            // Validate agencies exist
            var existingAgencies = await _context.Agencies
                .Where(a => dto.AgencyIds.Contains(a.Id) && a.IsActive)
                .ToListAsync();

            if (existingAgencies.Count != dto.AgencyIds.Count)
            {
                var invalidIds = dto.AgencyIds.Except(existingAgencies.Select(a => a.Id)).ToList();
                return BadRequest($"Invalid or inactive agency IDs: {string.Join(", ", invalidIds)}");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Update markup properties
                markup.GroupName = dto.GroupName.Trim();
                markup.MarkupFee = dto.MarkupFee;
                markup.MarkupType = dto.MarkupType;
                markup.IsActive = dto.IsActive;

                // Remove existing agency relationships
                _context.MarkupAgencies.RemoveRange(markup.MarkupAgencies);

                // Add new agency relationships
                var markupAgencies = dto.AgencyIds.Select(agencyId => new MarkupAgency
                {
                    Id = Guid.NewGuid().ToString(),
                    MarkupId = markup.Id,
                    AgencyId = agencyId,
                    AssignedAt = DateTime.UtcNow
                }).ToList();

                _context.MarkupAgencies.AddRange(markupAgencies);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return NoContent();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // DELETE: api/markups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarkup(string id)
        {
            var markup = await _context.Markups
                .Include(m => m.MarkupAgencies)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (markup == null)
                return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Remove markup-agency relationships first
                _context.MarkupAgencies.RemoveRange(markup.MarkupAgencies);
                _context.Markups.Remove(markup);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return NoContent();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }

    public class CreateUpdateMarkupDto
    {
        [Required]
        [MaxLength(200)]
        public string GroupName { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Markup fee must be greater than 0")]
        public decimal MarkupFee { get; set; }

        [Required]
        public string MarkupType { get; set; } 

        public bool IsActive { get; set; } = true;

        [Required]
        [MinLength(1, ErrorMessage = "At least one agency must be selected")]
        public List<string> AgencyIds { get; set; } = new List<string>();
    }

    public class MarkupDto
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public decimal MarkupFee { get; set; }
        public string MarkupType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<AgencyDto> Agencies { get; set; } = new List<AgencyDto>();
    }

    public class MarkupSummaryDto
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public decimal MarkupFee { get; set; }
        public string MarkupType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AgencyCount { get; set; }
        public List<string> AgencyNames { get; set; } = new List<string>();
    }

    public class MarkupAgencyDto
    {
        public string Id { get; set; }
        public string MarkupId { get; set; }
        public string AgencyId { get; set; }
        public DateTime AssignedAt { get; set; }
        public MarkupSummaryDto Markup { get; set; }
        public AgencyDto Agency { get; set; }
    }
}