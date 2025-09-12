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
    public class WalletSettingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WalletSettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/walletsettings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WalletSettingSummaryDto>>> GetWalletSettings()
        {
            var query = _context.WalletSetting.AsQueryable();
            return await query
                .OrderBy(ws => ws.PaymentType)
                .Select(ws => new WalletSettingSummaryDto
                {
                    Id = ws.Id,
                    PaymentType = ws.PaymentType,
                    Value = ws.Value,
                    ValueType = ws.ValueType,
                    IsActive = ws.IsActive
                })
                .ToListAsync();
        }

        // GET: api/walletsettings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WalletSettingDto>> GetWalletSetting(string id)
        {
            var setting = await _context.WalletSetting
                .Where(ws => ws.Id == id)
                .Select(ws => new WalletSettingDto
                {
                    Id = ws.Id,
                    PaymentType = ws.PaymentType,
                    Value = ws.Value,
                    ValueType = ws.ValueType,
                    IsActive = ws.IsActive
                })
                .FirstOrDefaultAsync();

            if (setting == null)
            {
                return NotFound();
            }

            return setting;
        }

        // POST: api/walletsettings
        [HttpPost]
        public async Task<ActionResult<WalletSettingDto>> PostWalletSetting(CreateUpdateWalletSettingDto dto)
        {
            // Validate value type
            if (dto.ValueType != "Amount" && dto.ValueType != "Percentage")
            {
                return BadRequest("ValueType must be either 'Amount' or 'Percentage'");
            }

            // Validate percentage values
            if (dto.ValueType == "Percentage" && (dto.Value < 0 || dto.Value > 100))
            {
                return BadRequest("Percentage values must be between 0 and 100");
            }


            var setting = new WalletSetting
            {
                Id = Guid.NewGuid().ToString(),
                PaymentType = dto.PaymentType.Trim(),
                Value = dto.Value,
                ValueType = dto.ValueType,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.WalletSetting.Add(setting);
            await _context.SaveChangesAsync();

            var result = new WalletSettingDto
            {
                Id = setting.Id,
                PaymentType = setting.PaymentType,
                Value = setting.Value,
                ValueType = setting.ValueType,
                IsActive = setting.IsActive,
                CreatedAt = setting.CreatedAt
            };

            return CreatedAtAction(nameof(GetWalletSetting), new { id = setting.Id }, result);
        }

        // PUT: api/walletsettings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWalletSetting(string id, CreateUpdateWalletSettingDto dto)
        {
            // Validate value type
            if (dto.ValueType != "Amount" && dto.ValueType != "Percentage")
            {
                return BadRequest("ValueType must be either 'Amount' or 'Percentage'");
            }

            // Validate percentage values
            if (dto.ValueType == "Percentage" && (dto.Value < 0 || dto.Value > 100))
            {
                return BadRequest("Percentage values must be between 0 and 100");
            }

            var setting = await _context.WalletSetting.FindAsync(id);

            if (setting == null)
            {
                return NotFound();
            }


            // Check for duplicate setting (excluding current one)
            var existingSetting = await _context.WalletSetting
                .FirstOrDefaultAsync(ws=>ws.Id != id);

           

            // Update setting properties
            setting.PaymentType = dto.PaymentType.Trim();
            setting.Value = dto.Value;
            setting.ValueType = dto.ValueType;
            setting.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/walletsettings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWalletSetting(string id)
        {
            var setting = await _context.WalletSetting.FindAsync(id);

            if (setting == null)
                return NotFound();

            _context.WalletSetting.Remove(setting);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/walletsettings/paymenttypes
        [HttpGet("paymenttypes")]
        public ActionResult<IEnumerable<PaymentTypeDto>> GetPaymentTypes()
        {
            var paymentTypes = new List<PaymentTypeDto>
            {
                new PaymentTypeDto { Value = "Wallet", Display = "Wallet" },
                new PaymentTypeDto { Value = "OnlinePayment", Display = "Online Payment" },
                
            };

            return paymentTypes;
        }

    }

    public class CreateUpdateWalletSettingDto
    {
        [Required]
        [MaxLength(100)]
        public string PaymentType { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        public string ValueType { get; set; } 


        public bool IsActive { get; set; } = true;

    }

    public class WalletSettingDto
    {
        public string Id { get; set; }
        public string PaymentType { get; set; }
        public decimal Value { get; set; }
        public string ValueType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class WalletSettingSummaryDto
    {
        public string Id { get; set; }
        public string PaymentType { get; set; }
        public decimal Value { get; set; }
        public string ValueType { get; set; }
        public bool IsActive { get; set; }
    }

    public class PaymentTypeDto
    {
        public string Value { get; set; }
        public string Display { get; set; }
    }

}