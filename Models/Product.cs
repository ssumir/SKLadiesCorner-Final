using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http; // Make sure this is included for IFormFile

namespace SKLadiesCorner.Models
{
    public class Product
    {
        [Key] // Assuming this is the primary key
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        [Display(Name = "Product Name")] // This is for display in your UI
        public string ProductName { get; set; } = string.Empty;

        [Display(Name = "Current Product Image Path")]
        public string? ImagePath { get; set; }

        [NotMapped]
        [Display(Name = "Upload New Product Image")]
        public IFormFile? ImageFile { get; set; }

        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Color { get; set; }

        [StringLength(50)]
        public string? Size { get; set; }

        public bool IsActive { get; set; } = true;

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [StringLength(50)]
        public string? SKU { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}