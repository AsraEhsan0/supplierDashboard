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
    public class WalletTransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WalletTransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WalletTransactionSummaryDto>>> GetWalletTransactions()
        {
            var query = _context.WalletTransactions
                .Include(wt => wt.SubAgency)
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var transactions = await query
                .OrderByDescending(wt => wt.TransactionDate)
                .Select(wt => new WalletTransactionSummaryDto
                {
                    Id = wt.Id,
                    SubAgencyName = wt.SubAgency.AgencyName,
                    TransactionDate = wt.TransactionDate,
                    TransactionType = wt.TransactionType,
                    Amount = wt.Amount,
                    PaymentMode = wt.PaymentMode,
                   
                })
                .ToListAsync();

            return transactions;
        }

        // GET: api/wallettransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WalletTransactionDto>> GetWalletTransaction(string id)
        {
            var transaction = await _context.WalletTransactions
                .Include(wt => wt.SubAgency)
                .Where(wt => wt.Id == id)
                .Select(wt => new WalletTransactionDto
                {
                    Id = wt.Id,
                    SubAgencyId = wt.SubAgencyId,
                    SubAgencyName = wt.SubAgency.AgencyName,
                    TransactionDate = wt.TransactionDate,
                    TransactionType = wt.TransactionType,
                    TransactionSubType = wt.TransactionSubType,
                    Amount = wt.Amount,
                    PaymentMode = wt.PaymentMode,
                    DocumentNumbers = wt.DocumentNumbers,
                    Description = wt.Description,
                    CreatedAt = wt.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // POST: api/wallettransactions
        [HttpPost]
        public async Task<ActionResult<WalletTransactionDto>> PostWalletTransaction(CreateUpdateWalletTransactionDto dto)
        {
            var validTransactionTypes = new[] { "Payement", "Recieve" };
            if (!validTransactionTypes.Contains(dto.TransactionType))
            {
                return BadRequest("TransactionType must be either 'Recieve' or 'Payement'");
            }

            var subAgency = await _context.SubAgencies
                .FirstOrDefaultAsync(sa => sa.Id == dto.SubAgencyId);

            if (subAgency == null)
            {
                return BadRequest("Invalid or inactive sub agency ID");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {


                var walletTransaction = new WalletTransaction
                {
                    Id = Guid.NewGuid().ToString(),
                    SubAgencyId = dto.SubAgencyId,
                    TransactionDate = dto.TransactionDate,
                    TransactionType = dto.TransactionType,
                    TransactionSubType = dto.TransactionSubType,
                    Amount = dto.Amount,
                    PaymentMode = dto.PaymentMode,
                    DocumentNumbers = dto.DocumentNumbers?.Trim(),
                    Description = dto.Description?.Trim(),
                    CreatedAt = DateTime.UtcNow
                };

                _context.WalletTransactions.Add(walletTransaction);


                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Return the created transaction
                var result = new WalletTransactionDto
                {
                    Id = walletTransaction.Id,
                    SubAgencyId = walletTransaction.SubAgencyId,
                    SubAgencyName = subAgency.AgencyName,
                    TransactionDate = walletTransaction.TransactionDate,
                    TransactionType = walletTransaction.TransactionType,
                    TransactionSubType = walletTransaction.TransactionSubType,
                    Amount = walletTransaction.Amount,
                    PaymentMode = walletTransaction.PaymentMode,
                    DocumentNumbers = walletTransaction.DocumentNumbers,
                    Description = walletTransaction.Description,
                    CreatedAt = walletTransaction.CreatedAt
                };

                return CreatedAtAction(nameof(GetWalletTransaction), new { id = walletTransaction.Id }, result);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutWalletTransaction(string id, CreateUpdateWalletTransactionDto dto)
        {
            var existingTransaction = await _context.WalletTransactions
                .Include(wt => wt.SubAgency)
                .FirstOrDefaultAsync(wt => wt.Id == id);

            if (existingTransaction == null)
            {
                return NotFound();
            }
            existingTransaction.Amount = dto.Amount;
            existingTransaction.SubAgencyId = dto.SubAgencyId;
            existingTransaction.PaymentMode = dto.PaymentMode;
            existingTransaction.DocumentNumbers = dto.DocumentNumbers?.Trim();
            existingTransaction.Description = dto.Description?.Trim();
            existingTransaction.ModifiedAt = DateTime.UtcNow;
            existingTransaction.CreatedAt = DateTime.UtcNow;
            existingTransaction.TransactionType = dto.TransactionType;
            existingTransaction.TransactionSubType = dto.TransactionSubType;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWalletTransaction(string id)
        {
            var transaction = await _context.WalletTransactions.FindAsync(id);

            if (transaction == null)
                return NotFound();


            _context.WalletTransactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("types")]
        public ActionResult<IEnumerable<TransactionTypeDto>> GetTransactionTypes()
        {
            var transactionTypes = new List<TransactionTypeDto>
            {
                new TransactionTypeDto { Value = "Recieve", Display = "Recieve" },
                new TransactionTypeDto { Value = "Payement", Display = "Payment" }
            };

            return transactionTypes;
        }

        [HttpGet("transactionSubtypes")]
        public ActionResult<IEnumerable<PaymentModeDto>> GetTransanctionSubtypes()
        {
            var transactionSubTypes = new List<PaymentModeDto>
            {
                new PaymentModeDto { Value = "Deposit", Display = "Deposit" },
                new PaymentModeDto { Value = "Refund", Display = "Refund" },
                new PaymentModeDto { Value = "Commission", Display = "Commission" },
                new PaymentModeDto { Value = "Charge", Display = "charge" }
            };

            return transactionSubTypes;
        }

        [HttpGet("paymenttypes")]
        public ActionResult<IEnumerable<PaymentModeDto>> GetPaymentTypes()
        {
            var PaymentTypes = new List<PaymentModeDto>
            {
                new PaymentModeDto { Value = "Cash", Display = "Cash" },
                new PaymentModeDto { Value = "BankTransfer", Display = "Bank Transfer" },
                new PaymentModeDto { Value = "CreditCard", Display = "Credit Card" },
                new PaymentModeDto { Value = "Check", Display = "Check" }
            };

            return PaymentTypes;
        }


    }
    public class CreateUpdateWalletTransactionDto
    {
        [Required]
        public string SubAgencyId { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string TransactionType { get; set; } // Credit, Debit

        [MaxLength(100)]
        public string TransactionSubType { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentMode { get; set; }

        [MaxLength(100)]
        public string DocumentNumbers { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }
    }

    public class WalletTransactionDto
    {
        public string Id { get; set; }
        public string SubAgencyId { get; set; }
        public string SubAgencyName { get; set; }
        public string AgencyName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string TransactionSubType { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; }
        public string DocumentNumbers { get; set; }
        public string Description { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Remarks { get; set; }
    }

    public class WalletTransactionSummaryDto
    {
        public string Id { get; set; }
        public string SubAgencyName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public string PaymentMode { get; set; }
        public bool IsActive { get; set; }
    }

    public class WalletBalanceDto
    {
        public string SubAgencyId { get; set; }
        public string SubAgencyName { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal AvailableBalance { get; set; }
        public DateTime LastTransactionDate { get; set; }
        public int TotalTransactions { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal TotalDebits { get; set; }
    }

    public class TransactionTypeDto
    {
        public string Value { get; set; }
        public string Display { get; set; }
    }

    public class PaymentModeDto
    {
        public string Value { get; set; }
        public string Display { get; set; }
    }

}