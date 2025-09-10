using System.ComponentModel.DataAnnotations;

namespace SupplierDashboard.Models.Entities
{
    public class VoidService
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string GroupName { get; set; }

        [Required]
        public decimal VoidFee { get; set; }

        [Required]
        [MaxLength(20)]
        public string VoidType { get; set; } // "Amount" or "Percentage"

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation property for many-to-many relationship
        public virtual ICollection<VoidServiceAgency> VoidServiceAgencies { get; set; } = new List<VoidServiceAgency>();
    }
}