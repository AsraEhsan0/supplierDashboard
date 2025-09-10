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
    public class VoidServicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VoidServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/voidservices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VoidServiceSummaryDto>>> GetVoidServices()
        {
            return await _context.VoidServices
                .Include(vs => vs.VoidServiceAgencies)
                    .ThenInclude(vsa => vsa.Agency)
                .Select(vs => new VoidServiceSummaryDto
                {
                    Id = vs.Id,
                    GroupName = vs.GroupName,
                    VoidFee = vs.VoidFee,
                    VoidType = vs.VoidType,
                    IsActive = vs.IsActive,
                    CreatedAt = vs.CreatedAt,
                    AgencyCount = vs.VoidServiceAgencies.Count,
                    AgencyNames = vs.VoidServiceAgencies.Select(vsa => vsa.Agency.AgencyName).ToList()
                })
                .OrderByDescending(vs => vs.CreatedAt)
                .ToListAsync();
        }

        // GET: api/voidservices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VoidServiceDto>> GetVoidService(string id)
        {
            var voidService = await _context.VoidServices
                .Include(vs => vs.VoidServiceAgencies)
                    .ThenInclude(vsa => vsa.Agency)
                .Where(vs => vs.Id == id)
                .Select(vs => new VoidServiceDto
                {
                    Id = vs.Id,
                    GroupName = vs.GroupName,
                    VoidFee = vs.VoidFee,
                    VoidType = vs.VoidType,
                    IsActive = vs.IsActive,
                    CreatedAt = vs.CreatedAt,
                    Agencies = vs.VoidServiceAgencies.Select(vsa => new AgencyDto
                    {
                        Id = vsa.Agency.Id,
                        AgencyName = vsa.Agency.AgencyName,
                        Email = vsa.Agency.Email,
                        Phone = vsa.Agency.Phone,
                        Address = vsa.Agency.Address,
                        IsActive = vsa.Agency.IsActive,
                        CreatedAt = vsa.Agency.CreatedAt
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (voidService == null)
            {
                return NotFound();
            }

            return voidService;
        }

        // POST: api/voidservices
        [HttpPost]
        public async Task<ActionResult<VoidServiceDto>> PostVoidService(CreateUpdateVoidServiceDto dto)
        {
            // Validate void type
            if (dto.VoidType != "Amount" && dto.VoidType != "Percentage")
            {
                return BadRequest("VoidType must be either 'Amount' or 'Percentage'");
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
            var existingVoidService = await _context.VoidServices
                .FirstOrDefaultAsync(vs => vs.GroupName.ToLower() == dto.GroupName.ToLower());

            if (existingVoidService != null)
            {
                return BadRequest("A void service group with this name already exists");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var voidService = new VoidService
                {
                    Id = Guid.NewGuid().ToString(),
                    GroupName = dto.GroupName.Trim(),
                    VoidFee = dto.VoidFee,
                    VoidType = dto.VoidType,
                    IsActive = dto.IsActive,
                    CreatedAt = DateTime.UtcNow
                };

                _context.VoidServices.Add(voidService);
                await _context.SaveChangesAsync();

                // Create voidservice-agency relationships
                var voidServiceAgencies = dto.AgencyIds.Select(agencyId => new VoidServiceAgency
                {
                    Id = Guid.NewGuid().ToString(),
                    VoidServiceId = voidService.Id,
                    AgencyId = agencyId,
                    AssignedAt = DateTime.UtcNow
                }).ToList();

                _context.VoidServiceAgencies.AddRange(voidServiceAgencies);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                // Return the created void service with agencies
                var result = new VoidServiceDto
                {
                    Id = voidService.Id,
                    GroupName = voidService.GroupName,
                    VoidFee = voidService.VoidFee,
                    VoidType = voidService.VoidType,
                    IsActive = voidService.IsActive,
                    CreatedAt = voidService.CreatedAt,
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

                return CreatedAtAction(nameof(GetVoidService), new { id = voidService.Id }, result);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // PUT: api/voidservices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoidService(string id, CreateUpdateVoidServiceDto dto)
        {
            // Validate void type
            if (dto.VoidType != "Amount" && dto.VoidType != "Percentage")
            {
                return BadRequest("VoidType must be either 'Amount' or 'Percentage'");
            }

            var voidService = await _context.VoidServices
                .Include(vs => vs.VoidServiceAgencies)
                .FirstOrDefaultAsync(vs => vs.Id == id);

            if (voidService == null)
            {
                return NotFound();
            }

            // Check for duplicate group name (excluding current void service)
            var existingVoidService = await _context.VoidServices
                .FirstOrDefaultAsync(vs => vs.GroupName.ToLower() == dto.GroupName.ToLower() && vs.Id != id);

            if (existingVoidService != null)
            {
                return BadRequest("A void service group with this name already exists");
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
                // Update void service properties
                voidService.GroupName = dto.GroupName.Trim();
                voidService.VoidFee = dto.VoidFee;
                voidService.VoidType = dto.VoidType;
                voidService.IsActive = dto.IsActive;

                // Remove existing agency relationships
                _context.VoidServiceAgencies.RemoveRange(voidService.VoidServiceAgencies);

                // Add new agency relationships
                var voidServiceAgencies = dto.AgencyIds.Select(agencyId => new VoidServiceAgency
                {
                    Id = Guid.NewGuid().ToString(),
                    VoidServiceId = voidService.Id,
                    AgencyId = agencyId,
                    AssignedAt = DateTime.UtcNow
                }).ToList();

                _context.VoidServiceAgencies.AddRange(voidServiceAgencies);

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

        // DELETE: api/voidservices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoidService(string id)
        {
            var voidService = await _context.VoidServices
                .Include(vs => vs.VoidServiceAgencies)
                .FirstOrDefaultAsync(vs => vs.Id == id);

            if (voidService == null)
                return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Remove voidservice-agency relationships first
                _context.VoidServiceAgencies.RemoveRange(voidService.VoidServiceAgencies);
                _context.VoidServices.Remove(voidService);

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
    public class CreateUpdateVoidServiceDto
    {
        [Required]
        [MaxLength(200)]
        public string GroupName { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Void fee must be greater than 0")]
        public decimal VoidFee { get; set; }

        [Required]
        public string VoidType { get; set; } // "Amount" or "Percentage"

        public bool IsActive { get; set; } = true;

        [Required]
        [MinLength(1, ErrorMessage = "At least one agency must be selected")]
        public List<string> AgencyIds { get; set; } = new List<string>();
    }

    public class VoidServiceDto
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public decimal VoidFee { get; set; }
        public string VoidType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<AgencyDto> Agencies { get; set; } = new List<AgencyDto>();
    }

    public class VoidServiceSummaryDto
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public decimal VoidFee { get; set; }
        public string VoidType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AgencyCount { get; set; }
        public List<string> AgencyNames { get; set; } = new List<string>();
    }

    public class VoidServiceAgencyDto
    {
        public string Id { get; set; }
        public string VoidServiceId { get; set; }
        public string AgencyId { get; set; }
        public DateTime AssignedAt { get; set; }
        public VoidServiceSummaryDto VoidService { get; set; }
        public AgencyDto Agency { get; set; }
    }
}