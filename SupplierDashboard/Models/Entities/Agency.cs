using System.ComponentModel.DataAnnotations;

namespace SupplierDashboard.Models.Entities
{
    public class Agency
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string AgencyName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        public bool IsActive { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual ICollection<Agent> Agents { get; set; } = new List<Agent>();
        public virtual ICollection<MarkupAgency> MarkupAgencies { get; set; } = new List<MarkupAgency>();
        public virtual ICollection<DiscountAgency> DiscountAgencies { get; set; } = new List<DiscountAgency>();
        public virtual ICollection<VoidServiceAgency> VoidServiceAgencies { get; set; } = new List<VoidServiceAgency>();
    }
}