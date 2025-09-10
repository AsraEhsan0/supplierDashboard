using System.ComponentModel.DataAnnotations;

namespace SupplierDashboard.Models.Entities
{
    public class VoidServiceAgency
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string VoidServiceId { get; set; }
        public virtual VoidService VoidService { get; set; }

        [Required]
        public string AgencyId { get; set; }
        public virtual Agency Agency { get; set; }

        public DateTime AssignedAt { get; set; }

        // Optional: You can add additional properties specific to this relationship
        // public string AssignedBy { get; set; }
        // public string Notes { get; set; }
    }
}