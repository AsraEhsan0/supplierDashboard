using System.ComponentModel.DataAnnotations;

namespace SupplierDashboard.Models.Entities
{
    public class DiscountAgency
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string DiscountId { get; set; }
        public virtual Discount Discount { get; set; }

        [Required]
        public string AgencyId { get; set; }
        public virtual Agency Agency { get; set; }

        public DateTime AssignedAt { get; set; }

        // Optional: You can add additional properties specific to this relationship
        // public string AssignedBy { get; set; }
        // public string Notes { get; set; }
    }
}