using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SupplierDashboard.Models.Entities
{
    public class Agent
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string AgentName { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public string AgencyId { get; set; }

        // Navigation property
        [ForeignKey("AgencyId")]
        [JsonIgnore]
        public Agency Agency { get; set; }
    }
}