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
    public class DiscountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DiscountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/discounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountSummaryDto>>> GetDiscounts()
        {
            return await _context.Discounts
                .Include(d => d.DiscountAgencies)
                    .ThenInclude(da => da.Agency)
                .Select(d => new DiscountSummaryDto
                {
                    Id = d.Id,
                    GroupName = d.GroupName,
                    DiscountFee = d.DiscountFee,
                    DiscountType = d.DiscountType,
                    IsActive = d.IsActive,
                    CreatedAt = d.CreatedAt,
                    AgencyCount = d.DiscountAgencies.Count,
                    AgencyNames = d.DiscountAgencies.Select(da => da.Agency.AgencyName).ToList()
                })
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        // GET: api/discounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DiscountDto>> GetDiscount(string id)
        {
            var discount = await _context.Discounts
                .Include(d => d.DiscountAgencies)
                    .ThenInclude(da => da.Agency)
                .Where(d => d.Id == id)
                .Select(d => new DiscountDto
                {
                    Id = d.Id,
                    GroupName = d.GroupName,
                    DiscountFee = d.DiscountFee,
                    DiscountType = d.DiscountType,
                    IsActive = d.IsActive,
                    CreatedAt = d.CreatedAt,
                    Agencies = d.DiscountAgencies.Select(da => new AgencyDto
                    {
                        Id = da.Agency.Id,
                        AgencyName = da.Agency.AgencyName,
                        Email = da.Agency.Email,
                        Phone = da.Agency.Phone,
                        Address = da.Agency.Address,
                        IsActive = da.Agency.IsActive,
                        CreatedAt = da.Agency.CreatedAt
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (discount == null)
            {
                return NotFound();
            }

            return discount;
        }

        // POST: api/discounts
        [HttpPost]
        public async Task<ActionResult<DiscountDto>> PostDiscount(CreateUpdateDiscountDto dto)
        {
            // Validate discount type
            if (dto.DiscountType != "Amount" && dto.DiscountType != "Percentage")
            {
                return BadRequest("DiscountType must be either 'Amount' or 'Percentage'");
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
            var existingDiscount = await _context.Discounts
                .FirstOrDefaultAsync(d => d.GroupName.ToLower() == dto.GroupName.ToLower());

            if (existingDiscount != null)
            {
                return BadRequest("A discount group with this name already exists");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var discount = new Discount
                {
                    Id = Guid.NewGuid().ToString(),
                    GroupName = dto.GroupName.Trim(),
                    DiscountFee = dto.DiscountFee,
                    DiscountType = dto.DiscountType,
                    IsActive = dto.IsActive,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Discounts.Add(discount);
                await _context.SaveChangesAsync();

                // Create discount-agency relationships
                var discountAgencies = dto.AgencyIds.Select(agencyId => new DiscountAgency
                {
                    Id = Guid.NewGuid().ToString(),
                    DiscountId = discount.Id,
                    AgencyId = agencyId,
                    AssignedAt = DateTime.UtcNow
                }).ToList();

                _context.DiscountAgencies.AddRange(discountAgencies);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                // Return the created discount with agencies
                var result = new DiscountDto
                {
                    Id = discount.Id,
                    GroupName = discount.GroupName,
                    DiscountFee = discount.DiscountFee,
                    DiscountType = discount.DiscountType,
                    IsActive = discount.IsActive,
                    CreatedAt = discount.CreatedAt,
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

                return CreatedAtAction(nameof(GetDiscount), new { id = discount.Id }, result);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // PUT: api/discounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiscount(string id, CreateUpdateDiscountDto dto)
        {
            // Validate discount type
            if (dto.DiscountType != "Amount" && dto.DiscountType != "Percentage")
            {
                return BadRequest("DiscountType must be either 'Amount' or 'Percentage'");
            }

            var discount = await _context.Discounts
                .Include(d => d.DiscountAgencies)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (discount == null)
            {
                return NotFound();
            }

            // Check for duplicate group name (excluding current discount)
            var existingDiscount = await _context.Discounts
                .FirstOrDefaultAsync(d => d.GroupName.ToLower() == dto.GroupName.ToLower() && d.Id != id);

            if (existingDiscount != null)
            {
                return BadRequest("A discount group with this name already exists");
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
                // Update discount properties
                discount.GroupName = dto.GroupName.Trim();
                discount.DiscountFee = dto.DiscountFee;
                discount.DiscountType = dto.DiscountType;
                discount.IsActive = dto.IsActive;

                // Remove existing agency relationships
                _context.DiscountAgencies.RemoveRange(discount.DiscountAgencies);

                // Add new agency relationships
                var discountAgencies = dto.AgencyIds.Select(agencyId => new DiscountAgency
                {
                    Id = Guid.NewGuid().ToString(),
                    DiscountId = discount.Id,
                    AgencyId = agencyId,
                    AssignedAt = DateTime.UtcNow
                }).ToList();

                _context.DiscountAgencies.AddRange(discountAgencies);

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

        // DELETE: api/discounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(string id)
        {
            var discount = await _context.Discounts
                .Include(d => d.DiscountAgencies)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (discount == null)
                return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Remove discount-agency relationships first
                _context.DiscountAgencies.RemoveRange(discount.DiscountAgencies);
                _context.Discounts.Remove(discount);

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
    public class CreateUpdateDiscountDto
    {
        [Required]
        [MaxLength(200)]
        public string GroupName { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Discount fee must be greater than 0")]
        public decimal DiscountFee { get; set; }

        [Required]
        public string DiscountType { get; set; } // "Amount" or "Percentage"

        public bool IsActive { get; set; } = true;

        [Required]
        [MinLength(1, ErrorMessage = "At least one agency must be selected")]
        public List<string> AgencyIds { get; set; } = new List<string>();
    }

    public class DiscountDto
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public decimal DiscountFee { get; set; }
        public string DiscountType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<AgencyDto> Agencies { get; set; } = new List<AgencyDto>();
    }

    public class DiscountSummaryDto
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public decimal DiscountFee { get; set; }
        public string DiscountType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AgencyCount { get; set; }
        public List<string> AgencyNames { get; set; } = new List<string>();
    }

    public class DiscountAgencyDto
    {
        public string Id { get; set; }
        public string DiscountId { get; set; }
        public string AgencyId { get; set; }
        public DateTime AssignedAt { get; set; }
        public DiscountSummaryDto Discount { get; set; }
        public AgencyDto Agency { get; set; }
    }
}