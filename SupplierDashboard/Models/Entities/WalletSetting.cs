using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupplierDashboard.Models.Entities
{
    public class WalletSetting
    {
        [Key]
        public string Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string PaymentType { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Value { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string ValueType { get; set; } 
        
        public bool IsActive { get; set; } = true;
      
        public DateTime CreatedAt { get; set; }
       
    }
}