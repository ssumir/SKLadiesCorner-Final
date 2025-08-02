using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SKLadiesCorner.Models
{
    public class ShoppingCart
    {
        [Key]
        public int CartId { get; set; } // Note: this is an item-level ID here, not a cart header ID.

        public int? UserId { get; set; } // Nullable for guest carts
        [ForeignKey("UserId")]
        public User User { get; set; }

        [StringLength(255)]
        public string SessionId { get; set; } // Used for guest carts

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Note: For a single user/session, ProductId should be unique in the cart.
        // This will be handled in DbContext OnModelCreating by defining a composite unique index:
        // For logged-in users: (UserId, ProductId)
        // For guest users: (SessionId, ProductId)
        // This makes this table act as both the "cart header" and "cart items" simultaneously.

    }
}
