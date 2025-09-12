using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplierDashboard.Models.Entities
{
    public class WalletTransaction
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string SubAgencyId { get; set; }
        public virtual SubAgency SubAgency { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string TransactionType { get; set; }

        [MaxLength(100)]
        public string TransactionSubType { get; set; } 

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentMode { get; set; } 

        [MaxLength(100)]
        public string DocumentNumbers { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

    }
}