using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKLadiesCorner.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderStatus { get; set; }

        [StringLength(100)]
        public string ShippingMethod { get; set; }

        [StringLength(100)]
        public string TrackingNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentStatus { get; set; }

        [StringLength(50)]
        public string PaymentMethod { get; set; }

        // Denormalized Shipping Address
        [Required]
        [StringLength(100)]
        public string ShippingFirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string ShippingLastName { get; set; }
        [Required]
        [StringLength(255)]
        public string ShippingAddressLine1 { get; set; }
        [StringLength(255)]
        public string ShippingAddressLine2 { get; set; }
        [Required]
        [StringLength(100)]
        public string ShippingCity { get; set; }
        [Required]
        [StringLength(100)]
        public string ShippingStateProvince { get; set; }
        [Required]
        [StringLength(20)]
        public string ShippingPostalCode { get; set; }
        [Required]
        [StringLength(100)]
        public string ShippingCountry { get; set; }

        // Denormalized Billing Address
        [Required]
        [StringLength(100)]
        public string BillingFirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string BillingLastName { get; set; }
        [Required]
        [StringLength(255)]
        public string BillingAddressLine1 { get; set; }
        [StringLength(255)]
        public string BillingAddressLine2 { get; set; }
        [Required]
        [StringLength(100)]
        public string BillingCity { get; set; }
        [Required]
        [StringLength(100)]
        public string BillingStateProvince { get; set; }
        [Required]
        [StringLength(20)]
        public string BillingPostalCode { get; set; }
        [Required]
        [StringLength(100)]
        public string BillingCountry { get; set; }

        // Navigation Property
        //public ICollection<OrderItem> OrderItems { get; set; }
    }
}
