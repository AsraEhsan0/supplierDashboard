using Microsoft.AspNetCore.Identity;
using SupplierDashboard.Enems;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplierDashboard.Models.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string? MiddleName { get; set; }

        // User Rights
        public bool AccountActive { get; set; } = true;
        public bool AllowBookUnderCancellationPolicy { get; set; } = false;
        public bool AllowCancellationAfterVoucher { get; set; } = false;
        public bool ConsultantReceiveBookingEmail { get; set; } = false;

        // User Role Type
        [Required]
        public UserRoleType RoleType { get; set; }

        // Company Details
        [MaxLength(100)]
        public string? Timezone { get; set; }

        [MaxLength(20)]
        public string? CompanyPhone { get; set; }

        [MaxLength(50)]
        public string? AccountingId { get; set; }

        // Foreign Key to Agency
        public string? AgencyId { get; set; }

        [ForeignKey("AgencyId")]
        public virtual Agency? Agency { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}