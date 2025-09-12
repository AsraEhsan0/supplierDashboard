using System.ComponentModel.DataAnnotations;

namespace SupplierDashboard.Models.Entities
{
    public class SubAgency
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string AgencyName { get; set; }

        [Required]
        [MaxLength(500)]
        public string Address { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(200)]
        public string HandlingConsultant { get; set; }

        [Required]
        [MaxLength(20)]
        public string ContactNumber { get; set; }

        public bool Status { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();
    }
}
