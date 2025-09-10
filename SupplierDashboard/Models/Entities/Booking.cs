using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplierDashboard.Models.Entities
{
    public class Booking
    {
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupName { get; set; }

        [Required]
        [StringLength(100)]
        public string PassengerName { get; set; }

        [Required]
        [StringLength(10)]
        public string PNR { get; set; }

        [StringLength(50)]
        public string FlightNo { get; set; }

        [StringLength(100)]
        public string Segment { get; set; }

        public int SeatsSold { get; set; } = 0;

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public string AgentId { get; set; }

        // Navigation property
        [ForeignKey("AgentId")]
        public Agent Agent { get; set; }
    }
}