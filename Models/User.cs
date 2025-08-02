using System.ComponentModel.DataAnnotations;

namespace SKLadiesCorner.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }

        [Required]
        [StringLength(50)]
        public string UserType { get; set; } // "customer", "admin"

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        //public ICollection<Order> Orders { get; set; }
        //public ICollection<ShoppingCart> ShoppingCarts { get; set; } // Represents items in user's cart
        //public ICollection<Review> Reviews { get; set; }
    }
}
